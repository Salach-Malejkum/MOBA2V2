using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDragHandler : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [SerializeField]
    private InventoryOnClick inventoryOnClick;
    private Transform originalSlot;
    public Transform OriginalSlot
    {
        get { return originalSlot; }
    }
    private readonly float clickTreshold = 0.5f;
    public float ClickTreshold
    {
        get { return clickTreshold; }
    }
    private float pointerDownTime;
    public float PointerDownTime
    {
        get { return pointerDownTime; }
    }
    [SerializeField]
    private Inventory inventory;

    public void OnPointerDown(PointerEventData eventData)
    {
        this.pointerDownTime = Time.time;
        this.originalSlot = this.transform.parent;
        if (this.InventorySlotNotEmptyAndLMBClicked(this.transform.parent.GetSiblingIndex(), eventData))
        {
            this.transform.SetParent(this.transform.parent.parent);
            GetComponent<CanvasGroup>().blocksRaycasts = false;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (this.InventorySlotNotEmptyAndLMBClicked(this.originalSlot.transform.GetSiblingIndex(), eventData))
        {
            this.transform.position = Input.mousePosition;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            this.transform.SetParent(this.OriginalSlot);
            this.transform.localPosition = Vector3.zero;
            this.GetComponent<CanvasGroup>().blocksRaycasts = true;
            this.inventoryOnClick.LeftClick(this.transform.parent.GetSiblingIndex());
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            this.inventoryOnClick.RightClick(this.transform.parent.GetSiblingIndex());
        }
    }

    private bool InventorySlotNotEmptyAndLMBClicked(int index, PointerEventData eventData)
    {
        return this.inventory.Equipment[index] != null && eventData.button == PointerEventData.InputButton.Left;
    }

}
