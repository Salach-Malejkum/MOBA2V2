using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDragHandler : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    private Transform originalSlot;
    private readonly float clickTreshold = 0.5f;
    private float pointerDownTime;

    public void OnPointerDown(PointerEventData eventData)
    {
        this.pointerDownTime = Time.time;
        if (InventorySlotNotEmpty())
        {
            if(eventData.button == PointerEventData.InputButton.Left)
            {
                this.originalSlot = this.transform.parent;
                this.transform.SetParent(this.transform.parent.parent);
                GetComponent<CanvasGroup>().blocksRaycasts = false;
            }
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (InventoryManager.instance.Equipment[this.originalSlot.transform.GetSiblingIndex()] != null && eventData.button == PointerEventData.InputButton.Left)
        {
            this.transform.position = Input.mousePosition;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if( eventData.button == PointerEventData.InputButton.Left)
        {
            LeftClick();
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            RightClick();
        }

    }

    private void LeftClick()
    {
        this.transform.SetParent(this.originalSlot);
        this.transform.localPosition = Vector3.zero;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        if (Time.time - this.pointerDownTime < this.clickTreshold)
        {
            if (InventorySlotNotEmpty())
            {
                if (InventoryManager.instance.Equipment[this.transform.parent.GetSiblingIndex()].GetType() == typeof(ItemTypeOne))
                {
                    InventoryManager.instance.Equipment[this.transform.parent.GetSiblingIndex()].OnItemUse();
                }
            }

            InventoryManager.instance.PrepareToSellMid(this.transform.parent.GetSiblingIndex());
        }
    }

    private void RightClick()
    {
        if (Time.time - this.pointerDownTime < this.clickTreshold)
        {
            InventoryManager.instance.SellMid(this.transform.parent.GetSiblingIndex());
        }
    }

    private bool InventorySlotNotEmpty()
    {
        return InventoryManager.instance.Equipment[this.transform.parent.GetSiblingIndex()] != null;
    }

}
