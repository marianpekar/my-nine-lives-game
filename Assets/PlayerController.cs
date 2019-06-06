using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    AnimatorCommand run, walk, walkBack, jumpBack, jumpLong, jumpHigh, idle;
    CharacterController characterController;
    Animator animator;

    Vector3 moveDirection = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        run = new Run();
        walk = new Walk();
        walkBack = new WalkBack();
        jumpBack = new JumpBack();
        jumpLong = new JumpLong();
        jumpHigh = new JumpHigh();
        idle = new Idle();

        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerStates.Singleton.IsDead) return;
        PlayerStates.Singleton.IsWalking = Input.GetButton("Walk");

        // Move
        if (characterController.isGrounded)
        {
            float verticalAxis = Input.GetAxis("Vertical");

            moveDirection = new Vector3(0, 0, verticalAxis);
            moveDirection = transform.TransformDirection(moveDirection);

            if (verticalAxis > 0 && PlayerStates.Singleton.IsWalking)
            {
                moveDirection *= PlayerStates.Singleton.WalkingSpeed;
                walk.Execute(animator);
            }
            else if (verticalAxis < 0)
            {
                PlayerStates.Singleton.IsWalkingBackward = true;
                moveDirection *= PlayerStates.Singleton.WalkingBackSpeed;
                walkBack.Execute(animator);
            }
            else if (verticalAxis > 0)
            {
                PlayerStates.Singleton.IsWalkingBackward = false;
                moveDirection *= PlayerStates.Singleton.RunningSpeed;
                run.Execute(animator);
            }
            else
            {
                PlayerStates.Singleton.IsWalkingBackward = false;
                idle.Execute(animator);
            }

            // Jumps
            // TODO: JumpIdle and JumpHigh
            if (Input.GetButtonDown("Jump") && PlayerStates.Singleton.IsWalking)
            {
                moveDirection.y = PlayerStates.Singleton.LongJumpSpeed;
                moveDirection.z = PlayerStates.Singleton.LongJumpDistance;
                moveDirection = transform.TransformDirection(moveDirection);
                jumpLong.Execute(animator);
            }
            else if (Input.GetButtonDown("Jump") && PlayerStates.Singleton.IsWalkingBackward)
            {
                moveDirection.y = PlayerStates.Singleton.BackJumpSpeed;
                moveDirection.z = -PlayerStates.Singleton.BackJumpDistance;
                moveDirection = transform.TransformDirection(moveDirection);
                jumpBack.Execute(animator);
            }
            else if (Input.GetButtonDown("Jump"))
            {
                moveDirection.y = PlayerStates.Singleton.HighJumpSpeed;
                jumpHigh.Execute(animator);
            }
        }
        // Gravity
        moveDirection.y -= PlayerStates.Singleton.Gravity * Time.deltaTime;
        characterController.Move(moveDirection * Time.deltaTime);

        // Rotation
        float horizontalAxis = Input.GetAxis("Horizontal");
        transform.Rotate(0, horizontalAxis, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.FromToRotation(transform.up, GetHitNormal()) * transform.rotation, 5 * Time.deltaTime);
    }

    private Vector3 GetHitNormal()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit))
        {
            return hit.normal;
        } else
        {
            return Vector3.zero;
        }
    }
}
