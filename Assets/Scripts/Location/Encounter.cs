using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;

[CreateAssetMenu(fileName = "Encounter", menuName = "Locations/New Encounter")]
[Serializable]
public class Encounter : SerializedScriptableObject
{
    //[TableList(ShowIndexLabels = true)]
    public string EncounterTitle;
    [ListDrawerSettings(ShowIndexLabels = true, NumberOfItemsPerPage = 1, DraggableItems = false, Expanded = true)]
    public List<EncounterStage> EncounterStages = new List<EncounterStage>();
}
[Serializable]
public class EncounterStage
{
    [TitleGroup("Stage Info")]
    [LabelWidth(100)]
    public string StageTitle;
    [TitleGroup("Stage Info"), LabelWidth(100)]
    public Sprite StageImage;
    [HorizontalGroup("a")]
    public int StageHP;
    [HorizontalGroup("a"), SuffixLabel("s", Overlay = true)]
    public float BaseActionLength;
    [TableList(AlwaysExpanded = true)]
    public List<StatTest> statTests = new List<StatTest>();
    public EncounterResult SuccessResult = new EncounterResult();
    public EncounterResult FailureResult = new EncounterResult();
}
[Serializable]
public class StatTest
{
    public GlobalDefinitions.Stats statToTest;
    public int difficultyThreshold;
}
[Serializable]
public class EncounterResult
{
    [TitleGroup("Loot")]
    [HorizontalGroup("Loot/split")]
    [BoxGroup("Loot/split/Min Roll")]
    public int minRolls;
    [HorizontalGroup("Loot/split")]
    [BoxGroup("Loot/split/Max Roll")]
    public int maxRolls;
    public int baseItemLevel;
    public LootTable lootTable;
    public int LocationProgress;
    [TitleGroup("Damage")]
    [HorizontalGroup("Damage/split")]
    [BoxGroup("Damage/split/Damage")]
    public int damageNumber;
    [HorizontalGroup("Damage/split")]
    [BoxGroup("Damage/split/Type")]
    public GlobalDefinitions.DamageTypes damageType;
    public bool RevealsAdditionalExits = false;
    [ShowIf("RevealsAdditionalExits")]
    public int exitIndex;
}
