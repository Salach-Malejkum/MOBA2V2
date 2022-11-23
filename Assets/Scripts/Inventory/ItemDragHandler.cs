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
        this.originalSlot = this.transform.parent;
        if (InventorySlotNotEmpty(this.transform.parent.GetSiblingIndex(), eventData) && eventData.button == PointerEventData.InputButton.Left)
        {
            
            this.transform.SetParent(this.transform.parent.parent);
            GetComponent<CanvasGroup>().blocksRaycasts = false;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (InventorySlotNotEmpty(this.originalSlot.transform.GetSiblingIndex(), eventData) && eventData.button == PointerEventData.InputButton.Left)
        {
            this.transform.position = Input.mousePosition;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
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
            if (Inventory.instance.Equipment[this.transform.parent.GetSiblingIndex()])
            {
                if (Inventory.instance.Equipment[this.transform.parent.GetSiblingIndex()].GetType() == typeof(ItemTypeOne))
                {
                    Inventory.instance.Equipment[this.transform.parent.GetSiblingIndex()].OnItemUse();
                }
            }

            Inventory.instance.PassItemToSellToShopManager(this.transform.parent.GetSiblingIndex());
        }
    }

    private void RightClick()
    {
        if (Time.time - this.pointerDownTime < this.clickTreshold)
        {
            Inventory.instance.PassItemToInstaSellToShopManager(this.transform.parent.GetSiblingIndex());
        }
    }

    private bool InventorySlotNotEmpty(int index, PointerEventData eventData)
    {
        return Inventory.instance.Equipment[index] != null && eventData.button == PointerEventData.InputButton.Left;
    }

}
