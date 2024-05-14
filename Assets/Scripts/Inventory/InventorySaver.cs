using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class InventorySaver : MonoBehaviour
{
    public static List<ItemSaver> items = new List<ItemSaver>();
    public static List<GolemSaver> golems = new List<GolemSaver>();
    InventoryState state = new InventoryState();
    [Button]
    void Save(GameState gs)
    {
        state = new InventoryState();
        state.InventorySaverID = StaticGUIDs.Instance.GetGUID(gameObject);
        foreach(ItemSaver i in items)
        {
            if(i != null)
                if(i.transform.parent.tag != "ItemTray")
                    state.items.Add(i.Save());
        }
        foreach(GolemSaver g in golems)
        {
            if (g != null)
                state.golems.Add(g.Save());
        }
        gs.inventoryState = state;
    }
    [Button]
    void Load(GameState gs)
    {
        state = gs.inventoryState;
        foreach(ItemState item in state.items)
        {
            GameObject slot = StaticGUIDs.Instance.GetGameObject(item.slotID);
            GameObject g = Instantiate(ItemDatabase.Instance.GetItemByID(item.itemID).item, slot.transform);
            g.GetComponent<ItemSaver>().Load(item);
        }
        foreach(GolemState golem in state.golems)
        {
            GameObject g = Instantiate(ItemDatabase.Instance.GetItemByID(golem.itemID).item);
            g.GetComponent<GolemSaver>().Load(golem);
        }
    }
    private void Awake()
    {
        SaveManager.Instance.AboutToSave.AddListener(Save);
        SaveManager.Instance.doLoad.AddListener(Load);
    }
}
[System.Serializable]
public class InventoryState
{
    public string InventorySaverID;
    public List<ItemState> items = new List<ItemState>();
    public List<GolemState> golems = new List<GolemState>();
}
