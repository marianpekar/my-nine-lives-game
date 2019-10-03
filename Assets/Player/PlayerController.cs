using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    AnimatorCommand run, walk, walkBack, jumpBack, jump, jumpHigh, idle;
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
        jump = new Jump();
        idle = new Idle();

        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        PlayerStates.Singleton.Position = transform.position;

        if (PlayerStates.Singleton.IsDead) return;

        PlayerStates.Singleton.IsWalking = Input.GetButton("Walk");
        PlayerStates.Singleton.IsGrounded = characterController.isGrounded;

        if (PlayerStates.Singleton.IsGrounded)
        {
            // Move
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
            if (Input.GetButtonDown("Jump") && PlayerStates.Singleton.IsWalking)
            {
                Time.timeScale = PlayerStates.Singleton.SlowMotionTimeScale;
                Invoke("ResetTimeScale", PlayerStates.Singleton.SlowMotionDuration);
                moveDirection.y = PlayerStates.Singleton.JumpHeight;
                moveDirection.z = PlayerStates.Singleton.JumpDistance;
                moveDirection = transform.TransformDirection(moveDirection);
                jump.Execute(animator);
            }
            else if (Input.GetButtonDown("Jump") && PlayerStates.Singleton.IsWalkingBackward)
            {
                moveDirection.y = PlayerStates.Singleton.BackJumpSpeed;
                moveDirection.z = -PlayerStates.Singleton.BackJumpDistance;
                moveDirection = transform.TransformDirection(moveDirection);
                jumpBack.Execute(animator);
            }
        }

        // Gravity
        moveDirection.y -= PlayerStates.Singleton.Gravity * Time.deltaTime;
        characterController.Move(moveDirection * Time.deltaTime);

        // Rotation
        float horizontalAxis = Input.GetAxis("Horizontal");
        transform.Rotate(0, horizontalAxis * PlayerStates.Singleton.RotationSpeed, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.FromToRotation(transform.up, GetHitNormal()) * transform.rotation, 5 * Time.deltaTime);
    }

    private void ResetTimeScale()
    {
        Time.timeScale = 1f;
    }

    private Vector3 GetHitNormal()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit))
        {
            //Debug.Log(Vector3.Angle(hit.normal, Vector3.up));
            Debug.DrawRay(hit.point, Vector3.up, Color.red);
            Debug.DrawRay(hit.point, hit.normal, Color.blue);

            if (Vector3.Angle(hit.normal, Vector3.up) < 45)
                return hit.normal;
            else
                return Vector3.zero;
        } else
        {
            return Vector3.zero;
        }
    }
}
