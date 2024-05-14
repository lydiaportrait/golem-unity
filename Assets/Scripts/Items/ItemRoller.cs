using DataStructures.RandomSelector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Events;

public class ItemRoller : Singleton<ItemRoller>
{
    DynamicRandomSelector<LootEntry> lootSelector = new DynamicRandomSelector<LootEntry>();
    DynamicRandomSelector<AffixEntry> affixSelector = new DynamicRandomSelector<AffixEntry>();
    DynamicRandomSelector<NameEntry> nameSelector = new DynamicRandomSelector<NameEntry>();
    public delegate LootEntry LootEntryDelegate(LootEntry le);
    public LootEntryDelegate ModifyLootEntries;
    public delegate LootEntry LootEntryDelegate2();
    public LootEntryDelegate2 AddThingsToLootTable;
    public delegate AffixEntry AffixEntryDelegate(AffixEntry ae);
    public AffixEntryDelegate ModifyAffixEntries;
    public delegate AffixEntry AffixEntryDelegate2();
    public AffixEntryDelegate2 AddThingsToAffixTable;
    public delegate int IntDelegate(int x);
    public IntDelegate ModifyNumberOfItems;
    public delegate int MultiIntDelegate(int rolled, int min, int max);
    public MultiIntDelegate ModifyAffixSlotCount;
    public MultiIntDelegate ModifyGeneratedAffixCount;

    public AffixItemInfoEvent AffixAdded = new AffixItemInfoEvent();
    public bool affixIsOK = true;
    [Button]
    public void RollItem(LootTable lootTable, GameObject golem, Location location, int iLevel, Transform parentTo)
    {
        NullAllDelegates();
        lootSelector.Clear();
        List<LootEntry> newLootTable = new List<LootEntry>(lootTable.loot);
        if (AddThingsToLootTable != null)
            newLootTable.Add(AddThingsToLootTable.Invoke());
        golem.GetComponent<GolemInfo>().SetupItemDelegates();
        foreach (LootEntry le in newLootTable)
        {
            LootEntry lootEntry = new LootEntry
            {
                item = le.item,
                lootTable = le.lootTable,
                weight = le.weight
            };
            if (ModifyLootEntries != null)
                lootEntry = ModifyLootEntries(lootEntry);
            Debug.Log(lootEntry.weight);
            lootSelector.Add(lootEntry, lootEntry.weight); //add all entries w weight to selector
        }
        if (lootSelector.itemsList.Count == 0)
            return;
        lootSelector.Build();
        LootEntry selectedItem = lootSelector.SelectRandomItem(); //select random item
        if (selectedItem.lootTable != null)
        {
            RollItem(selectedItem.lootTable, golem, location, iLevel, parentTo);          //if it was a loot table then roll on it instead
            return;
        }
        ItemData itemData = selectedItem.item;
        GameObject go = Instantiate(itemData.item, parentTo);
        ItemInfo info = go.GetComponent<ItemInfo>();
        int affixSlotCount = Random.Range(itemData.minAffixSlot, itemData.maxAffixSlot + 1);
        if (ModifyAffixSlotCount != null)
            affixSlotCount = ModifyAffixSlotCount.Invoke(affixSlotCount, itemData.minAffixSlot, itemData.maxAffixSlot);
        string itemName;
        if (itemData.usesStaticName)
            itemName = itemData.itemName;
        else
            itemName = GenerateName(itemData.nameGenerator);
        info.Create(itemData, iLevel, affixSlotCount, itemName);
        if (itemData.HasGarunteedImplicits)
        {
            foreach(AffixData a in itemData.garunteedImplicits)
            {
                GameObject af = Instantiate(a.affix, go.transform);
                Affix affix = af.GetComponent<Affix>();
                affix.Create(a, golem, location);
                info.implicitAffixes.Add(affix);
            }
        }
        if (itemData.CanRollAffixes)
        {
            int affixNumber = Random.Range(0, Mathf.FloorToInt(affixSlotCount / 2) + 1) + Random.Range(1, Mathf.FloorToInt(affixSlotCount / 2) + 1); // split into 2 rolls to make results more commonly the middle, less commonly the edges
            if (ModifyGeneratedAffixCount != null)
                affixNumber = ModifyGeneratedAffixCount.Invoke(affixNumber, 0, affixSlotCount);
            for (int i = 0; i < affixNumber; i++)
            {
                if (info.affixes.Count < info.maxAffixCount)
                    RollAffix(itemData, info, golem, location);
            }
        }
    }

