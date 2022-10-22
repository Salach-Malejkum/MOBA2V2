using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public int GoldValue = 0;
    public int DelayAmount = 1; // Second count
    public TMP_Text GoldValueText;

    public GameObject shopCanva;
    public GameObject marketPlace;
    public Button SellBtn;

    public float border = 100f;
    public float Timer;

    private ShopItemSo SellItem;

    void Update()
    {
        Timer += Time.deltaTime;

        if (Timer >= DelayAmount)
        {
            Timer = 0f;
            GoldValue++; // For every DelayAmount or "second" it will add one to the GoldValue
            GoldValueText.text = "G: " + GoldValue;
            //Debug.Log("ShopManager Gold: "+GoldValue);
        }
    }

    public void Subtract(int amount)
    {
        //Debug.Log("odejmuje");
        GoldValue -= amount;
        GoldValueText.text = "G: " + GoldValue;
    }

    public float Distance()
    {
        return Vector3.Distance(transform.position, marketPlace.transform.position);
    }

    public void ToggleShop(InputAction.CallbackContext context)
    {
        if (shopCanva.activeSelf)
        {
            shopCanva.SetActive(false);
        }
        else
        {
            shopCanva.SetActive(true);
        }
    }

    public void PrepareToSell(ShopItemSo itemToSell)
    {
        SellItem = itemToSell;
        SellBtn.interactable = true;
    }

    public void InstaSell(ShopItemSo itemToSell)
    {
        SellItem = itemToSell;
        Sell();
    }

    public void Sell()
    {
        float amount = SellItem.price * 0.8f;
        GoldValue += (int) amount;
        GoldValueText.text = "G: " + GoldValue;
        SellBtn.interactable = false;
    }
}