using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class PredatorController : AIController
{
    private float stopChasingDistance;
    private bool isChasingPlayer = false;

    new void Start()
    {
        base.Start();
        stopChasingDistance = info.visibleRadius * 3f;

        PlayerEvents.Singleton.RegisterPlayerDiedActions(StopChasing);
        PlayerEvents.Singleton.RegisterPlayerDiedActions(StopAgent);
    }

    void Chase()
    {
        isChasingPlayer = true;
        PlayerStates.Singleton.IsChased = true;
        agent.destination = PlayerStates.Singleton.Position;
        agent.speed = info.runSpeed;
        agent.angularSpeed = info.runAngularSpeed;
        animator.SetBool("isRunning", true);
    }
    void StopChasing()
    {
        isChasingPlayer = false;
        PlayerStates.Singleton.IsChased = false;
    }

    void StopAgent()
    {
        agent.SetDestination(agent.transform.position);
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
        if (CalculateDistanceToPlayer() <= info.catchDistance)
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

        if (CloseToPlayer(info.detectionRadius))
        {
            if (!PlayerStates.Singleton.IsStealth || CloseToPlayer(info.criticalDetectionRadius))
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
