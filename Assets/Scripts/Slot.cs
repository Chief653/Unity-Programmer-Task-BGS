using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class Slot : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Item currentItem;
    public ItemType itemType;
    private GameObject currentItemObject;
    private Transform originalParent;
    private Vector3 originalPosition;
    private Image itemImage;
    private Outline outline;
    private Tween delayedCallTween;

    void Start() {
        outline = GetComponent<Outline>();
    }

    public void SetItem(Item item, GameObject itemObject)
    {
        currentItem = item;
        currentItemObject = itemObject;
        currentItemObject.transform.SetParent(transform);
        currentItemObject.transform.localPosition = Vector3.zero;
        itemImage = currentItemObject.GetComponent<Image>();

        if( currentItem == InventoryManager.instance.equippedTool ||
            currentItem == InventoryManager.instance.equippedConsumable ||
            currentItem == InventoryManager.instance.equippedArmor )
            EquipItem();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (currentItem != null)
        {
            EquipItem();
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (currentItem != null)
        {
            originalParent = currentItemObject.transform.parent;
            originalPosition = currentItemObject.transform.localPosition;
            currentItemObject.transform.SetParent(transform.root);
            itemImage.raycastTarget = false;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (currentItem != null)
        {
            currentItemObject.transform.position = Input.mousePosition;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (currentItem == null)
            return;

        GameObject newSlotObject = eventData.pointerCurrentRaycast.gameObject;

        if (newSlotObject != null && newSlotObject.CompareTag("Slot"))
        {
            Slot newSlot = newSlotObject.GetComponent<Slot>();

            if (newSlot != null && newSlot.currentItem == null)
            {
                newSlot.SetItem(currentItem, currentItemObject);
                ClearSlot();
            }
            else
            {
                ReturnToOriginalSlot();
            }
            }
        else
        {
            ReturnToOriginalSlot();
        }

        itemImage.raycastTarget = true;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(currentItemObject == null)
            return;

        GameObject newSlotObject = eventData.pointerCurrentRaycast.gameObject;

        if (newSlotObject != null && (newSlotObject.CompareTag("Slot") || newSlotObject.CompareTag("Item"))) {
            delayedCallTween = DOVirtual.DelayedCall(.5f, () =>
            {
                RectTransform rectTransform = InventoryManager.instance.detailsObj.GetComponent<RectTransform>();

                Vector2 mousePosition;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    rectTransform.parent as RectTransform,
                    Input.mousePosition,
                    null,
                    out mousePosition
                );

                rectTransform.anchoredPosition = mousePosition;

                InventoryManager.instance.detailsObj.GetComponent<SetupDetails>().SetupDet(currentItem);
                InventoryManager.instance.detailsObj.SetActive(true);
            });
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GameObject newSlotObject = eventData.pointerCurrentRaycast.gameObject;

        if (newSlotObject == null)
            return;

        if (newSlotObject.CompareTag("Slot") || newSlotObject.CompareTag("Item"))
            return;

        if (delayedCallTween != null && delayedCallTween.IsActive())
        {
            delayedCallTween.Kill();
        }

        InventoryManager.instance.detailsObj.SetActive(false);
    }

    private void ReturnToOriginalSlot()
    {
        currentItemObject.transform.SetParent(originalParent);
        currentItemObject.transform.localPosition = originalPosition;
    }

    private void ClearSlot()
    {
        currentItem = null;
        currentItemObject = null;
        outline.effectColor = Color.white;
    }

    private void EquipItem()
    {
        List<Slot> allSlots = new List<Slot>();

        switch (itemType)
        {
            case ItemType.Tool:
                allSlots = InventoryManager.instance.toolSlots;
                InventoryManager.instance.equippedTool = currentItem;
                break;
            case ItemType.Consumable:
                allSlots = InventoryManager.instance.consumableSlots;
                InventoryManager.instance.equippedConsumable = currentItem;
                break;
            case ItemType.Armor:
                allSlots = InventoryManager.instance.armorSlots;
                InventoryManager.instance.equippedArmor = currentItem;
                break;
        }

        foreach (Slot slot in allSlots)
        {
            Outline outlines = slot?.GetComponent<Outline>();
            if (outlines != null)
            {
                outlines.effectColor = Color.white;
            }
        }

        outline.effectColor = Color.yellow;
    }
}
