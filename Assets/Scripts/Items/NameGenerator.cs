using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "NameGenerator", menuName = "Items/New Name Generator")]
public class NameGenerator : ScriptableObject
{
    [TableList(AlwaysExpanded = true)]
    public List<NameEntry> nameEntries = new List<NameEntry>();
}
[System.Serializable]
public class NameEntry
{
    public string name;
    public int weight;
}
