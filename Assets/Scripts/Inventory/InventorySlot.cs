using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    [SerializeField]
    private GameObject image;

    public void OnDrop(PointerEventData eventData)
    {
        ShopItemSo droppedItem = Inventory.instance.Equipment[eventData.pointerDrag.GetComponent<ItemDragHandler>().transform.parent.GetSiblingIndex()];
        if( eventData.pointerDrag.transform.parent.name == gameObject.name)
        {
            return;
        }
        if( Inventory.instance.Equipment[this.transform.GetSiblingIndex()] == null)
        {
            Inventory.instance.Equipment[this.transform.GetSiblingIndex()] = droppedItem;
            Inventory.instance.Equipment[eventData.pointerDrag.GetComponent<ItemDragHandler>().transform.parent.GetSiblingIndex()] = null;
            Inventory.instance.RefreshSlots();
            Inventory.instance.BlockSell();
        }
        else
        {
            ShopItemSo tempItem = Inventory.instance.Equipment[transform.GetSiblingIndex()];
            Inventory.instance.Equipment[this.transform.GetSiblingIndex()] = droppedItem;
            Inventory.instance.Equipment[eventData.pointerDrag.GetComponent<ItemDragHandler>().transform.parent.GetSiblingIndex()] = tempItem;
            Inventory.instance.RefreshSlots();
            Inventory.instance.BlockSell();
        }
    }

    public void RefreshSlot()
    {
        if( Inventory.instance.Equipment[transform.GetSiblingIndex()] != null)
        {
            this.image.GetComponent<Image>().sprite = Inventory.instance.Equipment[this.transform.GetSiblingIndex()].Image;
            this.image.SetActive(true);
        }
        else
        {
            this.image.SetActive(false);
        }
    }
}
