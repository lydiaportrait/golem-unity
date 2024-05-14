using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;

[CreateAssetMenu(fileName = "Location", menuName = "Locations/New Location")]
public class LocationData : ScriptableObject
{
    [TitleGroup("Location Progress")]
    public bool locationHasProgressEncounters;
    [TitleGroup("Location Progress"), ShowIf("locationHasProgressEncounters")]
    public int MaxProgress;
    [TitleGroup("Location Progress"), ShowIf("locationHasProgressEncounters")]
    [TableList(AlwaysExpanded = true)]
    public List<WeightedEncounter> bossEncounters = new List<WeightedEncounter>();
    [TitleGroup("Locations With Weights")]
    [TableList(AlwaysExpanded = true)]
    public List<WeightedEncounter> encounters = new List<WeightedEncounter>();
}
[Serializable]
public class WeightedEncounter
{
    public Encounter encounter;
    public int weight;
}
