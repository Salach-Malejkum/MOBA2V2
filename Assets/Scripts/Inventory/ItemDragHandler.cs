using Mirror;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDragHandler : NetworkBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    private Transform originalSlot;
    private readonly float clickTreshold = 0.5f;
    private float pointerDownTime;

    //[Client]
    public void OnPointerDown(PointerEventData eventData)//client
    {
        this.pointerDownTime = Time.time;
        this.originalSlot = this.transform.parent;
        if (InventorySlotNotEmpty(this.transform.parent.GetSiblingIndex(), eventData) && eventData.button == PointerEventData.InputButton.Left)
        {
            
            this.transform.SetParent(this.transform.parent.parent);
            GetComponent<CanvasGroup>().blocksRaycasts = false;
        }
    }

    //[Client]
    public void OnDrag(PointerEventData eventData)//client
    {
        if (InventorySlotNotEmpty(this.originalSlot.transform.GetSiblingIndex(), eventData) && eventData.button == PointerEventData.InputButton.Left)
        {
            this.transform.position = Input.mousePosition;
        }
    }

    //[TargetRpc]
    public void OnPointerUp(PointerEventData eventData)//target
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

    [TargetRpc]
    private void LeftClick()//target
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

    [TargetRpc]
    private void RightClick()//target
    {
        if (Time.time - this.pointerDownTime < this.clickTreshold)
        {
            Inventory.instance.PassItemToInstaSellToShopManager(this.transform.parent.GetSiblingIndex());
        }
    }

    [Client]
    private bool InventorySlotNotEmpty(int index, PointerEventData eventData)//client
    {
        return Inventory.instance.Equipment[index] != null && eventData.button == PointerEventData.InputButton.Left;
    }

}
