using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public ShopItemSo[] shopItemSo;
    public GameObject[] shopPanelsGo;
    public ShopTemplate[] shopPanels;
    public Button[] myBuyButtons;

    public int GoldValue = 0;
    public int DelayAmount = 1; // Second count
    public TMP_Text GoldValueText;

    protected float Timer;

    void Update()
    {
        Timer += Time.deltaTime;

        if (Timer >= DelayAmount)
        {
            Timer = 0f;
            GoldValue++; // For every DelayAmount or "second" it will add one to the GoldValue
            GoldValueText.text = "G: " + GoldValue;
            CheckParchasable();
        }
    }

    public void Subtract(int amount)
    {
        GoldValue -= amount;
        GoldValueText.text = "G: " + GoldValue;
    }


    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < shopItemSo.Length; i++)
            shopPanelsGo[i].SetActive(true);
        LoadPanels();
        CheckParchasable();
    }

    public void CheckParchasable()
    {
        for (int i = 0; i < shopItemSo.Length; i++)
        {
            if (GoldValue >= shopItemSo[i].price)
                myBuyButtons[i].interactable = true;
            else
                myBuyButtons[i].interactable = false;
        }
    }

    public void BuyItem(int btnNo)
    {
        if (GoldValue >= shopItemSo[btnNo].price)
        {
            Subtract(shopItemSo[btnNo].price);
            CheckParchasable();
        }
    }

    public void LoadPanels()
    {
        for (int i = 0; i < shopItemSo.Length; i++ )
        {
            shopPanels[i].titleTxt.text = shopItemSo[i].title;
            shopPanels[i].attackVal.text = "Attack: " + shopItemSo[i].attack.ToString();
            shopPanels[i].magicVal.text = "Magic: " + shopItemSo[i].magic.ToString();
            shopPanels[i].defenceVal.text = "Defence: " + shopItemSo[i].defence.ToString();
            shopPanels[i].priceVal.text = "Price: " + shopItemSo[i].price.ToString();
        }
    }
}