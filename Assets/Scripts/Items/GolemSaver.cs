using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemSaver : ItemSaver
{
    public new GolemState Save()
    {
        ItemState item = base.Save();
        GolemState golem = new GolemState(item)
        {
            golemConstruct = GetComponent<GolemInfo>().golemConstruct
        };
        return golem;
    }
    public void Load(GolemState state)
    {
        base.Load(state);
        GolemInfo info = gameObject.GetComponent<GolemInfo>();
        info.golemConstruct = state.golemConstruct;
        info.golemConstruct.Create(gameObject);

    }
    public override void Awake()
    {
        InventorySaver.golems.Add(this);
    }
}

[Serializable]
public class GolemState : ItemState
{
    public GolemConstruct golemConstruct;
    public GolemState(ItemState item)
    {
        itemID = item.itemID;
        slotID = item.slotID;
        type = item.type;
        itemTags = item.itemTags;
        affixes = item.affixes;
    }
}