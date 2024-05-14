using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationSaver : MonoBehaviour
{
    [SerializeField]
    Location loc;
    [SerializeField]
    LocationBalloonInfo locBalloon;
    private void Awake()
    {
        SaveManager.Instance.AboutToSave.AddListener(Save);
    }


    public void Save(GameState gs)
    {
        LocationState state = new LocationState();
        state.gameObjectID = GetComponent<UniqueID>().uniqueID;
        state.LocationProgress = loc.LocationProgress;
        state.isShown = GetComponent<CanvasGroup>().blocksRaycasts;
        state.BalloonVisible = locBalloon.gameObject.activeSelf;
        gs.locationStates.Add(state);
    }
    public void Load(LocationState ls)
    {
        loc.LocationProgress = ls.LocationProgress;
        if (loc.LocationProgress >= loc.location.MaxProgress)
            loc.ShowBossIndicator();
        if (ls.BalloonVisible)
            locBalloon.gameObject.SetActive(true);
        CanvasGroup cg = GetComponent<CanvasGroup>();
        if (ls.isShown)
        {
            cg.alpha = 1;
            cg.blocksRaycasts = true;
        }
        else
        {
            cg.alpha = 0;
            cg.blocksRaycasts = false;
        }
    }
}
public class LocationState
{
    public string gameObjectID;
    public int LocationProgress;
    public bool isShown;
    public bool BalloonVisible;
}
public interface ISaveable
{
    void Save(GameState gs);
    void Load(GameState gs);
}