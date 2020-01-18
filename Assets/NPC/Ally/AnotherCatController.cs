using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AnotherCatController : AIController
{
    public AnotherCatInfo anotherCatInfo;

    SkinSelector skinSelector;
    private new void Start()
    {
        base.Start();
        skinSelector = GetComponent<SkinSelector>();
    }

    void Flee()
    {
        Vector3 fleeDirection = -base.CalculateDirectionToPlayer().normalized;
        Vector3 newGoalUp = (transform.position + fleeDirection * anotherCatInfo.fleeRadius) + Vector3.up * anotherCatInfo.fleeRadius;

        Vector3 newGoal;
        if (Physics.Raycast(newGoalUp, Vector3.down * anotherCatInfo.fleeRadius, out RaycastHit hit))
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
            agent.speed = info.runSpeed;
            agent.angularSpeed = info.runAngularSpeed;
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
        if (CalculateDistanceToPlayer() < info.catchDistance)
        {
            PlayerStates.Singleton.AnotherCatCatched(anotherCatInfo.value);
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

        if (CloseToPlayer(info.detectionRadius))
        {
            if (!PlayerStates.Singleton.IsStealth || CloseToPlayer(info.criticalDetectionRadius))
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
