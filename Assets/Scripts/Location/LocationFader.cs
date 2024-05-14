using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LocationFader : MonoBehaviour
{
    [SerializeField]
    CanvasGroup cg;
    float alpha;
    bool isShown;
    public void Show()
    {
        DOTween.To(() => cg.alpha, x => cg.alpha = x, 1f, 1f).OnComplete(() => { cg.blocksRaycasts = true; isShown = true; });
    }
    public void Hide()
    {
        DOTween.To(() => cg.alpha, x => cg.alpha = x, 0f, 1f).OnComplete(() => { cg.blocksRaycasts = false; isShown = false; });
    }
    public void Toggle()
    {
        if (isShown)
            Hide();
        else
            Show();
    }
    private void Start()
    {
        if (cg.blocksRaycasts == true)
            isShown = true;
        alpha = cg.alpha;
    }
}
