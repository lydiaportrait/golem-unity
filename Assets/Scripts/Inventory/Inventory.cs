using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<GlobalDefinitions.ItemType> inventoryType = new List<GlobalDefinitions.ItemType>();
    public string InventoryName = "";
    public List<InventorySlot> slots = new List<InventorySlot>();
    [Button]
    private void Init()
    {
        foreach (Transform child in transform)
            if (child.GetComponent<InventorySlot>())
                slots.Add(child.GetComponent<InventorySlot>());
    }
    public bool AddItem(GameObject itemToAdd)
    {
        ItemInfo itemInfo = itemToAdd.GetComponent<ItemInfo>();
        ItemDrag itemDrag = itemToAdd.GetComponent<ItemDrag>();
        IResourceAddable rl = itemToAdd.GetComponent<IResourceAddable>();
        if (rl != null)
        {
            rl.Add();
            return true;
        }
        if (itemInfo.isStackable)
        {
            foreach(InventorySlot slot in slots)
            {
                if(slot.currentlyHolding != null)
                {
                    ItemInfo i = slot.currentlyHolding.GetComponent<ItemInfo>();
                    if (itemDrag.TryAddStackTo(i))
                    {
                        Destroy(itemToAdd);
                        return true;
                    }
                }
            }
        }
        foreach (InventorySlot slot in slots)
        {
            if (slot.currentlyHolding == null)
            {
                itemToAdd.transform.SetParent(slot.transform);
                itemToAdd.transform.localPosition = new Vector2(0, 0);
                slot.currentlyHolding = itemToAdd;
                return true;
            }
        }
        return false;
    }
}
