using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ConsumableCraftingManager : Singleton<ConsumableCraftingManager>
{
    Player player;
    public bool WaitingForClick = false;
    Action<GameObject> functionToDo;
    List<GlobalDefinitions.ItemType> ValidTypes = new List<GlobalDefinitions.ItemType>();
    [SerializeField] GraphicRaycaster gr;
    [SerializeField] EventSystem es;
    private void Awake()
    {
        player = ReInput.players.GetPlayer(0);
    }
    public void UseItem(Action<GameObject> action, List<GlobalDefinitions.ItemType> validtypes)
    {
        WaitingForClick = true;
        functionToDo = action;
        ValidTypes = validtypes;
    }
    // Update is called once per frame
    void Update()
    {
        if (!WaitingForClick)
            return;
        if(player.GetButtonDown("LeftClick") && player.GetButton("Control"))
        {
            DoClick();
        }
        else if (player.GetButtonDown("LeftClick"))
        {
            DoClick();
            StartCoroutine(DelayedDisable());
        }
    }
    public void DisableClick()
    {
        StartCoroutine(DelayedDisable());
    }
    IEnumerator DelayedDisable()
    {
        yield return new WaitUntil(() => { return player.GetButtonUp("LeftClick"); });
        WaitingForClick = false;
    }
    void DoClick()
    {
        ItemInfo iteminfo;
        PointerEventData ped = new PointerEventData(es);
        ped.position = player.controllers.Mouse.screenPosition;
        List<RaycastResult> results = new List<RaycastResult>();
        gr.Raycast(ped, results);
        foreach (RaycastResult r in results)
        {
            Debug.Log(r.gameObject.name);
            iteminfo = r.gameObject.GetComponent<ItemInfo>();
            if (iteminfo != null)
            {
                if (!ValidTypes.Contains(iteminfo.itemType))
                {
                    ErrorMessageShower.Instance.ShowError("That item is not a valid target");
                }
                else
                {
                    functionToDo.Invoke(r.gameObject);
                }
            }
        }
    }
}
