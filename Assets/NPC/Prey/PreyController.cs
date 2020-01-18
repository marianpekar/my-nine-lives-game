using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PreyController : AIController
{
    public AIPreyInfo preyInfo;
    void Flee()
    {
        Vector3 fleeDirection = -base.CalculateDirectionToPlayer().normalized;
        Vector3 newGoalUp = (transform.position + fleeDirection * preyInfo.fleeRadius) + Vector3.up * preyInfo.fleeRadius;

        Vector3 newGoal;
        if (Physics.Raycast(newGoalUp, Vector3.down * preyInfo.fleeRadius, out RaycastHit hit))
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
        CheckForBeingEaten();
        CheckForDanger();

        animator.SetFloat("velocity", agent.velocity.magnitude);
    }

    void CheckForBeingEaten()
    {
        if (CalculateDistanceToPlayer() < info.catchDistance)
        {
            PlayerStates.Singleton.PreyCatched(preyInfo.value, preyInfo.nutrition);
            parentSpawner.Respawn(this.gameObject);
            Walk();
        }
    }

    void CheckForDanger()
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
