using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [SerializeField]
    private int DelayAmount = 1;

    [SerializeField]
    private TMP_Text GoldValueText;

    public int GoldValue = 0;
    public GameObject shopCanva;
    public GameObject marketPlace;
    public Button SellBtn;

    public float border = 100f;
    private float Timer;

    private ShopItemSo SellItem;
    private int SellItemIndex = -1;

    void Update()
    {
        Timer += Time.deltaTime;

        if (Timer >= DelayAmount)
        {
            Timer = 0f;
            GoldValue++;
            GoldValueText.text = "G: " + GoldValue;
        }

        if (Distance() >= border)
        {
            SellBtn.interactable = false;
        }
    }

    public void Subtract(int amount)
    {
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
            SellBtn.interactable = false;
        }
        else
        {
            shopCanva.SetActive(true);
        }
    }

    public void PrepareToSell(ShopItemSo itemToSell, int itemToSellIndex)
    {
        SellItem = itemToSell;
        SellItemIndex = itemToSellIndex;
        SellBtn.interactable = true;
    }

    public void InstaSell(ShopItemSo itemToSell)
    {
        SellItemIndex = -1;
        SellItem = itemToSell;
        Sell();
    }

    public void Sell()
    {
        float amount = SellItem.price * 0.8f;
        GoldValue += (int) amount;
        GoldValueText.text = "G: " + GoldValue;
        SellBtn.interactable = false;
        if(SellItemIndex != -1)
        {
            InventoryManager.instance.RemoveItem(SellItemIndex);
            SellItemIndex = -1;
        }
    }
}