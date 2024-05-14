using Rewired;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemContextMenu : MonoBehaviour
{
    [SerializeField] private Canvas _tooltipCanvas;
    [SerializeField] private RectTransform rectTransform;
    [Tooltip("The distance from the mouse position the tooltip will appear at (relative to tooltip pivot)")]
    public Vector2 positionOffset;
    [Tooltip("The margins from the edge of the screen which the tooltip will stay within")]
    public Vector2 margins;

    private Vector2 _bounds;
    private Player player;
    public void SetPosition()
    {
        gameObject.SetActive(true);
        Vector2 screenPos = player.controllers.Mouse.screenPosition;
        // world position origin is wherever the pivot is
        Vector2 newPos = screenPos + positionOffset;
        float maxX = Screen.width - margins.x;
        float minX = margins.x;
        float maxY = Screen.height - margins.y;
        float minY = margins.y;
        float rightEdge = newPos.x + (1f - rectTransform.pivot.x) * rectTransform.rect.width * _tooltipCanvas.scaleFactor;
        if (rightEdge > maxX)
        {
            newPos.x -= rightEdge - maxX;
        }
        float leftEdge = newPos.x - rectTransform.pivot.x * rectTransform.rect.width * _tooltipCanvas.scaleFactor;
        if (leftEdge < minX)
        {
            newPos.x += minX - leftEdge;
        }

        // y is measured from top
        float topEdge = newPos.y + (1f - rectTransform.pivot.y) * rectTransform.rect.height * _tooltipCanvas.scaleFactor;
        if (topEdge > maxY)
        {
            newPos.y -= topEdge - maxY;
        }

        float bottomEdge = newPos.y - rectTransform.pivot.y * rectTransform.rect.height * _tooltipCanvas.scaleFactor;
        if (bottomEdge < minY)
        {
            newPos.y += minY - bottomEdge;
        }
        RectTransformUtility.ScreenPointToWorldPointInRectangle(_tooltipCanvas.transform as RectTransform, newPos, null, out var worldPoint);

        rectTransform.position = worldPoint;
    }
    private void Update()
    {
        if (ContextMenuManager.Instance.isShowing && (player.GetButtonUp("RightClick") || player.GetButtonUp("LeftClick")))
        {
            ContextMenuManager.Instance.HideContextMenu();
            return;
        }

    }
    private void Awake()
    {
        player = ReInput.players.GetPlayer(0);
    }
}
