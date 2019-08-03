using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    public float wanderRadius = 30f;
    public float detectionRadius = 6f;
    public float criticalDetectionRadius = 2f;
    public float fleeRadius = 60f;
    public float stealthLevelDetectionLimit = 0.75f;

    public float walkSpeed = 0.5f;
    public float walkAngularSpeed = 120f;
    public float runSpeed = 6f;
    public float runAngularSpeed = 600f;

    public float maxIdleTime = 6f;
    public float minIdleTime = 2f;

    NavMeshAgent agent;
    Animator animator;
    PreySpawner parentSpawner;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        parentSpawner = GetComponentInParent<PreySpawner>();

        Walk();
        agent.SetDestination(RandomNavmeshLocation(wanderRadius));
    }

    public Vector3 RandomNavmeshLocation(float radius)
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += transform.position;
        NavMeshHit hit;
        Vector3 finalPosition = Vector3.zero;

        if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1))
            finalPosition = hit.position;

        return finalPosition;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, criticalDetectionRadius);
    }

    bool CloseToPlayer(Vector3 position, float detectionRadius)
    {
        return (Vector3.Distance(position, transform.position) < detectionRadius);
    }

    void Flee(Vector3 position)
    {
        Vector3 fleeDirection = (transform.position - position).normalized;
        Vector3 newGoalUp = (transform.position + fleeDirection * fleeRadius) + Vector3.up * fleeRadius;

        Vector3 newGoal;
        RaycastHit hit;
        if (Physics.Raycast(newGoalUp, Vector3.down * fleeRadius, out hit))
            newGoal = hit.point;
        else
            newGoal = Vector3.zero;
   

        Debug.DrawLine(transform.position, newGoalUp, Color.red);
        Debug.DrawLine(newGoalUp, newGoal, Color.blue);

        NavMeshPath path = new NavMeshPath();
        agent.CalculatePath(newGoal, path);

        if (path.status != NavMeshPathStatus.PathInvalid)
        {
            agent.SetDestination(path.corners[path.corners.Length - 1]);
            animator.SetTrigger("run");
            agent.speed = runSpeed;
            agent.angularSpeed = runAngularSpeed;
        }
    }

    public void Idle()
    {
        animator.SetTrigger("idle");
        agent.speed = 0;
        agent.angularSpeed = 0;
    }

    public void Walk()
    {
        animator.SetTrigger("walk");
        agent.speed = walkSpeed;
        agent.angularSpeed = walkAngularSpeed;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        CheckForBeingEaten();
        CheckForDanger();
        CheckForGoal();
    }

    void CheckForBeingEaten()
    {
        if (Vector3.Distance(this.transform.position, PlayerStates.Singleton.Position) < 0.5f)
        {
            Debug.Log("This agent has been eaten");
            this.transform.position = parentSpawner.CalculateSpawnPosition();
            Walk();
            agent.SetDestination(RandomNavmeshLocation(wanderRadius));
        }
    }

    void CheckForDanger()
    {
        if (CloseToPlayer(PlayerStates.Singleton.Position, detectionRadius))
        {
            if (PlayerStates.Singleton.CurrentStealthLevel > stealthLevelDetectionLimit || CloseToPlayer(PlayerStates.Singleton.Position, criticalDetectionRadius))
            {
                Flee(PlayerStates.Singleton.Position);
            }
        }
    }

    void CheckForGoal()
    {
        if (agent.remainingDistance < 1f)
        {
            Idle();
            Invoke("Walk", Random.Range(minIdleTime, maxIdleTime));
            agent.SetDestination(RandomNavmeshLocation(wanderRadius));
        }
    }
}
