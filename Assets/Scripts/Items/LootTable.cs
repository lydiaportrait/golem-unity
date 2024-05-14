using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "LootTable", menuName = "Items/New Loot Table")]
public class LootTable : ScriptableObject
{
    [TableList(AlwaysExpanded = true, ShowIndexLabels = false)]
    public List<LootEntry> loot = new List<LootEntry>();
#if (UNITY_EDITOR)
    [Button]
    void AssignDefaultWeights()
    {
        foreach(LootEntry l in loot)
        {
            if (l.item != null)
                l.weight = l.item.defaultWeight;
        }
    }
#endif
}

[System.Serializable]
public class LootEntry
{
    [HideIf("lootTable")]
    public ItemData item;
    [HideIf("item")]
    public LootTable lootTable;
    public int weight;
}