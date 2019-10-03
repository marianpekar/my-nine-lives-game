using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerStates
{
    // Movement
    public float RunningSpeed { get; } = 3.5f;
    public float WalkingSpeed { get; } = 0.8f;
    public float WalkingBackSpeed { get; } = 1f;
    public float RotationSpeed { get; } = 2f;
    public float JumpHeight { get; } = 4f;
    public float JumpDistance { get; } = 10f;
    public float BackJumpSpeed { get; } = 4f;
    public float BackJumpDistance { get; } = 4f;

    // Physics
    public float Gravity { get; set; } = 20f;

    // Binary states
    public bool IsDead { get; set; } = false;
    public bool IsWalking { get; set; } = false;
    public bool IsWalkingBackward { get; set; } = false;
    public bool IsGrounded { get; set; } = true;

    // Stealth
    public bool IsStealth { get; set; } = false;

    public Vector3 Position { get; set; }

    // Slow Motion
    public float SlowMotionTimeScale { get; set; } = 0.25f;
    public float SlowMotionDuration { get; set; } = 1f; // in seconds


    private static PlayerStates instance;

    public static PlayerStates Singleton
    {
        get
        {
            if(instance == null)
            {
                instance = new PlayerStates();
            }

            return instance;
        }
    }
}
