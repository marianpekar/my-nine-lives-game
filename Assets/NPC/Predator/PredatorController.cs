using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class PredatorController : AIController
{
    public float stopChasingDistance;
    private bool isChasingPlayer = false;
    public float catchDistance = 1.5f;
    new void Start()
    {
        base.Start();
        stopChasingDistance = visibleRadius * 2f;

        PlayerEvents.Singleton.RegisterPlayerDiedActions(StopChasing);
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
        if (CalculateDistanceToPlayer() <= catchDistance)
        {
            PlayerStates.Singleton.RemoveLive();
            StopChasing();
        }
    }

    void CheckForPlayer()
    {
        if (PlayerStates.Singleton.IsDead)
            return;

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
