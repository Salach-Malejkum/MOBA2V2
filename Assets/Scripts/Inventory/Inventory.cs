using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    [SerializeField]
    private InventorySlot[] eqDisplaySlots;
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
        if(instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(this);
        }
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        RefreshSlots();
        this.shop = this.transform.GetComponent<ShopManager>();
    }

    public void RefreshSlots()
    {
        for (int i = 0; i < this.eqDisplaySlots.Length; i++)
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

    public void SellWBtnInBetween(int itemIndex)
    {
        if (this.shop.ShopCanva.activeSelf && this.shop.IsInBorder())
        {
            this.shop.PrepareToSell(this.equipment[itemIndex], itemIndex);
        }
    }

    public void InstaSellInBetween(int itemIndex)
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
}
