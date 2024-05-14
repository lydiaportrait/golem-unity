using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Rewired;
using System;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(LayoutElement))]
public class Tooltip : MonoBehaviour
{
    public const int MaxWidth = 360;
    public bool isAdvancedView = false;
    private Player player;
    [SerializeField] private TextMeshProUGUI _tooltipTmp;
    [SerializeField] private Canvas _tooltipCanvas;
    [SerializeField] private RectTransform rectTransform;
    private RectTransform _rectTransform;
    private LayoutElement _layoutElement;

    [Tooltip("The distance from the mouse position the tooltip will appear at (relative to tooltip pivot)")]
    public Vector2 positionOffset;
    [Tooltip("The margins from the edge of the screen which the tooltip will stay within")]
    public Vector2 margins;

    private Vector2 _bounds;

    public void SetPosition()
    {
        Vector2 screenPos = ReInput.controllers.Mouse.screenPosition;
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

    public void Show(string message)
    {
        if (_tooltipTmp.text == message && gameObject.activeSelf)
            return;

        gameObject.SetActive(true);

        _tooltipTmp.text = message;

        _layoutElement.preferredWidth = -1;
        if (_tooltipTmp.preferredWidth > MaxWidth)
        {
            _layoutElement.preferredWidth = MaxWidth;
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(_rectTransform);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }
    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _layoutElement = GetComponent<LayoutElement>();

        _bounds = new Vector2(Screen.width, Screen.height);
    } 
}
