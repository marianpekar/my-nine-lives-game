using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    AnimatorCommand run, walk, walkBack, jump, idle;
    CharacterController characterController;
    Animator animator;

    Vector3 moveDirection = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        run = new Run();
        walk = new Walk();
        walkBack = new WalkBack();
        jump = new Jump();
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
                moveDirection *= PlayerStates.Singleton.WalkingBackSpeed;
                walkBack.Execute(animator);
            }
            else if (verticalAxis > 0)
            {
                moveDirection *= PlayerStates.Singleton.RunningSpeed;
                run.Execute(animator);
            }
            else
                idle.Execute(animator);

            // Jump
            if (Input.GetButton("Jump"))
                moveDirection.y = PlayerStates.Singleton.JumpSpeed;
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
