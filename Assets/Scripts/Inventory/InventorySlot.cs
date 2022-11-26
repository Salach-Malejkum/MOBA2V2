using Mirror;
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
        ShopItemSo droppedItem = inventory.Equipment[eventData.pointerDrag.GetComponent<ItemDragHandler>().transform.parent.GetSiblingIndex()];
        if( eventData.pointerDrag.transform.parent.name == gameObject.name)
        {
            return;
        }
        if (inventory.Equipment[this.transform.GetSiblingIndex()] == null)
        {
            inventory.Equipment[this.transform.GetSiblingIndex()] = droppedItem;
            inventory.Equipment[eventData.pointerDrag.GetComponent<ItemDragHandler>().transform.parent.GetSiblingIndex()] = null;
            inventory.CmdRefreshSlots();
            inventory.BlockSell();
        }
        else
        {
            ShopItemSo tempItem = inventory.Equipment[transform.GetSiblingIndex()];
            inventory.Equipment[this.transform.GetSiblingIndex()] = droppedItem;
            inventory.Equipment[eventData.pointerDrag.GetComponent<ItemDragHandler>().transform.parent.GetSiblingIndex()] = tempItem;
            inventory.CmdRefreshSlots();
            inventory.BlockSell();
        }
    }
    
    public void RefreshSlot()
    {
        if (inventory.Equipment[transform.GetSiblingIndex()] != null)
        {
            this.image.GetComponent<Image>().sprite = inventory.Equipment[this.transform.GetSiblingIndex()].Image;
            this.image.SetActive(true);
        }
        else
        {
            this.image.SetActive(false);
        }
    }
}
