using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;

public class MakeGolem : MonoBehaviour
{
    [SerializeField]
    List<InventorySlot> partSlots;
    [SerializeField]
    InventorySlot outputSlot;
    [SerializeField]
    Toggle sendToggle;
    [SerializeField]
    Inventory golemInventory;
    List<InventorySlot> usedslots = new List<InventorySlot>();
    public void BuildGolem()
    {
        GolemConstruct golemConstruct = new GolemConstruct();
        usedslots = new List<InventorySlot>();
        foreach (InventorySlot i in partSlots)
        {
            if(i.GetComponent<GolemPieceSlot>().isRequired && i.currentlyHolding == null)
            {
                ErrorMessageShower.Instance.ShowError("You must fill the required slots");
                return;
            }
            else if(i.currentlyHolding != null)
            {
                GolemPiece currentPiece = i.currentlyHolding.GetComponent<GolemPiece>();
                currentPiece.Apply(golemConstruct);
                usedslots.Add(i);
            } 
        }
        if(outputSlot.currentlyHolding != null)
        {
            if (sendToggle.isOn)
            {
                if(golemInventory.AddItem(outputSlot.currentlyHolding))
                    outputSlot.currentlyHolding = null;
                else
                {
                    ErrorMessageShower.Instance.ShowError("The golem inventory is full. Please free up some space before attempting to construct a golem.");
                    return;
                }
            }
            else
            {
                ErrorMessageShower.Instance.ShowError("The output slot is full. Please empty this before attempting to construct a golem.");
                return;
            }
        }
        foreach(InventorySlot used in usedslots)
        {
            Destroy(used.currentlyHolding);
            used.currentlyHolding = null;
        }
        ItemData golemTemplate = ItemDatabase.Instance.GetItemByID(1); // 1 = golem
        GameObject g = Instantiate(golemTemplate.item, outputSlot.transform); 
        g.transform.localPosition = new Vector2(0, 0);
        g.transform.localScale = new Vector2(1, 1);
        outputSlot.currentlyHolding = g;
        g.GetComponent<ItemInfo>().Create(golemTemplate, 0, 0, "Golem"); //TODO: golem itemlevel & name gen
        golemConstruct.Create(g);
    }
}
[Serializable]
public class GolemConstruct
{
    [TableList(AlwaysExpanded = true, ShowIndexLabels = false)]
    public List<GolemEffect> GolemEffects = new List<GolemEffect>();
    [TableList(AlwaysExpanded = true, ShowIndexLabels = false), ReadOnly]
    public List<Stat> stats = new List<Stat>();
    
    public List<GlobalDefinitions.ItemTag> GolemTags = new List<GlobalDefinitions.ItemTag>();
    public void Create(GameObject golemBase)
    {
        GolemInfo info = golemBase.GetComponent<GolemInfo>();
        info.golemConstruct = this;
        info.CreateGolemEffects(GolemEffects);
        currentHP = (int)stats[(int)GlobalDefinitions.Stats.Health].currentBaseValue + (int)stats[(int)GlobalDefinitions.Stats.Health].currentModifier;
        //TODO: apply shit here bitch
    }
    public int currentHP;
    public GolemConstruct()
    {
        foreach (int i in Enum.GetValues(typeof(GlobalDefinitions.Stats)))
        {
            stats.Add(new Stat((GlobalDefinitions.Stats)i, 0));
        }
    }
}
[Serializable]
public class GolemEffect
{
    public int EffectID;
    public float roll;
    public float roll2;
    public GolemEffect(int id, float Roll1, float Roll2)
    {
        EffectID = id;
        roll = Roll1;
        roll2 = Roll2;
    }
}
[Serializable]
public class Stat
{
    public GlobalDefinitions.Stats Name;
    public float currentBaseValue;
    public float currentModifier;
    public float multiplier;
    public Stat(GlobalDefinitions.Stats name, float baseValue)
    {
        Name = name;
        currentBaseValue = baseValue;
        currentModifier = 0;
        multiplier = 1;
    }
    public float ReturnNiceValue()
    {
        Debug.Log(currentBaseValue.ToString());
        return (currentBaseValue + currentModifier) * multiplier;
    }
}