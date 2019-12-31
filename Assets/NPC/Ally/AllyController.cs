using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyController : AIController
{
    public float interactDistance = 1f;

    new void Start()
    {
        base.Start();
    }

    void Follow()
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
        CheckForInteraction();
        CheckForPlayer();

        animator.SetFloat("velocity", agent.velocity.magnitude);
    }

    void CheckForInteraction()
    {
        if (CalculateDistanceToPlayer() <= interactDistance)
        {
            PlayerStates.Singleton.AddLive();
            parentSpawner.Respawn(this.gameObject);
            Walk();
        }
    }

    void CheckForPlayer()
    {
        if (PlayerStates.Singleton.IsDead)
            return;

        if (SeePlayer())
            Follow();

        if (CloseToPlayer(detectionRadius))
        {
            if (!PlayerStates.Singleton.IsStealth || CloseToPlayer(criticalDetectionRadius))
                Follow();
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
