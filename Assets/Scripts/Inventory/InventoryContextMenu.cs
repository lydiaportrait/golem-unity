using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using Rewired.Integration.UnityUI;
using UnityEngine.EventSystems;
using System;

public class InventoryContextMenu : MonoBehaviour, IPointerDownHandler
{
    private Player player;
    public bool contextMenuEnabled = true;
    public List<ContextMenuItem> menuItems = new List<ContextMenuItem>();
    public void OnPointerDown(PointerEventData eventData)
    {
        if (contextMenuEnabled && player.GetButtonDown("RightClick"))
        {
            TooltipManager.Instance.Hide();
            ContextMenuManager.Instance.ShowMenu(menuItems);
        }
    }

    private void Awake()
    {
        player = ReInput.players.GetPlayer(0);
        Action<GameObject> destroy = new Action<GameObject>(DestroyItem);
        foreach (Inventory inven in InventoryManager.Instance.ReturnValidInventories(gameObject))
            menuItems.Add(new ContextMenuItem("Send to: " + inven.InventoryName, new Action<GameObject>((x) => { transform.parent.GetComponent<InventorySlot>().currentlyHolding = null; inven.AddItem(gameObject); })));
        menuItems.Add(new ContextMenuItem("Destroy", destroy));
    }

    private void DestroyItem(GameObject x)
    {
        Destroy(gameObject);
    }
}
