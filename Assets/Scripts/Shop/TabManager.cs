using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabManager : MonoBehaviour
{
    [SerializeField]
    private List<ShopItemSo> shopItemSo;
    [SerializeField]
    private List<GameObject> shopPanelsGo;
    [SerializeField]
    private List<ShopTemplate> shopPanels;
    [SerializeField]
    private List<Button> myBuyButtons;

    [SerializeField]
    private ShopManager shopManager;

    private int DelayAmount = 1;
    private float Timer;

    void Update()
    {
        Timer += Time.deltaTime;

        if (Timer >= DelayAmount)
        {
            Timer = 0f;
            CheckParchasable();

        }
    }

    void Start()
    {
        for (int i = 0; i < shopItemSo.Count; i++)
            shopPanelsGo[i].SetActive(true);
        LoadPanels();
        CheckParchasable();
    }

    public void CheckParchasable()
    {
        if (shopManager.Distance() <= shopManager.border && !InventoryManager.instance.IsEqFull())
        {
            for (int i = 0; i < shopItemSo.Count; i++)
            {
                if (shopManager.GoldValue >= shopItemSo[i].price)
                    myBuyButtons[i].interactable = true;
                else
                    myBuyButtons[i].interactable = false;
            }
        }
        else
        {
            for (int i = 0; i < shopItemSo.Count; i++)
            {
                myBuyButtons[i].interactable = false;
            }
        }
    }

    public void BuyItem(int btnNo)
    {
        if (shopManager.GoldValue >= shopItemSo[btnNo].price)
        {
            shopManager.Subtract(shopItemSo[btnNo].price);
            InventoryManager.instance.AddToEquipment(shopItemSo[btnNo]);
            CheckParchasable();
        }
    }

    public void LoadPanels()
    {
        for (int i = 0; i < shopItemSo.Count; i++)
        {
            shopPanels[i].titleTxt.text = shopItemSo[i].title;
            shopPanels[i].attackVal.text = "Attack: " + shopItemSo[i].attack.ToString();
            shopPanels[i].magicVal.text = "Magic: " + shopItemSo[i].magic.ToString();
            shopPanels[i].defenceVal.text = "Defence: " + shopItemSo[i].defence.ToString();
            shopPanels[i].priceVal.text = "Price: " + shopItemSo[i].price.ToString();
            shopPanels[i].ItemIm.sprite = shopItemSo[i].image;
        }
    }
}
