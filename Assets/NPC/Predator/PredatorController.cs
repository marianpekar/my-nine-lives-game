using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class PredatorController : AIController
{
    public float stopChasingDistance = 20f;
    void Chase()
    {
        agent.destination = PlayerStates.Singleton.Position;
        agent.speed = runSpeed;
        agent.angularSpeed = runAngularSpeed;
        animator.SetBool("isRunning", true);
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
        if ((agent.destination == PlayerStates.Singleton.Position && CalculateDistanceToPlayer() < stopChasingDistance) || PlayerStates.Singleton.IsDead)
            Walk();
    }

    void CheckForPlayerCatched()
    {
        if (CalculateDistanceToPlayer() < 1f)
        {
            PlayerStates.Singleton.RemoveLive();
            Walk();
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
