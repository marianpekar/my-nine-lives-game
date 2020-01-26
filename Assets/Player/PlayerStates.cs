using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerStates
{
    // Movement
    public float RunningSpeed { get; } = 4f;
    public float SprintSpeedBoost { get; } = 3f;
    public float WalkingSpeed { get; } = 0.8f;
    public float WalkingBackSpeed { get; } = 1f;
    public float RotationSpeed { get; } = 3f;
    public float JumpHeight { get; } = 4f;
    public float JumpDistance { get; } = 10f;
    public float BackJumpSpeed { get; } = 4f;
    public float BackJumpDistance { get; } = 4f;
    public bool IsJumping { get; set; }

    // Stamina
    private float stamina = 1.0f;
    public float Stamina { 
        get { 
            return stamina; 
        } 
        set {
            if (value > MaxStamina)
                stamina = MaxStamina;
            else if (value <= 0)
                stamina = 0;
            else
                stamina = value;

            PlayerEvents.Singleton.InvokeStaminaChangedActions();
        } 
    }
    public float MaxStamina { get; set; } = 1.0f;
    public float StaminaConsumptionInterval { get; } = 0.05f;
    public float StaminaIncreaseInterval { get; } = 0.25f;
    public float StaminaStep { get; } = 0.01f;
    public float StaminaNeededForJump { get; } = 0.25f;
    public float StaminaFromCatchedPrey { get; } = 0.5f;

    // Physics
    public float Gravity { get; set; } = 20f;

    // Binary states
    public bool IsDead { get; set; } = false;
    public bool IsWalking { get; set; } = false;
    public bool IsWalkingBackward { get; set; } = false;
    public bool IsSprinting { get; set; } = false;
    public bool IsRunning { get; set; } = false;
    public bool IsGrounded { get; set; } = true;

    // Stealth
    public bool IsStealth { get; set; } = false;

    public Vector3 Position { get; set; }

    // Slow Motion
    public float SlowMotionTimeScale { get; set; } = 0.25f;
    public float SlowMotionDuration { get; set; } = 1f; // in seconds

    // Lives
    const int maxLives = 9;
    const int defaultLifes = 3;
    private int lives = defaultLifes;
    public void RemoveLive()
    {
        if (lives <= 1)
        {
            lives = 0;
            if (IsDead)
                return;
            PlayerEvents.Singleton.InvokePlayerDiedActions();
            IsDead = true;
            return;
        }

        lives--;
        PlayerEvents.Singleton.InvokeLifeRemovedActions();

        FeedLevel = 1.0f;
        Stamina = MaxStamina;
    }

    public void AddLife()
    {
        if(lives < maxLives)
        {
            lives++;
            PlayerEvents.Singleton.InvokeLifeAddedActions();
        }
    }

    public int GetMaxLives()
    {
        return maxLives;
    }

    public int GetLives()
    {
        return lives;
    }
    public int AnotherCats { get; set; }
    public void AnotherCatCatched(int value)
    {
        AnotherCats++;
        Score += value;
    }

    // Chasing
    private bool isChased = false;
    public bool IsChased { 
        get { 
            return isChased; 
        }
        set {
            if (value == true && isChased == false)
            {
                isChased = true;
                PlayerEvents.Singleton.InvokePlayerChasedStartActions();
            } 
            else if (value == false && isChased == true) 
            {
                isChased = false;
                PlayerEvents.Singleton.InvokePlayerChasedEndActions();
            }
                
        }
    }

    // Feeding
    public float EnergyConsumed { get; set; } = 0.01f;
    public float EnergyConsumedInterval { get; set; } = 6f; // in seconds
    public float LowFeedLevel { get; set; } = 0.25f;
    private float maxFeedLevel = 1.0f;
    private float feedLevel = 1.0f;

    public int Preys { get; set; } = 0;
    public int Score { get; set; } = 0;
    public float FeedLevel { 
        get 
        {
            return feedLevel; 
        } 
        set
        {
            feedLevel = value;

            if (feedLevel >= maxFeedLevel) {
                feedLevel = maxFeedLevel;
            } 
            else if (feedLevel <= 0f)
            {
                feedLevel = 0f;
                RemoveLive();
            }

            PlayerEvents.Singleton.InvokeFeedLevelChangedActions();
        } 
    } 

    public void PreyCatched(int value, float nutrition)
    {
        Preys++;
        FeedLevel += nutrition;
        Score += value;
        Stamina += StaminaFromCatchedPrey;

        PlayerEvents.Singleton.InvokeScoreChangedActions();
        PlayerEvents.Singleton.InvokePreyCatchedActions();
    }

    public void Reset()
    {
        FeedLevel = 1.0f;
        Preys = 0;
        Score = 0;
        AnotherCats = 0;
        Stamina = MaxStamina;
        lives = defaultLifes;
        IsDead = false;
        IsPaused = false;
    }

    // Pause
    private bool isPaused = false;
    public bool IsPaused { 
        get { return isPaused; }
        set {
            isPaused = value;
            PlayerEvents.Singleton.InvokePauseActions();
        } 
    }

    public void TooglePause()
    {
        IsPaused = !IsPaused;
        PlayerEvents.Singleton.InvokePauseActions();
    }

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
