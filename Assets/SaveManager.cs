using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
public class GameStateEvent : UnityEvent<GameState>
{

}
public class SaveManager : Singleton<SaveManager>
{
    public GameStateEvent AboutToSave = new GameStateEvent();
    public GameStateEvent doLoad = new GameStateEvent();
    public GameState state = new GameState();
    [Button]
    void Save(string filePath)
    {
        state = new GameState();
        if (AboutToSave != null)
            AboutToSave.Invoke(state);
        byte[] bytes = SerializationUtility.SerializeValue(this.state, DataFormat.Binary);
        File.WriteAllBytes(filePath, bytes);
    }
    [Button]
    void Load(string filePath = "oo")
    {
        byte[] bytes = File.ReadAllBytes(filePath);
        state = SerializationUtility.DeserializeValue<GameState>(bytes, DataFormat.Binary);
        foreach (LocationState ls in state.locationStates)
            StaticGUIDs.Instance.GetGameObject(ls.gameObjectID).GetComponent<LocationSaver>().Load(ls);
        if (doLoad != null)
            doLoad.Invoke(state);
    }
    
}
[Serializable]
public class GameState
{
    public ResourceState resourceState;
    public InventoryState inventoryState;
    public List<LocationState> locationStates = new List<LocationState>();
}