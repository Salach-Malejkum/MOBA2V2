using Mirror;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : NetworkBehaviour
{
    [SerializeField]
    private GameObject inventoryCanva;
    [SerializeField]
    private List<InventorySlot> eqDisplaySlots = new List<InventorySlot>();
    [HideInInspector]
    private ShopItemSo[] equipment = new ShopItemSo[5];
    public ShopItemSo[] Equipment
    {
        get { return equipment; }
        set { equipment = value; }
    }

    private ShopManager shop;

    public override void OnStartLocalPlayer()
    {
        this.inventoryCanva.SetActive(true);
        CmdRefreshSlots();
        this.shop = this.transform.GetComponent<ShopManager>();
    }

    [ClientCallback]
    public void CmdRefreshSlots()
    {
        for (int i = 0; i < this.eqDisplaySlots.Count; i++)
        {
            this.eqDisplaySlots[i].RefreshSlot();
        }
    }


    [Client]
    public bool IsEqFull()
    {
        for (int i = 0; i < this.equipment.Length; i++)
        {
            if (this.equipment[i] == null)
                return false;
        }
        return true;
    }

    [Command]
    public void CmdPassItemToSellToShopManager(int itemIndex)
    {
        RpcPassItemToSellToShopManager(itemIndex);
    }

    [TargetRpc]
    public void RpcPassItemToSellToShopManager(int itemIndex)
    {
        if (this.shop.ShopCanva.activeSelf && this.shop.ShopInRange)
        {
            this.shop.PrepareToSell(this.equipment[itemIndex], itemIndex, true);
        }
    }

    [Command]
    public void CmdInstaSell(int itemIndex)
    {
        RpcInstaSell(itemIndex);
    }

    [TargetRpc]
    public void RpcInstaSell(int itemIndex)
    {
        if (this.shop.ShopCanva.activeSelf && this.shop.ShopInRange)
        {
            this.shop.PrepareToSell(this.equipment[itemIndex], itemIndex, false);
            this.shop.CmdSell();
        }
    }

    [ClientCallback]
    public void CmdRemoveItem(int itemIndex)
    {
        this.equipment[itemIndex] = null;
    }

    [Client]
    public void BlockSell()
    {
        this.shop.SellBtn.interactable = false;
    }

    [Client]
    public bool ItemInEq(ShopItemSo itemToCheck)
    {
        foreach (ShopItemSo item in Equipment)
        {
            if (item != null)
            {
                if (itemToCheck.Title == item.Title)
                {
                    return true;
                }
            }
        }
        return false;
    }

    [Client]
    public bool OneComponentsBought(ShopItemSo item)
    {
        foreach (ShopItemSo component in item.Components)
        {
            if (this.ItemInEq(component))
            {
                return true;
            }
        }
        return false;
    }
}
