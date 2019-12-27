using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PredatorController : AIController
{
    void Chase()
    {
        agent.destination = PlayerStates.Singleton.Position;
    }

    // Update is called once per frame
    void Update()
    {
        CheckForGoal();
        CheckForPlayerCatched();
        CheckForPlayer();

        animator.SetFloat("velocity", agent.velocity.magnitude);
    }

    void CheckForPlayerCatched()
    {
        if (CalculateDistanceToPlayer() < 0.5f)
        {
            Debug.Log("Player catched");
            parentSpawner.Respawn(this.gameObject);
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

    void CheckForGoal()
    {
        if (!agent.isActiveAndEnabled || !agent.isOnNavMesh)
            return;

        if (agent.remainingDistance < 0.5f)
        {
            animator.SetBool("isRunning", false);
            Idle();
        }
    }
}
