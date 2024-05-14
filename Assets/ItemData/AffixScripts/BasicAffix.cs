using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAffix : Affix, IApplyable
{
    public override void Apply(GolemConstruct golem)
    {
        golem.GolemTags.Add(GlobalDefinitions.ItemTag.Cool);
        golem.stats[(int)GlobalDefinitions.Stats.Attack].currentBaseValue += 5;
        golem.stats[(int)GlobalDefinitions.Stats.Health].currentBaseValue += 50;
        golem.stats[(int)GlobalDefinitions.Stats.ColdResist].currentBaseValue += 5;
        golem.GolemEffects.Add(new GolemEffect(0, 5, 6));
    }
    public override void Create(AffixData creator, GameObject droppedBy, Location droppedIn)
    {
        scaling = 1;
        tier = creator.tier;
        AffixTags = creator.AffixTags;
        id = creator.id;
        affixData = creator;
        roll = Mathf.Round(Random.Range(creator.minRoll1, creator.maxRoll1));
        roll2 = Mathf.Round(Random.Range(creator.minRoll1, creator.maxRoll1));
    }
    public override string ReturnAffixString(bool isAdvancedView)
    {
        string toReturn = "Oooo its a boogle of " + roll.ToString() + " with an oigle of " + roll2.ToString();
        if(isAdvancedView)
            foreach (GlobalDefinitions.AffixTag at in AffixTags)
                toReturn += " (" + at.ToString() + ")";
        return toReturn;
    }
}
