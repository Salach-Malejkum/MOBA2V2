using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;

    public InventorySlot[] EqDisplaySlots;
    [HideInInspector]
    public ShopItemSo[] Equipment = new ShopItemSo[5];

    private ShopManager Shop;

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
        Shop = transform.GetComponent<ShopManager>();
    }

    public void RefreshSlots()
    {
        for (int i = 0; i < EqDisplaySlots.Length; i++)
        {
            EqDisplaySlots[i].RefreshSlot();
        }
    }

    public bool IsEqFull()
    {
        for (int i = 0; i < Equipment.Length; i++)
        {
            if (Equipment[i] == null)
                return false;
        }
        return true;
    }

    public void AddToEquipment(ShopItemSo item)
    {
        for (int i = 0; i < Equipment.Length; i++)
        {
            if (Equipment[i] == null)
            {
                Equipment[i] = item;
                RefreshSlots();
                return;
            }
        }
    }

    public void PrepareToSellMid(int itemIndex)
    {
        if (Shop.shopCanva.activeSelf && Shop.Distance() <= Shop.border)
        {
            Shop.PrepareToSell(Equipment[itemIndex], itemIndex);
        }
    }

    public void SellMid(int itemIndex)
    {
        if (Shop.Distance() <= Shop.border)
        {
            Shop.InstaSell(Equipment[itemIndex]);
            RemoveItem(itemIndex);
        }
    }

    public void RemoveItem(int itemIndex)
    {
        Equipment[itemIndex] = null;
        RefreshSlots();
    }

    public void BlockSell()
    {
        Shop.SellBtn.interactable = false;
    }
}
