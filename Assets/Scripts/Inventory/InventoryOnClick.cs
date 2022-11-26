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

    [Client]
    public void LeftClick(int slotIndex)
    {
        CmdLeftClick(slotIndex);
    }

    [Command]
    private void CmdLeftClick(int slotIndex)
    {
        RpcLeftClick(slotIndex);
    }

    [TargetRpc]
    private void RpcLeftClick(int slotIndex)
    {
        if (Time.time - this.inventorySlot[slotIndex].GetComponent<ItemDragHandler>().PointerDownTime < this.inventorySlot[slotIndex].GetComponent<ItemDragHandler>().ClickTreshold)
        {
            this.inventory.CmdPassItemToSellToShopManager(this.inventorySlot[slotIndex].transform.parent.GetSiblingIndex());
        }
    }

    [Client]
    public void RightClick(int slotindex)
    {
        CmdRightClick(slotindex);
    }

    [Command]
    private void CmdRightClick(int slotindex)
    {
        RpcRightClick(slotindex);
    }

    [TargetRpc]
    private void RpcRightClick(int slotindex)
    {
        if (Time.time - this.inventorySlot[slotindex].GetComponent<ItemDragHandler>().PointerDownTime < this.inventorySlot[slotindex].GetComponent<ItemDragHandler>().ClickTreshold)
        {
            this.inventory.CmdInstaSell(this.inventorySlot[slotindex].transform.parent.GetSiblingIndex());
        }
    }
}
