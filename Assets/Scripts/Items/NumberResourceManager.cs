using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;
using UnityEngine.Events;

public class ResourceEvent : UnityEvent<InventoryResource, int>
{

}
public class NumberResourceManager : Singleton<NumberResourceManager>
{
    [ListDrawerSettings(Expanded = true)]
    public List<InventoryResource> resources = new List<InventoryResource>();
    public List<ResourceObject> resourceObjects = new List<ResourceObject>();
    public ResourceEvent OnResourceChanged = new ResourceEvent();
    private void Awake()
    {
        SaveManager.Instance.AboutToSave.AddListener(Save);
        SaveManager.Instance.doLoad.AddListener(Load);
    }

    private void Load(GameState state)
    {
        resources = state.resourceState.resources;
        foreach (InventoryResource r in resources)
            r.AddNumber(0);
    }

    private void Save(GameState state)
    {
        ResourceState rs = new ResourceState
        {
            resources = resources
        };
        state.resourceState = rs;
    }
    public void ResourceChanged(InventoryResource i, int newNumber)
    {
        if (OnResourceChanged != null)
            OnResourceChanged.Invoke(i, newNumber);
    }
    [Button]
    public void AddResource(ResourceType r, int number)
    {
        foreach (InventoryResource i in resources)
            if (i.ResourceName == r)
                i.AddNumber(number);
    }
    public bool PayResource(ResourceType r, int number)
    {
        foreach (InventoryResource i in resources)
            if (i.ResourceName == r)
                return i.PayNumber(number);
        return false;
    }
}
[System.Serializable]
public class ResourceState
{
    public List<InventoryResource> resources = new List<InventoryResource>();
}
public enum ResourceType
{
    Gold,
    Sawdust,
    Wood
}
[System.Serializable]
public class InventoryResource
{
    public ResourceType ResourceName;
    public int currentNumber;
    public float costMultiplier = 1;
    public float gainMultiplier = 1;
    public void AddNumber(int number)
    {
        currentNumber += Mathf.FloorToInt(number * gainMultiplier);
        NumberResourceManager.Instance.ResourceChanged(this, currentNumber);
    }
    public bool PayNumber(int number)
    {
        if((number * costMultiplier) > currentNumber)
        {
            ErrorMessageShower.Instance.ShowError("Not enough " + ResourceName.ToString());
            return false;
        }
        currentNumber -= Mathf.CeilToInt(number * costMultiplier);
        NumberResourceManager.Instance.ResourceChanged(this, currentNumber);
        return true;
    }
    
}