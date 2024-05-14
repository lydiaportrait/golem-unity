using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Rewired;

public class ContextMenuManager : Singleton<ContextMenuManager>
{
    [SerializeField] GameObject ContextMenu;
    [SerializeField] GameObject ContextMenuButtonPrefab;
    private Player player;
    private ItemContextMenu itemContextMenu;
    public bool isShowing;
    public void DisplayContextMenu(List<ContextMenuItem> menuItems)
    {
        itemContextMenu.SetPosition();
        foreach(ContextMenuItem item in menuItems)
        {
            GameObject go = Instantiate(ContextMenuButtonPrefab, ContextMenu.transform);
            Button button = go.GetComponent<Button>();
            TextMeshProUGUI text = go.GetComponentInChildren<TextMeshProUGUI>();
            text.text = item.text;
            button.onClick.AddListener(delegate { item.action(ContextMenu); });
        }
        isShowing = true;
    }
    public void HideContextMenu()
    {
        foreach (Transform child in ContextMenu.transform)
            Destroy(child.gameObject);
        ContextMenu.SetActive(false);
        isShowing = false;
    }
    public void ShowMenu(List<ContextMenuItem> menuItems)
    {
        StartCoroutine("DelayedShow",menuItems);
    }
    private void Awake()
    {
        itemContextMenu = ContextMenu.GetComponent<ItemContextMenu>();
        player = ReInput.players.GetPlayer(0);
        ContextMenu.SetActive(false);
    }
    IEnumerator DelayedShow(List<ContextMenuItem> menuitems)
    {
        yield return new WaitUntil(() => { return player.GetButtonUp("RightClick"); });
        DisplayContextMenu(menuitems);
    }
}
[System.Serializable]
public class ContextMenuItem
{
    // this class - just a box to some data

    public string text;             // text to display on button
    public Action<GameObject> action;    // delegate to method that needs to be executed when button is clicked

    public ContextMenuItem(string text, Action<GameObject> action)
    {
        this.text = text;
        this.action = action;
    }
}