    public void RollAffix(ItemData itemData, ItemInfo itemInfo, GameObject golemDroppedBy, Location loc)
    {
        List<AffixEntry> newAffixEntries = new List<AffixEntry>(itemData.rollableAffixes);
        affixSelector.Clear();
        int iLevel = itemInfo.iLevel;
        if (AddThingsToAffixTable != null)
            newAffixEntries.Add(AddThingsToAffixTable.Invoke());
        foreach (AffixEntry ae in newAffixEntries)
        {
            affixIsOK = true;
            AffixEntry affixEntry = new AffixEntry
            {
                affixData = ae.affixData,
                weight = ae.weight
            };
            if (ModifyAffixEntries != null)
                affixEntry = ModifyAffixEntries(affixEntry);
            foreach (Affix a in itemInfo.affixes)
                if (a.affixData.ExclusiveWith.Contains(affixEntry.affixData))
                    affixIsOK = false;
            if (affixIsOK && affixEntry.affixData.ilevelMin <= iLevel && affixEntry.affixData.ilevelMax >= iLevel)
                affixSelector.Add(affixEntry, affixEntry.weight);
        }
        if (affixSelector.itemsList.Count == 0)
            return;
        affixSelector.Build();
        AffixEntry selectedAffix = affixSelector.SelectRandomItem();
        GameObject go = Instantiate(selectedAffix.affixData.affix, itemInfo.transform);
        Affix affix = go.GetComponent<Affix>();
        affix.Create(selectedAffix.affixData, golemDroppedBy, loc);
        itemInfo.affixes.Add(affix);
        if (AffixAdded != null)
            AffixAdded.Invoke(affix,itemInfo);

    }
    public void IndependentRollAffix(ItemData itemData, ItemInfo itemInfo, IRollerSetupDelegates caller) //TODO add support for caller adding delegates :)
    {
        NullAllDelegates();
        caller.SetupDelegates();
        RollAffix(itemData, itemInfo, null, null);
    }
    public bool CanItemRollAffix(ItemInfo itemInfo)
    {
        if (itemInfo.affixes.Count >= itemInfo.maxAffixCount)
        {
            ErrorMessageShower.Instance.ShowError("That item already is at affix capacity!");
            return false;
        }
        else
            return true;
    }
    public void RollItems(LootTable lootTable, GameObject golem, Location location, int numberOfTimes, int iLevel, Transform parentTo)
    {
        int newNumber = numberOfTimes;
        if(ModifyNumberOfItems != null)
            newNumber = ModifyNumberOfItems.Invoke(newNumber);
        for (int i = 0; i < newNumber; i++)
            RollItem(lootTable, golem, location, iLevel, parentTo);
    }
    public string GenerateName(NameGenerator ng)
    {
        nameSelector.Clear();
        foreach (NameEntry ne in ng.nameEntries)
            nameSelector.Add(ne, ne.weight);
        nameSelector.Build();
        return nameSelector.SelectRandomItem().name;
    }
    public void NullAllDelegates()
    {
        ModifyLootEntries = null;
        AddThingsToLootTable = null;
        ModifyAffixEntries = null;
        AddThingsToAffixTable = null;
        ModifyNumberOfItems = null;
        ModifyAffixSlotCount = null;
        ModifyGeneratedAffixCount = null;
        AffixAdded.RemoveAllListeners();
    }
}
public class AffixItemInfoEvent : UnityEvent<Affix, ItemInfo>
{

}