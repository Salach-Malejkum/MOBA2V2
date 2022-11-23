using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    private List<InventorySlot> eqDisplaySlots = new List<InventorySlot>();
    [HideInInspector]
    private ShopItemSo[] equipment = new ShopItemSo[5];
    public ShopItemSo[] Equipment
    {
        get { return equipment; }
        set { equipment = value; }
    }

    private ShopManager shop;

    private void Awake()
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

    private void Start()
    {
        RefreshSlots();
        this.shop = this.transform.GetComponent<ShopManager>();
        this.eqDisplaySlots.Add(GameObject.Find("slot (0)").GetComponent<InventorySlot>());
        this.eqDisplaySlots.Add(GameObject.Find("slot (1)").GetComponent<InventorySlot>());
        this.eqDisplaySlots.Add(GameObject.Find("slot (2)").GetComponent<InventorySlot>());
        this.eqDisplaySlots.Add(GameObject.Find("slot (3)").GetComponent<InventorySlot>());
        this.eqDisplaySlots.Add(GameObject.Find("slot (4)").GetComponent<InventorySlot>());
    }

    public void RefreshSlots()
    {
        for (int i = 0; i < this.eqDisplaySlots.Count; i++)
        {
            this.eqDisplaySlots[i].RefreshSlot();
        }
    }

    public bool IsEqFull()
    {
        for (int i = 0; i < this.equipment.Length; i++)
        {
            if (this.equipment[i] == null)
                return false;
        }
        return true;
    }

    public void AddToEquipment(ShopItemSo item)
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

    public void PassItemToSellToShopManager(int itemIndex)
    {
        if (this.shop.ShopCanva.activeSelf && this.shop.IsInBorder())
        {
            this.shop.PrepareToSell(this.equipment[itemIndex], itemIndex);
        }
    }

    public void PassItemToInstaSellToShopManager(int itemIndex)
    {
        if (this.shop.IsInBorder())
        {
            this.shop.InstaSell(this.equipment[itemIndex]);
            RemoveItem(itemIndex);
        }
    }

    public void RemoveItem(int itemIndex)
    {
        this.equipment[itemIndex] = null;
        RefreshSlots();
    }

    public void BlockSell()
    {
        this.shop.SellBtn.interactable = false;
    }

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
