using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    [SerializeField]
    private GameObject image;

    public void OnDrop(PointerEventData eventData)
    {
        ShopItemSo droppedItem = InventoryManager.instance.Equipment[eventData.pointerDrag.GetComponent<ItemDragHandler>().transform.parent.GetSiblingIndex()];
        if( eventData.pointerDrag.transform.parent.name == gameObject.name)
        {
            return;
        }
        if( InventoryManager.instance.Equipment[transform.GetSiblingIndex()] == null)
        {
            InventoryManager.instance.Equipment[transform.GetSiblingIndex()] = droppedItem;
            InventoryManager.instance.Equipment[eventData.pointerDrag.GetComponent<ItemDragHandler>().transform.parent.GetSiblingIndex()] = null;
            InventoryManager.instance.RefreshSlots();
            InventoryManager.instance.BlockSell();
        }
        else
        {
            ShopItemSo tempItem = InventoryManager.instance.Equipment[transform.GetSiblingIndex()];
            InventoryManager.instance.Equipment[transform.GetSiblingIndex()] = droppedItem;
            InventoryManager.instance.Equipment[eventData.pointerDrag.GetComponent<ItemDragHandler>().transform.parent.GetSiblingIndex()] = tempItem;
            InventoryManager.instance.RefreshSlots();
            InventoryManager.instance.BlockSell();
        }
    }

    public void RefreshSlot()
    {
        if( InventoryManager.instance.Equipment[transform.GetSiblingIndex()] != null)
        {
            image.GetComponent<Image>().sprite = InventoryManager.instance.Equipment[transform.GetSiblingIndex()].image;
            image.SetActive(true);
        }
        else
        {
            image.SetActive(false);
        }
    }
}
