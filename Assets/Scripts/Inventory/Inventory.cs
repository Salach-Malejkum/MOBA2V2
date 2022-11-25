using Mirror;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : NetworkBehaviour
{
    //public static Inventory instance;

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

    public override void OnStartClient()
    {
        RpcRefreshSlots();
        this.shop = this.transform.GetComponent<ShopManager>();
    }

    [Command]
    public void CmdRefreshSlots()
    {
        RpcRefreshSlots();
    }

    [TargetRpc]
    public void RpcRefreshSlots()
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

    ////przenieœ do tabManagera
    //[Command]
    //public void CmdAddToEquipment(ShopItemSo item)//prawdopodobnie bedzie trzeba przenieœæ gdzieœ indziej bo przekazuje obiekt typu ShopItemSo----------------------------------------------
    //{
    //    RpcAddToEquipment(item);
    //}

    ////przenieœ do tabManagera
    //[TargetRpc]
    //public void RpcAddToEquipment(ShopItemSo item)//prawdopodobnie bedzie trzeba przenieœæ gdzieœ indziej bo przekazuje obiekt typu ShopItemSo----------------------------------------------
    //{

    //    foreach (ShopItemSo component in item.Components)
    //    {
    //        for (int i = 0; i < this.equipment.Length; i++)
    //        {
    //            if (this.equipment[i] != null)
    //            {
    //                if (this.equipment[i].Title == component.Title)
    //                {
    //                    RpcRemoveItem(i);
    //                    break;
    //                }
    //            }
    //        }
    //    }
    //    for (int i = 0; i < this.equipment.Length; i++)
    //    {
    //        if (this.equipment[i] == null)
    //        {
    //            this.equipment[i] = item;
    //            RpcRefreshSlots();
    //            return;
    //        }
    //    }
    //}

    [Command]
    public void CmdPassItemToSellToShopManager(int itemIndex)
    {
        RpcPassItemToSellToShopManager(itemIndex);
    }

    [TargetRpc]
    public void RpcPassItemToSellToShopManager(int itemIndex)
    {
        if (this.shop.ShopCanva.activeSelf && this.shop.IsInBorder())
        {
            this.shop.PrepareToSell(this.equipment[itemIndex], itemIndex);
        }
    }

    [Command]
    public void CmdPassItemToInstaSellToShopManager(int itemIndex)
    {
        RpcPassItemToInstaSellToShopManager(itemIndex);
    }

    [TargetRpc]
    public void RpcPassItemToInstaSellToShopManager(int itemIndex)
    {
        if (this.shop.IsInBorder())
        {
            this.CmdInstaSell(itemIndex);
            RpcRemoveItem(itemIndex);
        }
    }

    [Command]
    public void CmdRemoveItem(int itemIndex)
    {
        RpcRemoveItem(itemIndex);
    }

    [TargetRpc]
    public void RpcRemoveItem(int itemIndex)
    {
        this.equipment[itemIndex] = null;
        RpcRefreshSlots();
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

    //Z ShopManagera
    [Command]
    public void CmdInstaSell(int itemIndex)
    {
        RpcInstaSell(itemIndex);
    }

    [TargetRpc]
    public void RpcInstaSell(int itemIndex)
    {
        this.shop.SellItemIndex = -1;
        this.shop.SellItem = this.equipment[itemIndex];
        this.shop.CmdSell();
    }
}
