using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AIPreyInfo", menuName = "My Nine Lives/AI Prey Info", order = 52)]
public class AIPreyInfo : ScriptableObject
{
    public float fleeRadius = 60f;
    public float nutrition = 0.1f;
    public int value = 10;
}
