using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testDelegateEffect : GolemEffectBase
{
    private GolemInfo gi;
    private int weight = 100;
    private int tempAddition = 0;
    private void Awake()
    {
        gi = transform.parent.GetComponent<GolemInfo>();
        gi.AddToItemRoller.AddListener(AddMethodToItemroller);
        gi.AddToLocationRoller.AddListener(AddMethodToLocRoler);
        gi.RemoveListeners.AddListener(RemoveListeners);
        
    }

    private void RemoveListeners(Location loc)
    {
        loc.ProgressBarWasFilled.RemoveListener(IncreasePower);
    }

    private void AddMethodToLocRoler(Location loc)
    {
        loc.ProgressBarWasFilled.AddListener(IncreasePower);
        loc.EncounterEnded.AddListener(ResetPower);
    }

    private void ResetPower(Encounter arg0)
    {
        tempAddition = 0;
    }

    private void IncreasePower()
    {
        tempAddition += 50;
    }

    void AddMethodToItemroller()
    {
        ItemRoller.Instance.ModifyLootEntries += ModifyWeight;
        ItemRoller.Instance.AddThingsToLootTable += AddLootEntry;
    }

    private LootEntry AddLootEntry()
    {
        Debug.Log("adding additional loot to the table");
        return new LootEntry();
    }

    public LootEntry ModifyWeight(LootEntry le)
    {
        le.weight += weight + tempAddition;
        return le;
    }
    public override string ReturnAffixString(bool IsAdvancedView)
    {
        return "Adds "+ (weight+tempAddition).ToString() + " Weight to loot entries";
    }
}
