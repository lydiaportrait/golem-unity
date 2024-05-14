using Rewired;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiWindowHandler : Singleton<UiWindowHandler>
{
    [SerializeField] GameObject StashWindow;
    [SerializeField] GameObject CraftingWindow;
    private Player player;
    private void Awake()
    {
        player = ReInput.players.GetPlayer(0);
        player.AddInputEventDelegate(ToggleStashWindow, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Stash");
        player.AddInputEventDelegate(ToggleCraftingWindow, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Crafting");
    }
    public void ToggleStashWindow(InputActionEventData obj)
    {
        if (StashWindow.activeSelf)
            StashWindow.SetActive(false);
        else
            StashWindow.SetActive(true);
    }
    public void ToggleCraftingWindow(InputActionEventData obj)
    {
        if (CraftingWindow.activeSelf)
            CraftingWindow.SetActive(false);
        else
            CraftingWindow.SetActive(true);
    }
}
