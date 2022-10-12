using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShopManager : MonoBehaviour
{
    public int GoldValue = 0;
    public int DelayAmount = 1; // Second count
    public TMP_Text GoldValueText;
    public GameObject[] Equipment;

    public GameObject shopCanva;
    public GameObject marketPlace;

    public float border = 100f;
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

    public float Distance()
    {
        return Vector3.Distance(transform.position, marketPlace.transform.position);
    }

    public void ToggleShop(InputAction.CallbackContext context)
    {
        Debug.Log("P");
        if (shopCanva.activeSelf)
        {
            shopCanva.SetActive(false);
        }
        else
        {
            shopCanva.SetActive(true);
        }
    }
}