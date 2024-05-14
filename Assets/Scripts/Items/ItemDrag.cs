using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Rewired;
public class ItemDrag : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [ReadOnly]
    public bool canDrag = true;
    private Player player;
    private Canvas _canvas;
    private Transform _previousParent;
    private int _previousIndex;
    [SerializeField] private GameObject _ghostRepPrefab;
    private GameObject _ghostRep;
    private static List<CanvasGroup> canvasGroups = new List<CanvasGroup>();
    [SerializeField] private ItemInfo thisInfo;
    private bool splitting = false;
    private void Awake()
    {
        _canvas = GetComponentInParent<Canvas>();
        canvasGroups.Add(gameObject.GetComponent<CanvasGroup>());
        player = ReInput.players.GetPlayer(0);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!canDrag || !player.GetButton("LeftClick") || ConsumableCraftingManager.Instance.WaitingForClick)
            return;
        transform.position = Input.mousePosition;
    }
    public void OnDragStart()
    {
        foreach (CanvasGroup cg in canvasGroups)
            cg.blocksRaycasts = false;
        _previousIndex = transform.GetSiblingIndex();
        _previousParent = transform.parent;
        transform.SetParent(_canvas.transform);
        if (!_previousParent.GetComponent<InventorySlot>())
            return;
        if (_previousParent.GetComponent<InventorySlot>().isCharacterPanel)
        {
            _ghostRep = Instantiate(_ghostRepPrefab, _previousParent);
            _ghostRep.GetComponent<Image>().sprite = gameObject.GetComponent<Image>().sprite;
            _ghostRep.transform.SetSiblingIndex(_previousIndex);
        }
    }
    public void OnDragEnd(PointerEventData eventData)
    {
        foreach (CanvasGroup cg in canvasGroups)
            cg.blocksRaycasts = true;
        if (eventData.pointerCurrentRaycast.isValid)
        {
            if (eventData.pointerCurrentRaycast.gameObject.GetComponent<InventorySlot>() != null)
            {
                DroppedOn(eventData.pointerCurrentRaycast.gameObject.GetComponent<InventorySlot>());
            }
            else
                InvalidDrop();
        }
        else
            InvalidDrop();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!canDrag || ConsumableCraftingManager.Instance.WaitingForClick)
            return;
        if (player.GetButton("LeftClick") && player.GetButton("Shift"))
        {
            int split = Mathf.FloorToInt((float)thisInfo.currentStack / 2);
            splitting = Split(split);
        }
        else if(player.GetButton("LeftClick") && player.GetButton("Control"))
        {
            int split = (thisInfo.currentStack - 1);
            splitting = Split(split);
        }
        else
            splitting = false;
        if (player.GetButton("LeftClick"))
        {
            OnDragStart();
            return;
        }
    }
    bool Split(int split1)
    {
        if (split1 == 0)
            return false;
        thisInfo.AddStack(split1 * -1);
        ItemData thisData = ItemDatabase.Instance.GetItemByID(thisInfo.id);
        GameObject go = Instantiate(thisData.item, transform.parent);
        ItemInfo info = go.GetComponent<ItemInfo>();
        info.Create(thisData, thisInfo.iLevel, thisInfo.maxAffixCount, thisInfo.itemName);
        info.AddStack(split1 - info.currentStack);
        transform.parent.GetComponent<InventorySlot>().currentlyHolding = go;
        return true;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (ConsumableCraftingManager.Instance.WaitingForClick)
            return;
        if (canDrag && player.GetButtonUp("LeftClick"))
        {
            if (ConsumableCraftingManager.Instance.WaitingForClick)
                return;
            OnDragEnd(eventData);
            return;
        }
    }

    public void DroppedOn(InventorySlot InventorySlot)
    {
        GameObject go = InventorySlot.currentlyHolding;
        if (go == gameObject || (!InventorySlot.slotType.Contains(GetComponent<ItemInfo>().itemType) && !InventorySlot.slotType.Contains(GlobalDefinitions.ItemType.None)) || !InventorySlot.isDroppable)
            InvalidDrop();
        else if (go == null || InventorySlot.containsMultiple)
        {
            MoveObject(InventorySlot);
        }
        else if (TryAddStackTo(go.GetComponent<ItemInfo>()))
        {
            if(!splitting)
                _previousParent.GetComponent<InventorySlot>().currentlyHolding = null;
            Destroy(gameObject);
        }
        else
        {
            ItemDrag otherDrag = InventorySlot.currentlyHolding.GetComponent<ItemDrag>();
            otherDrag._previousParent = InventorySlot.transform;
            _previousParent.GetComponent<InventorySlot>().currentlyHolding = null;
            otherDrag.DroppedOn(_previousParent.GetComponent<InventorySlot>());
            
            MoveObject(InventorySlot);
            _previousParent.GetComponent<InventorySlot>().UpdateHolding();
        }
    }
    public bool TryAddStackTo(ItemInfo target)
    {
        if (target.id == thisInfo.id && target.isStackable && (target.currentStack + thisInfo.currentStack) <= target.maxStackSize)
        {
            Debug.Log("adding 2 stack" + target.gameObject.name);
            target.AddStack(thisInfo.currentStack);
            return true;
        }
        return false;  
    }
    private void MoveObject(InventorySlot inventorySlot)
    {
        transform.SetParent(inventorySlot.transform);
        InventorySlot prev = _previousParent.GetComponent<InventorySlot>();
        prev.UpdateHolding();
        inventorySlot.UpdateHolding();
        transform.localPosition = new Vector2(0, 0);
        if (inventorySlot.isCharacterPanel)
        {
            transform.SetSiblingIndex(_ghostRep.transform.GetSiblingIndex());
            Destroy(_ghostRep);
        }
        prev.OnRemove(gameObject);
        inventorySlot.OnDrop(gameObject);

    }
    private void InvalidDrop()
    {
        InventorySlot prev = _previousParent.GetComponent<InventorySlot>();
        if (prev.isCharacterPanel)
            Destroy(_ghostRep);
        if(prev.currentlyHolding == gameObject || prev.currentlyHolding == null)
        {
            transform.SetParent(_previousParent);
            transform.SetSiblingIndex(_previousIndex);
            transform.localPosition = new Vector2(0, 0);
            prev.UpdateHolding();
        }
        else
        {
            TryAddStackTo(prev.currentlyHolding.GetComponent<ItemInfo>());
            Destroy(gameObject);
        }

    }
    private void OnDestroy()
    {
        canvasGroups.Remove(GetComponent<CanvasGroup>());
    }

}
