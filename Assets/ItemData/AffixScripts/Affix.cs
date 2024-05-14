using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public abstract class Affix : SerializedMonoBehaviour, IApplyable
{
    [ReadOnly]
    public int id;
    [ReadOnly]
    public List<GlobalDefinitions.AffixTag> AffixTags = new List<GlobalDefinitions.AffixTag>();
    [ReadOnly]
    public int tier;
    [ReadOnly]
    public float scaling = 1;
    [ReadOnly]
    public AffixData affixData;
    
    public float roll;
    public float roll2;
    public abstract void Apply(GolemConstruct golem);
    public abstract void Create(AffixData affix, GameObject droppedBy, Location droppedIn);
    public abstract string ReturnAffixString(bool IsAdvancedView);
}
