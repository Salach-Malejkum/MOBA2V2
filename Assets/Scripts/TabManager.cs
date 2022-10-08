using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TabManager : MonoBehaviour
{
    public ShopItemSo[] shopItemSo;
    public GameObject[] shopPanelsGo;
    public ShopTemplate[] shopPanels;
    public Button[] myBuyButtons;

    public ShopManager shopManager;

    public int DelayAmount = 1; // Second count
    public float Timer;

    void Update()
    {
        Timer += Time.deltaTime;

        if (Timer >= DelayAmount)
        {
            //Debug.Log("TabManager Gold: " + shopManager.GoldValue);
            CheckParchasable();
        }
    }

    void Start()
    {
        //for (int i = 0; i < myBuyButtons.Length; i++)
        //{
        //    myBuyButtons[i].onClick.AddListener(delegate { BuyItem(i); });
        //}

        for (int i = 0; i < shopItemSo.Length; i++)
            shopPanelsGo[i].SetActive(true);
        LoadPanels();
        CheckParchasable();
    }

    public void CheckParchasable()
    {
        
        for (int i = 0; i < shopItemSo.Length; i++)
        {
            if (shopManager.GoldValue >= shopItemSo[i].price)
                myBuyButtons[i].interactable = true;
            else
                myBuyButtons[i].interactable = false;
        }
    }

    public void BuyItem(int btnNo)
    {
        //Debug.Log(btnNo);
        if (shopManager.GoldValue >= shopItemSo[btnNo].price)
        {
            //Debug.Log("buying " + shopItemSo[btnNo].price);
            shopManager.Subtract(shopItemSo[btnNo].price);
            CheckParchasable();
        }
    }

    public void LoadPanels()
    {
        for (int i = 0; i < shopItemSo.Length; i++)
        {
            shopPanels[i].titleTxt.text = shopItemSo[i].title;
            shopPanels[i].attackVal.text = "Attack: " + shopItemSo[i].attack.ToString();
            shopPanels[i].magicVal.text = "Magic: " + shopItemSo[i].magic.ToString();
            shopPanels[i].defenceVal.text = "Defence: " + shopItemSo[i].defence.ToString();
            shopPanels[i].priceVal.text = "Price: " + shopItemSo[i].price.ToString();
        }
    }
}
