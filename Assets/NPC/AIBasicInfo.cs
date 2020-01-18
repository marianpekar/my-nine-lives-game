using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AIBasicInfo", menuName = "My Nine Lives/AI Basic Info", order = 51)]
public class AIBasicInfo : ScriptableObject
{
    public float wanderRadius = 30f;
    public float visibleRadius = 8f;
    public float fieldOfView = 45f;
    public float detectionRadius = 6f;
    public float criticalDetectionRadius = 3f;
    public float stealthLevelDetectionLimit = 0.75f;
    public float catchDistance = 1f;

    public float walkSpeed = 0.5f;
    public float walkAngularSpeed = 120f;
    public float runSpeed = 6f;
    public float runAngularSpeed = 600f;

    public float maxIdleTime = 6f;
    public float minIdleTime = 2f;
}
