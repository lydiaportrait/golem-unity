using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GolemPiece : MonoBehaviour, IApplyable
{
    public UnityEvent PieceUsed;
    public List<IApplyable> applyables = new List<IApplyable>();

    public void Apply(GolemConstruct golem)
    {
        ItemInfo info = GetComponent<ItemInfo>();
        foreach (IApplyable a in applyables)
        {
            a.Apply(golem);
        }
        foreach (Affix a in info.affixes)
        {
            a.Apply(golem);
        }
    }
}
