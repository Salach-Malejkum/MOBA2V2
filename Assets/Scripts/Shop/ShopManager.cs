using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [SerializeField]
    private int delayAmount = 1;

    [SerializeField]
    private TMP_Text goldValueText;

    private int goldValue = 0;
    public int GoldValue
    {
        get { return goldValue; }
        set { goldValue = value; }
    }

    [SerializeField]
    private GameObject shopCanva;
    public GameObject ShopCanva
    {
        get { return shopCanva; }
    }

    [SerializeField]
    private GameObject marketPlace;
    public GameObject MarketPlace
    {
        get { return marketPlace; }
    }

    [SerializeField]
    private Button sellBtn;
    public Button SellBtn
    {
        get { return sellBtn; }
    }

    private readonly float border = 10f;
    public float Border
    {
        get { return border; }
    }

    private float timer;

    private ShopItemSo sellItem;
    private int sellItemIndex = -1;

    private void Update()
    {
        this.timer += Time.deltaTime;

        if (this.timer >= this.delayAmount)
        {
            this.timer = 0f;
            this.goldValue++;
            this.goldValueText.text = "G: " + this.goldValue;
        }
        
        if (!IsInBorder())
        {
            this.sellBtn.interactable = false;
        }
    }

    public void Subtract(int amount)
    {
        this.goldValue -= amount;
        this.goldValueText.text = "G: " + this.goldValue;
    }

    public bool IsInBorder()
    {
        
        return Vector3.Distance(this.transform.position, this.MarketPlace.transform.position) <= this.Border;
    }

    public void ToggleShop(InputAction.CallbackContext _)
    {
        if (this.shopCanva.activeSelf)
        {
            this.shopCanva.SetActive(false);
            this.sellBtn.interactable = false;
        }
        else
        {
            this.shopCanva.SetActive(true);
        }
    }

    public void PrepareToSell(ShopItemSo itemToSell, int itemToSellIndex)
    {
        this.sellItem = itemToSell;
        this.sellItemIndex = itemToSellIndex;
        this.sellBtn.interactable = true;
    }

    public void InstaSell(ShopItemSo itemToSell)
    {
        this.sellItemIndex = -1;
        this.sellItem = itemToSell;
        this.Sell();
    }

    private void Sell()
    {
        float amount = this.sellItem.Price * 0.8f;
        this.goldValue += (int) amount;
        this.goldValueText.text = "G: " + this.goldValue;
        this.sellBtn.interactable = false;
        if(this.sellItemIndex != -1)
        {
            InventoryManager.instance.RemoveItem(this.sellItemIndex);
            this.sellItemIndex = -1;
        }
    }
}