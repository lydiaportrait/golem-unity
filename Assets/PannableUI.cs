using Rewired;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PannableUI : MonoBehaviour, IDragHandler, IPointerDownHandler
{
    [SerializeField] Canvas canvas;
    GraphicRaycaster raycaster;
    [SerializeField] RectTransform rt;
    private Player player;
    private Vector3 Position;
    private Vector3 vectorPosition;
    bool isDraggable;

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDraggable)
            return;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(canvas.transform as RectTransform, player.controllers.Mouse.screenPositionDelta, null, out vectorPosition);
        rt.position += vectorPosition;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        List<RaycastResult> r = new List<RaycastResult>();
        raycaster.Raycast(eventData, r);
        foreach(RaycastResult rr in r)
            if(rr.gameObject.tag != "WorldPannable")
            {
                isDraggable = false;
                return;
            }
        isDraggable = true;
    }

    private void Awake()
    {
        player = ReInput.players.GetPlayer(0);
        raycaster = canvas.GetComponent<GraphicRaycaster>();
    }
}
