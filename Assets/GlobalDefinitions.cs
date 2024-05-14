using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Sirenix.Serialization;

public class GlobalDefinitions : MonoBehaviour
{
    public enum AffixTag
    {
        All,
        Health,
        Mana,
        Chance
    }
    public enum ItemType
    {
        None,
        Core,
        Arm,
        Leg,
        Golem,
        Resource,
        Other
    }
    public enum ItemTag
    {
        Cool,
        Nice,
        Nasty
    }
    public enum Stats
    {
        Health,
        Mana,
        Attack,
        Endurance,
        Perception,
        Stealth,
        Charisma,

        ActionTime,
        PhysicalResist,
        FireResist,
        ColdResist,
        LightningResist
        
    }
    public enum DamageTypes
    {
        Physical,
        Cold,
        Fire,
        Lightning
    }
}

public interface IApplyable
{
    void Apply(GolemConstruct golem);
}


