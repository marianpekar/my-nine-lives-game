﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    public AIBasicInfo info;

    public Vector3 SpawnPosition { get; set; }
    float maxStuckTime;

    protected NavMeshAgent agent;
    protected Animator animator;
    protected AISpawner parentSpawner;

    // Start is called before the first frame update
    protected void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        animator = GetComponent<Animator>();
        parentSpawner = GetComponentInParent<AISpawner>();

        Walk();
        SetRandomDestination();

        maxStuckTime = 1.5f * info.maxIdleTime;
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
            parentSpawner.Respawn(this.gameObject);
        }
    }

    void SetRandomDestination()
    {
        try
        {
            agent.SetDestination(RandomNavmeshLocation(info.wanderRadius));
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
        Gizmos.DrawWireSphere(transform.position, info.detectionRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, info.detectionRadius);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, info.visibleRadius);
    }

    protected bool CloseToPlayer(float detectionRadius)
    {
        return CalculateDistanceToPlayer() < detectionRadius;
    }

    protected bool SeePlayer()
    {
        if(CalculateDistanceToPlayer() < info.visibleRadius)
        {
            Debug.DrawRay(transform.position, CalculateDirectionToPlayer() * CalculateDistanceToPlayer(), Color.green);
            Debug.DrawRay(transform.position, transform.forward * 3f, Color.blue);
        }

        return CalculateDistanceToPlayer() < info.visibleRadius && 
            (Vector3.Angle(CalculateDirectionToPlayer(), transform.forward) < info.fieldOfView / 2 || 
             Vector3.Angle(CalculateDirectionToPlayer(), transform.forward) > 360 - info.fieldOfView / 2);
    }
    public void Idle()
    {
        animator.SetBool("isRunning", false);
        animator.SetBool("isWalking", false);
        agent.speed = 0;
        agent.angularSpeed = 0;
        SetRandomDestination();
        Invoke("Walk", Random.Range(info.minIdleTime, info.maxIdleTime));
    }

    public void Walk()
    {
        animator.SetBool("isRunning", false);
        animator.SetBool("isWalking", true);
        agent.speed = info.walkSpeed;
        agent.angularSpeed = info.walkAngularSpeed;
    }
    void LateUpdate()
    {
        AlignWithTerrain();
    }

    Vector3 CalculateDirectionToGoal()
    {
        if (agent.path.corners.Length > 1)
            return agent.path.corners[1] - this.transform.position;
        else
            return agent.destination - this.transform.position;
    }

    protected Vector3 CalculateDirectionToPlayer()
    {
        return PlayerStates.Singleton.Position - transform.position;
    }

    protected float CalculateDistanceToPlayer()
    {
        return Vector3.Distance(PlayerStates.Singleton.Position, this.transform.position);
    }

    void AlignWithTerrain()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, LayerMask.NameToLayer("Terrain")))
        {
            Debug.DrawRay(this.transform.position, hit.normal, Color.magenta);

            Vector3 direction = CalculateDirectionToGoal();

            if (agent.speed > 0)
                this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(transform.forward + direction, hit.normal), 1.5f * Time.deltaTime);

            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - hit.distance, this.transform.position.z);
        }
    }

    protected Vector3 GetHitNormal()
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
}
