using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Items/New Item")]
public class ItemData : ScriptableObject
{
    public int id;
    public bool usesStaticName = false;
    [ShowIf("usesStaticName")]
    public string itemName;
    [HideIf("usesStaticName")]
    public NameGenerator nameGenerator;
    [HorizontalGroup("a")]
    public GlobalDefinitions.ItemType itemType;
    [HorizontalGroup("a"), LabelWidth(100)]
    public int defaultWeight;
    [HorizontalGroup("b")]
    public bool isStackable = false;
    [HorizontalGroup("b"), ShowIf("isStackable")]
    public int maxStackSize;
    [AssetSelector(Paths = "Assets/ItemData/Items")]
    public GameObject item;
    [Title("Tags"), ListDrawerSettings(Expanded = true)]
    public List<GlobalDefinitions.ItemTag> itemTags = new List<GlobalDefinitions.ItemTag>();
    [Title("Affixes")]
    public bool CanRollAffixes = false;
    [ShowIf("CanRollAffixes")]
    public bool CanHaveImplicits = false;
    [HorizontalGroup("d"), ShowIf("CanHaveImplicits")]
    public bool HasGarunteedImplicits = false;
    [HorizontalGroup("d"), ShowIf("CanHaveImplicits")]
    public bool HasChanceImplicits = false;
    [ShowIf("HasGarunteedImplicits")]
    public List<AffixData> garunteedImplicits = new List<AffixData>();
    [ShowIf("HasChanceImplicits")]
    public float chance = 0;
    public List<AffixEntry> chanceImplicits = new List<AffixEntry>();
    [HorizontalGroup("c"), ShowIf("CanRollAffixes")]
    public int minAffixSlot = 0;
    [HorizontalGroup("c"), ShowIf("CanRollAffixes")]
    public int maxAffixSlot = 0;
    [ShowIf("CanRollAffixes")]
    [TableList(AlwaysExpanded = true, ShowPaging = true, NumberOfItemsPerPage = 20)]
    public List<AffixEntry> rollableAffixes = new List<AffixEntry>();
#if (UNITY_EDITOR)
    [Button, ShowIf("CanRollAffixes")]
    void AssignDefaultWeights()
    {
        foreach (AffixEntry a in rollableAffixes)
        {
            if (a.affixData != null)
                a.weight = a.affixData.weight;
        }
    }
    [Button("Add Affixes w Tag", ButtonStyle.Box, Expanded = true), ShowIf("CanRollAffixes")]
    void AddAffixes(GlobalDefinitions.AffixTag tag)
    {
        List<AffixData> affixes = new List<AffixData>(); ;
        foreach (AffixData ad in ItemDatabase.Instance.affixes)
            if (ad.validItemTypes.Contains(itemType) && (ad.AffixTags.Contains(tag) || tag == GlobalDefinitions.AffixTag.All))
                affixes.Add(ad);
        foreach (AffixData ad in affixes)
            rollableAffixes.Add(new AffixEntry { affixData = ad, weight = 0 });
    }
#endif
}
[System.Serializable]
public class AffixEntry
{
    public AffixData affixData;
    public int weight;
}