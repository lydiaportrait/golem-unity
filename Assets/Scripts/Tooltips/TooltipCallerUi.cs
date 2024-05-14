using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Rewired;

public class TooltipCallerUi : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private string _message;
    private ITooltipConstructor tooltipMessageConstructor;
    private bool _pointerInside = false;
    private bool isAdvancedView = false;
    private Player player;
    public void OnPointerEnter(PointerEventData eventData)
    {
        _pointerInside = true;
        BuildTooltip();
    }
    public void BuildTooltip()
    {
        if (_pointerInside)
        {
            _message = tooltipMessageConstructor.ReturnTooltipText(isAdvancedView);
            TooltipManager.Instance.Show(_message);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipManager.Instance.Hide();
        _pointerInside = false;
    }

    private void Update()
    {
        if (_pointerInside)
            TooltipManager.Instance.SetPosition();
    }
    private void Awake()
    {
        tooltipMessageConstructor = GetComponent<ITooltipConstructor>();
        player = ReInput.players.GetPlayer(0);
        player.AddInputEventDelegate(OnAdvancedViewDown, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "AdvancedView");
        player.AddInputEventDelegate(OnAdvancedViewUp, UpdateLoopType.Update, InputActionEventType.ButtonJustReleased, "AdvancedView");
    }
    private void OnDestroy()
    {
        if (_pointerInside)
        {
            TooltipManager.Instance.Hide();
            _pointerInside = false;
        }
            
    }
    private void OnAdvancedViewUp(InputActionEventData obj)
    {
        isAdvancedView = false;
        if (_pointerInside)
            BuildTooltip();
    }

    private void OnAdvancedViewDown(InputActionEventData obj)
    {
        isAdvancedView = true;
        if (_pointerInside)
            BuildTooltip();
    }
}
public interface ITooltipConstructor
{
    string ReturnTooltipText(bool isAdvancedView);
}
