using Mirror;
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
        Debug.Log("OnPointerDown start with index" + this.transform.parent.GetSiblingIndex());
        this.pointerDownTime = Time.time;
        this.originalSlot = this.transform.parent;
        if (InventorySlotNotEmpty(this.transform.parent.GetSiblingIndex(), eventData) && eventData.button == PointerEventData.InputButton.Left)
        {
            
            this.transform.SetParent(this.transform.parent.parent);
            GetComponent<CanvasGroup>().blocksRaycasts = false;
        }
        Debug.Log("OnPointerDown end");
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
        Debug.Log("OnPointerUp start");
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            this.transform.SetParent(this.OriginalSlot);
            this.transform.localPosition = Vector3.zero;
            this.GetComponent<CanvasGroup>().blocksRaycasts = true;
            inventoryOnClick.CmdLeftClick(this.transform.parent.GetSiblingIndex());
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            inventoryOnClick.CmdRightClick(this.transform.parent.GetSiblingIndex());
        }
        Debug.Log("OnPointerUp end");
    }

    private bool InventorySlotNotEmpty(int index, PointerEventData eventData)
    {
        return inventory.Equipment[index] != null && eventData.button == PointerEventData.InputButton.Left;
    }

}
