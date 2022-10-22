using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;

    public InventorySlot[] EqDisplaySlots;
    [HideInInspector]
    public ShopItemSo[] Equipment = new ShopItemSo[5];

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
}
