using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : Singleton<InventoryManager>
{
    [ListDrawerSettings(Expanded = true)]
    public List<Inventory> inventories = new List<Inventory>();
    public Inventory recentInventory;
    [Button]
    private void Init()
    {
        foreach (Transform child in transform)
            if (child.GetComponent<Inventory>())
                inventories.Add(child.GetComponent<Inventory>());
    }

    public void AddToSensibleInventory(GameObject item)
    {
        foreach(Inventory inventory in inventories)
        {
            if (inventory.inventoryType.Contains(item.GetComponent<ItemInfo>().itemType))
            {
                inventory.AddItem(item);
                return;
            }
        }
        foreach (Inventory inventory in inventories)
            if (inventory.inventoryType.Contains(GlobalDefinitions.ItemType.None))
                inventory.AddItem(item);
    }
    public void AddToRecentInventory(GameObject item)
    {
        recentInventory.AddItem(item);
    }
    public List<Inventory> ReturnValidInventories(GameObject item)
    {
        List<Inventory> invens = new List<Inventory>();
        foreach (Inventory inventory in inventories)
        {
            if (inventory.inventoryType.Contains(item.GetComponent<ItemInfo>().itemType) || inventory.inventoryType.Contains(GlobalDefinitions.ItemType.None))
            {
                invens.Add(inventory);
            }
        }
        return invens;
    }
}
