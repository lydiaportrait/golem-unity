using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;


[CreateAssetMenu(fileName = "AffixData", menuName = "Items/New Affix")]
public class AffixData : ScriptableObject
{
    public int id;
    [TitleGroup("Generation Info")]
    [HorizontalGroup("Generation Info/split")]
    [BoxGroup("Generation Info/split/iLevelMin")]
    [HideLabel]
    public int ilevelMin;
    [BoxGroup("Generation Info/split/iLevelMax")]
    [HideLabel]
    public int ilevelMax;
    [HorizontalGroup("a")]
    [LabelWidth(90)]
    public int tier;
    [HorizontalGroup("a")]
    [LabelWidth(90)]
    public int weight;
    [ListDrawerSettings(Expanded = true)]
    public List<GlobalDefinitions.ItemType> validItemTypes = new List<GlobalDefinitions.ItemType>();

    [TitleGroup("Roll Info")]
    [HorizontalGroup("Roll Info/split")]
    [BoxGroup("Roll Info/split/rollMin")]
    [HideLabel]
    public float minRoll1;
    [BoxGroup("Roll Info/split/rollMax")]
    [HideLabel]
    public float maxRoll1;
    [BoxGroup("Roll Info/split/rollMin")]
    [HideLabel]
    public float minRoll2;
    [BoxGroup("Roll Info/split/rollMax")]
    [HideLabel]
    public float maxRoll2;

    [AssetSelector(Paths = "Assets/ItemData/Affixes")]
    public GameObject affix;
    [Title("Tags")]
    public List<GlobalDefinitions.AffixTag> AffixTags = new List<GlobalDefinitions.AffixTag>();
    [Title("Exclusivity")]
    public List<AffixData> ExclusiveWith = new List<AffixData>();
}
