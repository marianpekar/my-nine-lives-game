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

public class JumpBack : AnimatorCommand
{
    public void Execute(Animator animator)
    {
        animator.SetTrigger("jumpBack");
    } 
}

public class JumpLong : AnimatorCommand
{
    public void Execute(Animator animator)
    {
        animator.SetTrigger("jumpLong");
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
