using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public int GoldValue = 0;
    public int DelayAmount = 1; // Second count
    public TMP_Text GoldValueText;

    public float Timer;

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
}