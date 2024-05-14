using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemTooltipConstructor : MonoBehaviour, ITooltipConstructor
{
    [SerializeField] private ItemInfo info;
    [SerializeField] private GolemInfo gInfo;
    public string ReturnTooltipText(bool isAdvancedView)
    {
        string tooltipText = "";
        tooltipText += info.itemName + " the " + info.itemType.ToString() + "\n";
        tooltipText += "HP: " + gInfo.golemConstruct.currentHP.ToString() + "/" + gInfo.golemConstruct.stats[(int)GlobalDefinitions.Stats.Health].ReturnNiceValue().ToString() + "\n";
        if (isAdvancedView)
        {
            foreach(Stat stat in gInfo.golemConstruct.stats)
            {
                tooltipText += SplitCamelCase(stat.Name.ToString()) + " : " + stat.ReturnNiceValue().ToString() + "\n";
            }
        }
        foreach (GolemEffectBase gb in gInfo.golemEffects)
            tooltipText += gb.ReturnAffixString(isAdvancedView) + "\n";
        if (isAdvancedView)
        {
            foreach (GlobalDefinitions.ItemTag tag in info.itemTags)
                tooltipText += "(" + tag.ToString() + ") ";
            tooltipText += "\n";
        }
        return tooltipText;
    }
    public string SplitCamelCase(string input)
    {
        return System.Text.RegularExpressions.Regex.Replace(input, "([A-Z])", " $1", System.Text.RegularExpressions.RegexOptions.Compiled).Trim();
    }
}
