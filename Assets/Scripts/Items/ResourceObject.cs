using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class ResourceObject : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI numberText;
    [SerializeField] ResourceType resourceType;
    private void Awake()
    {
        NumberResourceManager.Instance.OnResourceChanged.AddListener(ChangeText);
    }

    private void ChangeText(InventoryResource i, int number)
    {
        if(i.ResourceName == resourceType)
            numberText.text = ": " + number.ToString();
    }
}
