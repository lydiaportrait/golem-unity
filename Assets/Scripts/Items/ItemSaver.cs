using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ItemSaver : SerializedMonoBehaviour
{
    public ItemState Save()
    {
        ItemInfo info = GetComponent<ItemInfo>();
        ItemState state = new ItemState
        {
            slotID = StaticGUIDs.Instance.GetGUID(transform.parent.gameObject),
            itemID = info.id,
            itemName = info.name,
            isStackable = info.isStackable,
            maxStackSize = info.maxStackSize,
            currentStack = info.currentStack,
            maxAffixCount = info.maxAffixCount,
            iLevel = info.iLevel,
            type = info.itemType,
            itemTags = info.itemTags
        };
        foreach (Affix a in info.affixes)
        {
            state.affixes.Add(new AffixItemData(a));
        }
        foreach (Affix a in info.implicitAffixes)
        {
            state.affixes.Add(new AffixItemData(a));
        }

        return state;
    }
    public void Load(ItemState state)
    {
        GameObject slot = StaticGUIDs.Instance.GetGameObject(state.slotID);
        if (slot.GetComponent<InventorySlot>())
        {
            InventorySlot iSlot = slot.GetComponent<InventorySlot>();
            iSlot.currentlyHolding = gameObject;
            iSlot.OnLoad(gameObject);
        }
        transform.localPosition = new Vector2(0, 0);
        transform.localScale = new Vector2(1, 1);
        ItemInfo info = GetComponent<ItemInfo>();
        info.id = state.itemID;
        info.itemName = state.itemName;
        info.isStackable = state.isStackable;
        info.maxStackSize = state.maxStackSize;
        info.currentStack = state.currentStack;
        info.maxAffixCount = state.maxAffixCount;
        info.iLevel = state.iLevel;
        info.itemType = state.type;
        info.itemTags = state.itemTags;
        foreach(AffixItemData a in state.affixes)
        {
            GameObject g = Instantiate(ItemDatabase.Instance.GetAffixById(a.id).affix, transform);
            Affix af = g.GetComponent<Affix>();
            info.affixes.Add(af);
            af.id = a.id;
            af.roll = a.roll;
            af.roll2 = a.roll2;
            af.scaling = a.scaling;
            af.tier = a.tier;
            af.affixData = ItemDatabase.Instance.GetAffixById(a.id);
        }
        foreach (AffixItemData a in state.implicitAffixes)
        {
            GameObject g = Instantiate(ItemDatabase.Instance.GetAffixById(a.id).affix, transform);
            Affix af = g.GetComponent<Affix>();
            info.affixes.Add(af);
            af.id = a.id;
            af.roll = a.roll;
            af.roll2 = a.roll2;
            af.scaling = a.scaling;
            af.tier = a.tier;
            af.affixData = ItemDatabase.Instance.GetAffixById(a.id);
        }
    }
    public virtual void Awake()
    {
        InventorySaver.items.Add(this);
    }

}
[Serializable]
public class ItemState
{
    public int itemID;
    public string itemName;
    public bool isStackable;
    public int maxStackSize;
    public int currentStack;
    public int maxAffixCount;
    public int iLevel;
    public string slotID;
    public GlobalDefinitions.ItemType type;
    public List<GlobalDefinitions.ItemTag> itemTags = new List<GlobalDefinitions.ItemTag>();
    public List<AffixItemData> implicitAffixes = new List<AffixItemData>();
    public List<AffixItemData> affixes = new List<AffixItemData>();
}
[Serializable]
public class AffixItemData
{
    public int id;
    public float roll;
    public float roll2;
    public int tier;
    public float scaling;
    public AffixData affixData;
    public AffixItemData(Affix a)
    {
        id = a.id;
        roll = a.roll;
        roll2 = a.roll2;
        tier = a.tier;
        scaling = a.scaling;
    }
}