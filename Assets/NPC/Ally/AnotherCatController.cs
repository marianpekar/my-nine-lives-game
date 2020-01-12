﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AnotherCatController : AIController
{
    public float fleeRadius = 8f;
    public int value = 2000;

    SkinSelector skinSelector;
    private new void Start()
    {
        base.Start();
        skinSelector = GetComponent<SkinSelector>();
    }

    void Flee()
    {
        Vector3 fleeDirection = -base.CalculateDirectionToPlayer().normalized;
        Vector3 newGoalUp = (transform.position + fleeDirection * fleeRadius) + Vector3.up * fleeRadius;

        Vector3 newGoal;
        if (Physics.Raycast(newGoalUp, Vector3.down * fleeRadius, out RaycastHit hit))
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

    // Update is called once per frame
    void Update()
    {
        CheckForWanderGoal();
        CheckForBeingCatched();
        CheckForPlayer();

        animator.SetFloat("velocity", agent.velocity.magnitude);
    }

    void CheckForBeingCatched()
    {
        if (CalculateDistanceToPlayer() < catchDistance)
        {
            PlayerStates.Singleton.AnotherCatCatched(value);
            PlayerStates.Singleton.AddLife();
            parentSpawner.Respawn(this.gameObject);
            skinSelector.SelectRandom();
            Walk();
        }
    }

    void CheckForPlayer()
    {
        if (SeePlayer())
            Flee();

        if (CloseToPlayer(detectionRadius))
        {
            if (!PlayerStates.Singleton.IsStealth || CloseToPlayer(criticalDetectionRadius))
                Flee();
        }
    }

    void CheckForWanderGoal()
    {
        if (!agent.isActiveAndEnabled || !agent.isOnNavMesh)
            return;

        if (agent.remainingDistance < 0.5f)
        {
            Idle();
        }
    }
}
