using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerStates
{
    public float RunningSpeed { get; } = 3.5f;
    public float WalkingSpeed { get; } = 0.8f;
    public float WalkingBackSpeed { get; } = 1f;
    public float RotationSpeed { get; } = 100f;
    public float HighJumpSpeed { get; } = 10f;
    public float LongJumpSpeed { get; } = 4f;
    public float LongJumpDistance { get; } = 12f;
    public float BackJumpSpeed { get; } = 4f;
    public float BackJumpDistance { get; } = 4f;
    public bool IsDead { get; set; } = false;
    public bool IsWalking { get; set; } = false;
    public bool IsWalkingBackward { get; set; } = false;

    public float Gravity { get; set; } = 20f;

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
