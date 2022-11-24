using Mirror;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : NetworkBehaviour
{
    public static Inventory instance;

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

    [ClientCallback]
    private void Awake()//client
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
            return;
        }
        DontDestroyOnLoad(this);
    }

    [ServerCallback]
    private void Start()//server
    {
        RefreshSlots();
        this.shop = this.transform.GetComponent<ShopManager>();
        //this.eqDisplaySlots.Add(GameObject.Find("slot (0)").GetComponent<InventorySlot>());
        //this.eqDisplaySlots.Add(GameObject.Find("slot (1)").GetComponent<InventorySlot>());
        //this.eqDisplaySlots.Add(GameObject.Find("slot (2)").GetComponent<InventorySlot>());
        //this.eqDisplaySlots.Add(GameObject.Find("slot (3)").GetComponent<InventorySlot>());
        //this.eqDisplaySlots.Add(GameObject.Find("slot (4)").GetComponent<InventorySlot>());
    }

    [TargetRpc]
    public void RefreshSlots()//target
    {
        for (int i = 0; i < this.eqDisplaySlots.Count; i++)
        {
            this.eqDisplaySlots[i].RefreshSlot();
        }
    }

    //[TargetRpc]
    public bool IsEqFull()//target
    {
        for (int i = 0; i < this.equipment.Length; i++)
        {
            if (this.equipment[i] == null)
                return false;
        }
        return true;
    }

    [TargetRpc]
    public void AddToEquipment(ShopItemSo item)//target
    {

        foreach (ShopItemSo component in item.Components)
        {
            for (int i = 0; i < this.equipment.Length; i++)
            {
                if (this.equipment[i] != null)
                {
                    if (this.equipment[i].Title == component.Title)
                    {
                        RemoveItem(i);
                        break;
                    }
                }
            }
        }
        for (int i = 0; i < this.equipment.Length; i++)
        {
            if (this.equipment[i] == null)
            {
                this.equipment[i] = item;
                RefreshSlots();
                return;
            }
        }

    }

    [TargetRpc]
    public void PassItemToSellToShopManager(int itemIndex)//target
    {
        if (this.shop.ShopCanva.activeSelf && this.shop.IsInBorder())
        {
            this.shop.PrepareToSell(this.equipment[itemIndex], itemIndex);
        }
    }

    [TargetRpc]
    public void PassItemToInstaSellToShopManager(int itemIndex)//target
    {
        if (this.shop.IsInBorder())
        {
            this.shop.InstaSell(this.equipment[itemIndex]);
            RemoveItem(itemIndex);
        }
    }

    [TargetRpc]
    public void RemoveItem(int itemIndex)// target
    {
        this.equipment[itemIndex] = null;
        RefreshSlots();
    }

    [Client]
    public void BlockSell()//client
    {
        this.shop.SellBtn.interactable = false;
    }

    //[TargetRpc]
    public bool ItemInEq(ShopItemSo itemToCheck)//target
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

    //[TargetRpc]
    public bool OneComponentsBought(ShopItemSo item)//target
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
