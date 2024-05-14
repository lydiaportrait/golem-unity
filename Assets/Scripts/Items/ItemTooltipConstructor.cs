using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class ItemTooltipConstructor : MonoBehaviour, ITooltipConstructor
{
    [SerializeField] private ItemInfo info;
    public string ReturnTooltipText(bool isAdvancedView)
    {
        string tooltipText = "";
        tooltipText += info.itemName + "\n";
        tooltipText += info.itemType.ToString();
        if(isAdvancedView)
            tooltipText += " of item level " + info.iLevel;
        tooltipText += "\n";
        foreach (Affix a in info.implicitAffixes)
            tooltipText += a.ReturnAffixString(isAdvancedView) + "\n";
        foreach (Affix a in info.affixes)
            tooltipText += a.ReturnAffixString(isAdvancedView) + "\n";
        if (isAdvancedView)
        {
            foreach (GlobalDefinitions.ItemTag tag in info.itemTags)
                tooltipText += "(" + tag.ToString() + ") ";
            tooltipText += "\n";
        }
        return tooltipText;
    }

}
