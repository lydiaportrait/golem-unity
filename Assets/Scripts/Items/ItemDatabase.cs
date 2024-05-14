using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class ItemDatabase : Singleton<ItemDatabase>
{
    [AssetList(AutoPopulate = true, Path = "ItemData/AffixData")]
    public List<AffixData> affixes = new List<AffixData>();
    [AssetList(AutoPopulate = true, Path = "ItemData/ItemData")]
    public List<ItemData> items = new List<ItemData>();
    [AssetList(AutoPopulate = true, Path = "ItemData/GolemEffects")]
    public List<GameObject> golemEffects = new List<GameObject>();
    [AssetList(AutoPopulate = true, Path = "ItemData/AffixData", CustomFilterMethod = "HasHealthTag")]
    public List<AffixData> healthAffixes = new List<AffixData>();
    // Start is called before the first frame update
    void Start()
    {

    }

    private bool HasHealthTag(AffixData obj)
    {
        return obj.AffixTags.Contains(GlobalDefinitions.AffixTag.Health);
    }

    public AffixData GetAffixById(int id)
    {
        UberDebug.LogChannel("Items", "Attempting to fetch affix with id " + id.ToString());
        if (id <= affixes.Count - 1)
            return affixes[id];
        return null;
        /*foreach (AffixData a in affixes)
        {
            if (a.id == id)
                return a;
        }
        return null;*/
    }
    public ItemData GetItemByID(int id)
    {
        UberDebug.LogChannel("Items", "Attempting to fetch item with id " + id.ToString());
        if (id <= items.Count - 1)
            return items[id];
        return null;
        /*foreach (ItemData item in items)
        {
            if (item.id == id)
                return item;
        }
        return null;*/
    }
    public GameObject GetGolemEffectByID(int id)
    {
        UberDebug.LogChannel("Items", "Attempting to fetch effect with id " + id.ToString());
        if (id <= golemEffects.Count - 1)
            return golemEffects[id];
        return null;
    }
}
