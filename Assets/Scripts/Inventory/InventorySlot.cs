using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[System.Serializable]
public class GameobjectEvent : UnityEvent<GameObject>
{

}

public class InventorySlot : MonoBehaviour
{
    public GameobjectEvent gameObjectDropped;
    public GameobjectEvent gameObjectRemoved;
    public GameobjectEvent gameObjectLoaded;
    public GameObject currentlyHolding;
    public bool containsMultiple = false;
    public bool isCharacterPanel = false;
    public bool isDroppable = true;
    public List<GlobalDefinitions.ItemType> slotType = new List<GlobalDefinitions.ItemType>();
    public void OnDrop(GameObject dropped)
    {
        Debug.Log("added 2 slot");
        if (gameObjectDropped != null)
            gameObjectDropped.Invoke(dropped);
    }
    public void OnRemove(GameObject removed)
    {
        Debug.Log("removed from slot" + gameObject.name);
        if (gameObjectRemoved != null)
            gameObjectRemoved.Invoke(removed);
    }
    public void OnLoad(GameObject obj)
    {
        if (gameObjectLoaded != null)
            gameObjectLoaded.Invoke(obj);
    }
    public void UpdateHolding()
    {
        if (transform.childCount == 0)
            currentlyHolding = null;
        else
            currentlyHolding = transform.GetChild(0).gameObject;
    }
}
