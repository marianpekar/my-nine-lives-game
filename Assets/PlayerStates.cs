using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerStates
{
    // Movement
    public float RunningSpeed { get; } = 3.5f;
    public float WalkingSpeed { get; } = 0.8f;
    public float WalkingBackSpeed { get; } = 1f;
    public float RotationSpeed { get; } = 100f;
    public float HighJumpSpeed { get; } = 9f;
    public float LongJumpSpeed { get; } = 4f;
    public float LongJumpDistance { get; } = 12f;
    public float BackJumpSpeed { get; } = 4f;
    public float BackJumpDistance { get; } = 4f;

    // Physics
    public float Gravity { get; set; } = 20f;

    // Binary states
    public bool IsDead { get; set; } = false;
    public bool IsWalking { get; set; } = false;
    public bool IsWalkingBackward { get; set; } = false;
    public bool IsGrounded { get; set; } = true;
    public bool IsCleaning { get; set; } = false;

    // Stealth Level
    public float CurrentStealthLevel { get; set; } = 1f;
    public float StealthLevelHigh { get; } = 0.75f;
    public float StealthLevelLow { get; } = 0.05f;
    public float StealthLevelAddition { get; set; } = 0f;
    public float StealthLevelMaximumAddition { get; } = 0.25f;
    public float AddToStealthAdditionRate { get; } = 1f; // in seconds
    public float AddToStealthAddition { get; } = 0.0005f;

    public float SubstractFromStealthAddition { get; } = 0.05f;


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
