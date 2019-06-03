using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface AnimatorCommand
{
    void Execute(Animator animator);
}

public class Run : AnimatorCommand
{
    public void Execute(Animator animator)
    {
        animator.SetBool("isRunning", true);
        animator.SetBool("isWalkingBack", false);
        animator.SetBool("isWalking", false);

        if (PlayerStates.Singleton.IsWalking)
            animator.SetBool("isIdleWalk", true);
        else
            animator.SetBool("isIdleWalk", false);
    }
}

public class Walk : AnimatorCommand
{
    public void Execute(Animator animator)
    {
        animator.SetBool("isWalking", true);
        animator.SetBool("isRunning", false);
        animator.SetBool("isWalkingBack", false);

        if (PlayerStates.Singleton.IsWalking)
            animator.SetBool("isIdleWalk", true);
        else
            animator.SetBool("isIdleWalk", false);
    }
}

public class WalkBack : AnimatorCommand
{
    public void Execute(Animator animator)
    {
        animator.SetBool("isWalkingBack", true);
        animator.SetBool("isRunning", false);
        animator.SetBool("isWalking", false);

        if (PlayerStates.Singleton.IsWalking)
            animator.SetBool("isIdleWalk", true);
        else
            animator.SetBool("isIdleWalk", false);
    }
}

public class Jump : AnimatorCommand
{
    public void Execute(Animator animator)
    {
        //Rigidbody rigidbody = gameObject.GetComponent<Rigidbody>();
        //rigidbody.AddForce(Vector3.up * PlayerStates.Singleton.JumpForce, ForceMode.Impulse);
    } 
}

public class Idle : AnimatorCommand
{
    public void Execute(Animator animator)
    {
        animator.SetBool("isRunning", false);
        animator.SetBool("isWalking", false);
        animator.SetBool("isWalkingBack", false);

        if (PlayerStates.Singleton.IsWalking)
            animator.SetBool("isIdleWalk", true);
        else
            animator.SetBool("isIdleWalk", false);
    }
}
