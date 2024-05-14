using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class AddAffixConsumable : MonoBehaviour, IRollerSetupDelegates
{
    [SerializeField] InventoryContextMenu cm;
    [ListDrawerSettings(Expanded = true)]
    public List<GlobalDefinitions.ItemType> validTypes = new List<GlobalDefinitions.ItemType>();
    private void Awake()
    {
        Action<GameObject> apply = new Action<GameObject>(ApplyEffects);
        cm.menuItems.Add(new ContextMenuItem("Use", apply));
    }

    private void ApplyEffects(GameObject obj)
    {
        Action<GameObject> x = new Action<GameObject>(Execute);
        ConsumableCraftingManager.Instance.UseItem(x, validTypes);
    }

    private void Execute(GameObject obj)
    {
        ItemInfo info = obj.GetComponent<ItemInfo>();
        if (ItemRoller.Instance.CanItemRollAffix(info))
        {
            ItemRoller.Instance.IndependentRollAffix(info.sourceData, info, this);
            obj.GetComponent<TooltipCallerUi>().BuildTooltip();
            ItemInfo thisinfo = GetComponent<ItemInfo>();
            if (thisinfo.currentStack == 1)
                ConsumableCraftingManager.Instance.DisableClick();
            thisinfo.AddStack(-1);
        }
    }

    public void SetupDelegates()
    {
        ItemRoller.Instance.ModifyAffixEntries += OnlyCertainAffixes;
    }

    private AffixEntry OnlyCertainAffixes(AffixEntry ae)
    {
        /*
        if (ae.affixData.AffixTags.Contains(GlobalDefinitions.AffixTag.Health))
            ItemRoller.Instance.affixIsOK = false;
        return ae;*/
        if(ae.weight > 10)
            ItemRoller.Instance.affixIsOK = false;
        return ae;
    }
}
public interface IRollerSetupDelegates
{
    void SetupDelegates();
}