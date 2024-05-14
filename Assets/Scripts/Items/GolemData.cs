using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GolemBaseData", menuName = "Items/New GolemBase")]
public class GolemData : SerializedScriptableObject
{
    public int id;
    
    [Title("Golem Pieces")]
    public List<GolemLimb> pieces = new List<GolemLimb>();
}

public struct GolemLimb
{
    [HorizontalGroup]
    public GlobalDefinitions.ItemType pieceType;
    [HorizontalGroup]
    public bool isNeccessary;
}
