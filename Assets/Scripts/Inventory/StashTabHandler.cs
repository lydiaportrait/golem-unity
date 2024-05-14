using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StashTabHandler : MonoBehaviour
{
    [SerializeField]
    [ListDrawerSettings(Expanded = true)]
    List<GameObject> StashTabs = new List<GameObject>();
    GameObject CurrentlyShown;
    public void ShowTab(GameObject tab)
    {
        CurrentlyShown.SetActive(false);
        CurrentlyShown = tab;
        tab.SetActive(true);
    }
    private void Awake()
    {
        if (CurrentlyShown == null)
            CurrentlyShown = StashTabs[0];
        foreach (GameObject stashTab in StashTabs)
            stashTab.SetActive(false);
        CurrentlyShown.SetActive(true);
    }
}
