using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class ItemInfo : MonoBehaviour
{
    public int id;
    public string itemName;
    public bool isStackable = false;
    public int maxStackSize;
    public int currentStack;
    public int maxAffixCount;
    public int iLevel;
    public GlobalDefinitions.ItemType itemType;
    [Title("Tags"), ListDrawerSettings(Expanded = true)]
    public List<GlobalDefinitions.ItemTag> itemTags = new List<GlobalDefinitions.ItemTag>();
    public List<Affix> implicitAffixes = new List<Affix>();
    public List<Affix> affixes = new List<Affix>();
    public ItemData sourceData;
    [SerializeField] private GameObject CountCircle;
    [SerializeField] private TextMeshProUGUI CountText;
    public void Create(ItemData itemData, int ilevel, int affixSlotCount, string name)
    {
        id = itemData.id;
        itemName = name;
        isStackable = itemData.isStackable;
        if (!isStackable)
            CountCircle.SetActive(false);
        else
            CountText.text = currentStack.ToString();
        maxStackSize = itemData.maxStackSize;
        maxAffixCount = affixSlotCount;
        iLevel = ilevel;
        itemType = itemData.itemType;
        itemTags = itemData.itemTags;
        sourceData = itemData;
    }
    public void AddStack(int number)
    {
        currentStack += number;
        CountText.text = currentStack.ToString();
        if(currentStack <= 0)
        {
            GetComponentInParent<InventorySlot>().currentlyHolding = null;
            Destroy(gameObject);
        }
    }
}
