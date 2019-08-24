﻿using System.Collections;
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

    public Vector3 SpawnPosition { get; set; }
    float maxStuckTime;

    NavMeshAgent agent;
    Animator animator;
    PreySpawner parentSpawner;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        animator = GetComponent<Animator>();
        parentSpawner = GetComponentInParent<PreySpawner>();

        Walk();
        SetRandomDestination();

        maxStuckTime = 1.5f * maxIdleTime;
        SpawnPosition = this.transform.position;
        PerformStuckCheck();
    }

    public void PerformStuckCheck()
    {
        Invoke("RespawnIfStuck", maxStuckTime);
    }

    public void RespawnIfStuck()
    {
        if (Vector3.Distance(SpawnPosition, this.transform.position) < 0.5f)
        {
            Debug.Log("Agent is stuck. Respawn.");
            parentSpawner.Respawn(this.gameObject);
        }
    }

    void SetRandomDestination()
    {
        try
        {
            agent.SetDestination(RandomNavmeshLocation(wanderRadius));
        }
        catch
        {
            Debug.Log("Agent can't set himself a destination. Respawn.");
            parentSpawner.Respawn(this.gameObject);
        }
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
            animator.SetBool("isRunning", true);
            agent.speed = runSpeed;
            agent.angularSpeed = runAngularSpeed;
        }
    }

    public void Idle()
    {
        animator.SetBool("isWalking", false);
        agent.speed = 0;
        agent.angularSpeed = 0;
        SetRandomDestination();
        Invoke("Walk", Random.Range(minIdleTime, maxIdleTime));
    }

    public void Walk()
    {
        animator.SetBool("isWalking", true);
        agent.speed = walkSpeed;
        agent.angularSpeed = walkAngularSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        CheckForGoal();
        CheckForBeingEaten();
        CheckForDanger();

        animator.SetFloat("velocity", agent.velocity.magnitude);
    }

    void LateUpdate()
    {
        AlignWithTerrain();
    }

    void AlignWithTerrain()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, LayerMask.NameToLayer("Terrain")))
        {
            Debug.DrawRay(this.transform.position, hit.normal, Color.magenta);

            Vector3 direction = Vector3.zero;
            if (agent.path.corners.Length > 1)
                 direction = agent.path.corners[1] - this.transform.position;
            else
                 direction = agent.destination - this.transform.position;

            if (agent.speed > 0)
                this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(transform.forward + direction, hit.normal), 1.5f * Time.deltaTime);

            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - hit.distance, this.transform.position.z);
        }
    }

    private Vector3 GetHitNormal()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit))
        {
            //Debug.Log(Vector3.Angle(hit.normal, Vector3.up));
            Debug.DrawRay(hit.point, Vector3.up, Color.red);
            Debug.DrawRay(hit.point, hit.normal, Color.blue);

            if (Vector3.Angle(hit.normal, Vector3.up) < 45)
                return hit.normal;
            else
                return Vector3.zero;
        }
        else
        {
            return Vector3.zero;
        }
    }

    void CheckForBeingEaten()
    {
        if (Vector3.Distance(this.transform.position, PlayerStates.Singleton.Position) < 0.5f)
        {
            Debug.Log("This agent has been eaten");
            parentSpawner.Respawn(this.gameObject);
            Walk();
        }
    }

    void CheckForDanger()
    {
        if (CloseToPlayer(PlayerStates.Singleton.Position, detectionRadius))
        {
            if (!PlayerStates.Singleton.IsStealth || CloseToPlayer(PlayerStates.Singleton.Position, criticalDetectionRadius))
                Flee(PlayerStates.Singleton.Position);
        }
    }

    void CheckForGoal()
    {
        if (agent.remainingDistance < 0.5f)
        {
            animator.SetBool("isRunning", false);
            Idle();
        }
    }
}
