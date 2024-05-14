using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceLoot : MonoBehaviour, IResourceAddable
{
    public ResourceType type;
    public int amount;
    public void Add()
    {
        NumberResourceManager.Instance.AddResource(type, amount);
        Destroy(gameObject);
    }
}

public interface IResourceAddable {
    void Add();
}
