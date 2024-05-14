using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class StaticGUIDs : Singleton<StaticGUIDs>
{
    [ShowInInspector, ReadOnly]
    public Dictionary<string, GameObject> AllGuids = new Dictionary<string, GameObject>();
    [ShowInInspector, ReadOnly]
    public Dictionary<GameObject, string> AllGameObjects = new Dictionary<GameObject, string>();
    public GameObject GetGameObject(string guid)
    {
        return AllGuids[guid];
    }
    public string GetGUID(GameObject go)
    {
        return AllGameObjects[go];
    }
    [Button]
    void ClearDataBase()
    {
        AllGuids = new Dictionary<string, GameObject>();
        AllGameObjects = new Dictionary<GameObject, string>();
    }
}
