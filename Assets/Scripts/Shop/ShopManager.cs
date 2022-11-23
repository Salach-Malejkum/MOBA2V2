using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    private int delayAmount = 1;
    private TMP_Text goldValueText;
    private PlayerStats playerStats ;
    public PlayerStats PlayerStats
    {
        get { return playerStats; }
    }

    private GameObject shopCanva;
    public GameObject ShopCanva
    {
        get { return shopCanva; }
    }

    private bool shopInRange;

    private List<GameObject> detailsPanels = new List<GameObject>();
    public List<GameObject> DetailsPanels
    {
        get { return detailsPanels; }
    }

    private Button sellBtn;
    public Button SellBtn
    {
        get { return sellBtn; }
    }

    private GameObject openShop;

    private float timer;

    private ShopItemSo sellItem;
    private int sellItemIndex = -1;

    private void Awake()
    {
        this.openShop = GameObject.Find("OpenShop");
        this.openShop.GetComponent<Button>().onClick.AddListener(this.ToggleShop);
        this.goldValueText = GameObject.Find("GoldCounter").GetComponent<TextMeshProUGUI>();
        this.shopCanva = GameObject.Find("ShopCanvas");
        this.detailsPanels.Add(GameObject.Find("DetailsPanel (1)"));
        this.detailsPanels.Add(GameObject.Find("DetailsPanel (2)"));
        this.detailsPanels.Add(GameObject.Find("DetailsPanel (3)"));
        this.sellBtn = GameObject.Find("SellBtn").GetComponent<Button>();
        this.SellBtn.onClick.AddListener(this.Sell);
        this.shopCanva.SetActive(false);
        this.DetailsPanels[0].SetActive(false);
        this.DetailsPanels[1].SetActive(false);
        this.DetailsPanels[2].SetActive(false);
        this.playerStats = this.transform.GetComponent<PlayerStats>();
    }

    private void Update()
    {
        this.timer += Time.deltaTime;

        if (this.timer >= this.delayAmount)
        {
            this.timer = 0f;
            this.goldValueText.text = "G: " + this.PlayerStats.GetPlayerGold();
            this.openShop.GetComponentInChildren<TMP_Text>().text = this.PlayerStats.GetPlayerGold() + " g";
        }
        
        if (!IsInBorder())
        {
            this.sellBtn.interactable = false;
        }
    }

    public void SubtractPurchasedItemCostFromOwnedGold(int amount)
    {
        this.PlayerStats.SetPlayerGold(this.PlayerStats.GetPlayerGold() - amount);
        this.goldValueText.text = "G: " + this.PlayerStats.GetPlayerGold();
        this.openShop.GetComponentInChildren<TMP_Text>().text = this.PlayerStats.GetPlayerGold() + " g";
    }

    public bool IsInBorder()
    {
        return this.shopInRange;
    }

    public void ToggleShop()
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

    public void Sell()
    {
        float amount = this.sellItem.TotalPrice * 0.8f;
        this.PlayerStats.SetPlayerGold(this.PlayerStats.GetPlayerGold() + amount);
        this.goldValueText.text = "G: " + this.PlayerStats.GetPlayerGold();
        this.openShop.GetComponentInChildren<TMP_Text>().text = this.PlayerStats.GetPlayerGold() + " g";
        this.sellBtn.interactable = false;
        if(this.sellItemIndex != -1)
        {
            Inventory.instance.RemoveItem(this.sellItemIndex);
            this.sellItemIndex = -1;
        }
        this.PlayerStats.SubtractItemStatsFromPlayer(this.sellItem);
    }

    public void Buy(ShopItemSo item)
    {
        int currPrice = CurrPrice(item);
        if (this.PlayerStats.GetPlayerGold() >= currPrice)
        {
            this.SubtractPurchasedItemCostFromOwnedGold(currPrice);
            Inventory.instance.AddToEquipment(item);
            this.PlayerStats.AddItemStatsToPlayer(item);
        }
    }

    public int CurrPrice(ShopItemSo itemToCheck)
    {
        int currPrice = itemToCheck.TotalPrice;

        foreach (ShopItemSo component in itemToCheck.Components.GroupBy(item => item.Title, (key, group) => group.First())) // je¿eli bed¹ przepisy o g³êbokoœci wiêkszej ni¿ 1 to bedzie to trzeba przerobiæ
        {
            if (Inventory.instance.ItemInEq(component))
            {
                currPrice -= component.TotalPrice;
            }
        }
        return currPrice;
    }

    public void DesactivateAllInfoPanels()
    {
        foreach (GameObject panel in detailsPanels)
        {
            panel.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "ChampionSpawner" || other.gameObject.name == "ChampionSpawner (1)")
        {
            this.shopInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "ChampionSpawner" || other.gameObject.name == "ChampionSpawner (1)")
        {
            this.shopInRange = false;
        }
    }
}