using Mirror;
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
        this.CmdLeftClick(slotIndex);
    }

    [Command]
    private void CmdLeftClick(int slotIndex)
    {
        this.RpcLeftClick(slotIndex);
    }

    [TargetRpc]
    private void RpcLeftClick(int slotIndex)
    {
        this.inventory.CmdPassItemToSellToShopManager(this.inventorySlot[slotIndex].transform.parent.GetSiblingIndex());
    }

    [Client]
    public void RightClick(int slotindex)
    {
        this.CmdRightClick(slotindex);
    }

    [Command]
    private void CmdRightClick(int slotindex)
    {
        this.RpcRightClick(slotindex);
    }

    [TargetRpc]
    private void RpcRightClick(int slotindex)
    {
        this.inventory.CmdInstaSell(this.inventorySlot[slotindex].transform.parent.GetSiblingIndex());
    }
}
