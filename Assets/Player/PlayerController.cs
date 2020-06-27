using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerController : MonoBehaviour
{
    AnimatorCommand run, walk, walkBack, jumpBack, jump, idle, die;
    CharacterController characterController;
    Animator animator;

    [SerializeField]
    private FixedJoystick fixedJoystick;

    private float joystickVerticalThreshold = 0.05f;
    private float joystickHorizontalThreshold = 0.15f;

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
        die = new Die();

        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        PlayerStates.Singleton.IsJumping = false;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (PlayerStates.Singleton.IsDead)
        {
            die.Execute(animator);
            return;
        }

        if (PlayerStates.Singleton.IsPaused) return;

        PlayerStates.Singleton.Position = transform.position;

        PlayerStates.Singleton.IsGrounded = characterController.isGrounded;

        if (PlayerStates.Singleton.IsGrounded)
        {
            // Move
            float verticalAxis = 0;
            if (fixedJoystick.Vertical > joystickVerticalThreshold) verticalAxis = 1;
            else if (fixedJoystick.Vertical < -joystickVerticalThreshold) verticalAxis = -1;

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
                PlayerStates.Singleton.IsRunning = true;
                PlayerStates.Singleton.IsWalkingBackward = false;
                moveDirection *= PlayerStates.Singleton.RunningSpeed + (PlayerStates.Singleton.IsSprinting ? PlayerStates.Singleton.SprintSpeedBoost : 0);
                run.Execute(animator);
            }
            else
            {
                PlayerStates.Singleton.IsWalkingBackward = false;
                PlayerStates.Singleton.IsRunning = false;
                idle.Execute(animator);
            }
        }

        // Gravity
        moveDirection.y -= PlayerStates.Singleton.Gravity * Time.deltaTime;
        characterController.Move(moveDirection * Time.deltaTime);

        // Rotation
        float horizontalAxis = 0;
        if (fixedJoystick.Horizontal > joystickHorizontalThreshold) horizontalAxis = fixedJoystick.Horizontal;
        else if (fixedJoystick.Horizontal < -joystickHorizontalThreshold) horizontalAxis = fixedJoystick.Horizontal;

        transform.Rotate(0, horizontalAxis * PlayerStates.Singleton.RotationSpeed, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.FromToRotation(transform.up, GetHitNormal()) * transform.rotation, 5 * Time.deltaTime);
    }

    public void Walk(bool enabled)
    {
        PlayerStates.Singleton.IsWalking = enabled;
    }

    public void Sprint(bool enabled)
    {
        if (PlayerStates.Singleton.Stamina > 0f)
            PlayerStates.Singleton.IsSprinting = enabled;
        else
            PlayerStates.Singleton.IsSprinting = enabled;
    }

    public void Jump()
    {
        if (!PlayerStates.Singleton.IsJumping &&
            PlayerStates.Singleton.IsWalking &&
            PlayerStates.Singleton.Stamina >= PlayerStates.Singleton.StaminaNeededForJump &&
            !PlayerStates.Singleton.IsWalkingBackward)
        {
            PlayerStates.Singleton.IsJumping = true;
            Invoke("SetIsJumpingToFalse", 2.8f * Time.timeScale);
            PlayerStates.Singleton.Stamina -= PlayerStates.Singleton.StaminaNeededForJump;
            moveDirection.y = PlayerStates.Singleton.JumpHeight;
            moveDirection.z = PlayerStates.Singleton.JumpDistance;
            moveDirection = transform.TransformDirection(moveDirection);
            jump.Execute(animator);

            characterController.Move(moveDirection * Time.deltaTime);
        }
        else if (PlayerStates.Singleton.IsWalkingBackward)
        {
            moveDirection.y = PlayerStates.Singleton.BackJumpSpeed;
            moveDirection.z = -PlayerStates.Singleton.BackJumpDistance;
            moveDirection = transform.TransformDirection(moveDirection);
            jumpBack.Execute(animator);
            characterController.Move(moveDirection * Time.deltaTime);
        }
    }

    private void SetIsJumpingToFalse()
    {
        PlayerStates.Singleton.IsJumping = false;
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
