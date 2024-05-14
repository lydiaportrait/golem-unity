using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class LocationBalloonInfo : MonoBehaviour
{
    public bool locationIsComplete = false;
    [ListDrawerSettings(Expanded = true)]
    public List<GameObject> connectedLocations = new List<GameObject>();
    public void CompleteLocation(EncounterStage e)
    {
        if (e.SuccessResult.RevealsAdditionalExits)
        {
            //TODO: this is self explanatory
        }
        locationIsComplete = true;
        foreach (GameObject go in connectedLocations) //TODO: make this nice, add functionality for like hidden exits ,, like a nice fade or effect r smth
            go.SetActive(true);
    }
}
