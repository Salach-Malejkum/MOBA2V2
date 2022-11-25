using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryOnClick : NetworkBehaviour
{
    [SerializeField]
    private List<GameObject> inventorySlot;
    [SerializeField]
    private Inventory inventory;

    [Command]
    public void CmdLeftClick(int slotIndex)
    {
        RpcLeftClick(slotIndex);
    }

    [TargetRpc]
    private void RpcLeftClick(int slotIndex)
    {
        Debug.Log("RpcLeftClick start with slot index" + slotIndex);
        if (Time.time - this.inventorySlot[slotIndex].GetComponent<ItemDragHandler>().PointerDownTime < this.inventorySlot[slotIndex].GetComponent<ItemDragHandler>().ClickTreshold)
        {
            if (inventory.Equipment[this.inventorySlot[slotIndex].transform.parent.GetSiblingIndex()])
            {
                if (inventory.Equipment[this.inventorySlot[slotIndex].transform.parent.GetSiblingIndex()].GetType() == typeof(ItemTypeOne))
                {
                    inventory.Equipment[this.inventorySlot[slotIndex].transform.parent.GetSiblingIndex()].OnItemUse();
                }
            }

            inventory.CmdPassItemToSellToShopManager(this.inventorySlot[slotIndex].transform.parent.GetSiblingIndex());
        }
        Debug.Log("RpcLeftClick end");
    }

    [Command]
    public void CmdRightClick(int slotIndex)
    {
        RpcRightClick(slotIndex);
    }

    [TargetRpc]
    private void RpcRightClick(int slotIndex)
    {
        if (Time.time - this.inventorySlot[slotIndex].GetComponent<ItemDragHandler>().PointerDownTime < this.inventorySlot[slotIndex].GetComponent<ItemDragHandler>().ClickTreshold)
        {
            inventory.CmdPassItemToInstaSellToShopManager(this.inventorySlot[slotIndex].transform.parent.GetSiblingIndex());
        }
    }
}
