using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    [SerializeField]
    private GameObject image;
    [SerializeField]
    private Inventory inventory;

    public void OnDrop(PointerEventData eventData)
    {
        ShopItemSo droppedItem = this.inventory.Equipment[eventData.pointerDrag.GetComponent<ItemDragHandler>().transform.parent.GetSiblingIndex()];
        if( eventData.pointerDrag.transform.parent.name == gameObject.name)
        {
            return;
        }

        if (this.IsSlotEmpty())
        {
            this.OnDropHelper(droppedItem, eventData);
        }
        else
        {
            ShopItemSo tempItem = this.inventory.Equipment[transform.GetSiblingIndex()];
            this.OnDropHelper(droppedItem, eventData, tempItem);
        }
    }
    
    public void RefreshSlot()
    {
        if (!this.IsSlotEmpty())
        {
            this.image.GetComponent<Image>().sprite = this.inventory.Equipment[this.transform.GetSiblingIndex()].Image;
            this.image.SetActive(true);
        }
        else
        {
            this.image.SetActive(false);
        }
    }

    private void OnDropHelper(ShopItemSo droppedItem, PointerEventData eventData, ShopItemSo item = null)
    {
        this.inventory.Equipment[this.transform.GetSiblingIndex()] = droppedItem;
        this.inventory.Equipment[eventData.pointerDrag.GetComponent<ItemDragHandler>().transform.parent.GetSiblingIndex()] = item;
        this.inventory.CmdRefreshSlots();
        this.inventory.BlockSell();
    }

    private bool IsSlotEmpty()
    {
        return this.inventory.Equipment[this.transform.GetSiblingIndex()] == null;
    }
}
