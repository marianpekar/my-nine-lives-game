﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    float wanderRadius = 30f;
    float detectionRadius = 10f;
    float criticalDetectionRadius = 2f;
    float fleeRadius = 30f;
    float stealthLevelDetectionLimit = 0.75f;

    float walkSpeed = 0.5f;
    float walkAngularSpeed = 120f;
    float runSpeed = 6f;
    float runAngularSpeed = 600f;

    float maxIdleTime = 6f;  
    float minIdleTime = 2f;

    NavMeshAgent agent;
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

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
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
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
        if(CloseToPlayer(PlayerStates.Singleton.Position, detectionRadius))
        {
            if(PlayerStates.Singleton.CurrentStealthLevel > stealthLevelDetectionLimit || CloseToPlayer(PlayerStates.Singleton.Position, criticalDetectionRadius))
            {
                Flee(PlayerStates.Singleton.Position);
            }
        }

        if (agent.remainingDistance < 1f)
        {
            Idle();
            Invoke("Walk", Random.Range(minIdleTime, maxIdleTime));
            agent.SetDestination(RandomNavmeshLocation(wanderRadius));
        }
    }
}
