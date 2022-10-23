using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDragHandler : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    private Transform originalSlot;
    private float clickTreshold = 0.5f;
    private float init;

    public void OnPointerDown(PointerEventData eventData)
    {
        init = Time.time;
        if (InventoryManager.instance.Equipment[transform.parent.GetSiblingIndex()] != null)
        {
            if(eventData.button == PointerEventData.InputButton.Left)
            {
                originalSlot = transform.parent;
                transform.SetParent(transform.parent.parent);
                GetComponent<CanvasGroup>().blocksRaycasts = false;
            }
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (InventoryManager.instance.Equipment[originalSlot.transform.GetSiblingIndex()] != null && eventData.button == PointerEventData.InputButton.Left)
        {
            transform.position = Input.mousePosition;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if( eventData.button == PointerEventData.InputButton.Left)
        {
            transform.SetParent(originalSlot);
            transform.localPosition = Vector3.zero;
            GetComponent<CanvasGroup>().blocksRaycasts = true;
            if( Time.time - init < clickTreshold)
            {
                LeftClick();
            }
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (Time.time - init < clickTreshold)
            {
                RightClick();
            }
        }

    }

    public void LeftClick()
    {
        if (InventoryManager.instance.Equipment[transform.parent.GetSiblingIndex()] != null)
        {
            if (InventoryManager.instance.Equipment[transform.parent.GetSiblingIndex()].GetType() == typeof(ItemTypeOne))
            {
                InventoryManager.instance.Equipment[transform.parent.GetSiblingIndex()].OnUse();
            }
        }

        InventoryManager.instance.PrepareToSellMid(transform.parent.GetSiblingIndex());
    }

    public void RightClick()
    {
        InventoryManager.instance.SellMid(transform.parent.GetSiblingIndex());
    }

}
