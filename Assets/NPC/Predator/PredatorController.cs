using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class PredatorController : AIController
{
    public float stopChasingDistance;
    private bool isChasingPlayer = false;
    new void Start()
    {
        base.Start();
        stopChasingDistance = visibleRadius * 2f;
    }

    void Chase()
    {
        isChasingPlayer = true;
        PlayerStates.Singleton.IsChased = true;
        agent.destination = PlayerStates.Singleton.Position;
        agent.speed = runSpeed;
        agent.angularSpeed = runAngularSpeed;
        animator.SetBool("isRunning", true);
    }
    void StopChasing()
    {
        isChasingPlayer = false;
        PlayerStates.Singleton.IsChased = false;
        Walk();
    }

    // Update is called once per frame
    void Update()
    {
        CheckForWanderGoal();
        CheckForPlayerCatched();
        CheckForPlayer();
        CheckForStopChasing();

        animator.SetFloat("velocity", agent.velocity.magnitude);
    }

    void CheckForStopChasing()
    {
        if ((isChasingPlayer && CalculateDistanceToPlayer() > stopChasingDistance) || PlayerStates.Singleton.IsDead)
        {
            StopChasing();
        }
    }

    void CheckForPlayerCatched()
    {
        if (CalculateDistanceToPlayer() < 1f)
        {
            PlayerStates.Singleton.RemoveLive();
            StopChasing();
        }
    }

    void CheckForPlayer()
    {
        if (SeePlayer())
            Chase();

        if (CloseToPlayer(detectionRadius))
        {
            if (!PlayerStates.Singleton.IsStealth || CloseToPlayer(criticalDetectionRadius))
                Chase();
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
