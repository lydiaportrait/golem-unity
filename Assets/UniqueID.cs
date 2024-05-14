using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEditor;

public class UniqueID : MonoBehaviour
{
    [SerializeField, ReadOnly]
    public string uniqueID = "";
    [OnInspectorGUI]
    private void OnInspectorGUI()
    {
        if(uniqueID == "")
        {
            uniqueID = GUID.Generate().ToString();
        }

    }
    private void Awake() {
        if (!StaticGUIDs.Instance.AllGuids.ContainsKey(uniqueID))
            StaticGUIDs.Instance.AllGuids.Add(uniqueID, gameObject);
        if (!StaticGUIDs.Instance.AllGameObjects.ContainsKey(gameObject))
            StaticGUIDs.Instance.AllGameObjects.Add(gameObject, uniqueID);
    }
#if (UNITY_EDITOR)
    private void Reset()
    {
        uniqueID = GUID.Generate().ToString();
    }
    private void OnValidate()
    {
        if (Event.current != null)
        {
            if (Event.current.type == EventType.ExecuteCommand && Event.current.commandName == "Duplicate")
            {
                uniqueID = GUID.Generate().ToString();

            }
            else if (Event.current.type == EventType.ExecuteCommand && Event.current.commandName == "Paste")
            {
                uniqueID = GUID.Generate().ToString();
            }
        }
    }
#endif
}
