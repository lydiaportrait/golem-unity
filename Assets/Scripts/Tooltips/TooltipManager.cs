using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[HideMonoScript]
public class TooltipManager : Singleton<TooltipManager>
{
    [SerializeField, SceneObjectsOnly] private Tooltip _tooltip;

    public void SetPosition()
    {
        _tooltip.SetPosition();
    }

    public void Show(string message)
    {
        _tooltip.Show(message);
    }

    public void Hide()
    {
        _tooltip.Hide();
    }
}
