using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public enum GlobalSkillList
    {
        CarryingCapacity,
        MovingSpeed,
        InteractionSpeed,
        Tinkering,
        Lockpicking,
        Survival,
        Herbalism,
        MaxHealth,
        StaminaRecovery,
        Reading,
        Comprehention,
        ManaPool
    }

public class SkillList
{
    [HideInInspector]
    public static string[] _skills =
{
    new string("CarryingCapacity"),
    new string("MovingSpeed"),
    new string("InteractionSpeed"),
    new string("Tinkering"),
    new string("Lockpicking"),
    new string("Survival"),
    new string("Herbalism"),
    new string("MaxHealth"),
    new string("StaminaRecovery"),
    new string("Reading"),
    new string("Comprehention"),
    new string("ManaPool")
    };
}
