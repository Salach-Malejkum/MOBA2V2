using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : NetworkBehaviour
{
    private int delayAmount = 1;
    [SerializeField]
    private TMP_Text goldValueText;
    private PlayerStats playerStats ;
    public PlayerStats PlayerStats
    {
        get { return playerStats; }
    }

    [SerializeField]
    private GameObject shopCanva;
    public GameObject ShopCanva
    {
        get { return shopCanva; }
    }

    private bool shopInRange;

    [SerializeField]
    private List<GameObject> detailsPanels = new List<GameObject>();
    public List<GameObject> DetailsPanels
    {
        get { return detailsPanels; }
    }

    [SerializeField]
    private Button sellBtn;
    public Button SellBtn
    {
        get { return sellBtn; }
    }

    [SerializeField]
    private Button openShop;

    private float timer;

    private ShopItemSo sellItem;
    private int sellItemIndex = -1;

    [Client]
    private void Awake()// client
    {
        //this.openShop = GameObject.Find("OpenShop");
        this.openShop.onClick.AddListener(this.ToggleShop);
        //this.goldValueText = GameObject.Find("GoldCounter").GetComponent<TextMeshProUGUI>();
        //this.shopCanva = GameObject.Find("ShopCanvas");
        //this.detailsPanels.Add(GameObject.Find("DetailsPanel (1)"));
        //this.detailsPanels.Add(GameObject.Find("DetailsPanel (2)"));
        //this.detailsPanels.Add(GameObject.Find("DetailsPanel (3)"));
        //this.sellBtn = GameObject.Find("SellBtn").GetComponent<Button>();
        this.SellBtn.onClick.AddListener(this.Sell);
        this.shopCanva.SetActive(false);
        this.DetailsPanels[0].SetActive(false);
        this.DetailsPanels[1].SetActive(false);
        this.DetailsPanels[2].SetActive(false);
        this.playerStats = this.transform.GetComponent<PlayerStats>();
    }

    [ServerCallback]
    private void FixedUpdate() // server callback
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

    [TargetRpc]
    public void SubtractPurchasedItemCostFromOwnedGold(float amount)// target
    {
        this.PlayerStats.SetPlayerGold(this.PlayerStats.GetPlayerGold() - amount);
        this.goldValueText.text = "G: " + this.PlayerStats.GetPlayerGold();
        this.openShop.GetComponentInChildren<TMP_Text>().text = this.PlayerStats.GetPlayerGold() + " g";
    }

    //[TargetRpc]
    public bool IsInBorder() //target
    {
        return this.shopInRange;
    }

    [Client]
    public void ToggleShop() // client
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

    [TargetRpc]
    public void PrepareToSell(ShopItemSo itemToSell, int itemToSellIndex) // target
    {
        this.sellItem = itemToSell;
        this.sellItemIndex = itemToSellIndex;
        this.sellBtn.interactable = true;
    }

    [TargetRpc]
    public void InstaSell(ShopItemSo itemToSell) // target
    {
        this.sellItemIndex = -1;
        this.sellItem = itemToSell;
        this.Sell();
    }

    [TargetRpc]
    public void Sell() // target
    {
        float amount = (float)Math.Round(this.sellItem.TotalPrice * 0.8f);
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

    [TargetRpc]
    public void Buy(ShopItemSo item) // target
    {
        float currPrice = CurrPrice(item);
        if (this.PlayerStats.GetPlayerGold() >= currPrice)
        {
            this.SubtractPurchasedItemCostFromOwnedGold(currPrice);
            Inventory.instance.AddToEquipment(item);
            this.PlayerStats.AddItemStatsToPlayer(item);
        }
    }

    //[TargetRpc]
    public float CurrPrice(ShopItemSo itemToCheck) // target
    {
        float currPrice = itemToCheck.TotalPrice;

        foreach (ShopItemSo component in itemToCheck.Components.GroupBy(item => item.Title, (key, group) => group.First())) // je¿eli bed¹ przepisy o g³êbokoœci wiêkszej ni¿ 1 to bedzie to trzeba przerobiæ
        {
            if (Inventory.instance.ItemInEq(component))
            {
                currPrice -= component.TotalPrice;
            }
        }
        return currPrice;
    }

    [Server]
    public void DesactivateAllInfoPanels()// server
    {
        foreach (GameObject panel in detailsPanels)
        {
            panel.SetActive(false);
        }
    }

    [Client]
    void OnTriggerEnter(Collider other)// client
    {
        if (other.gameObject.name == "ChampionSpawner" || other.gameObject.name == "ChampionSpawner (1)")
        {
            this.shopInRange = true;
        }
    }

    [Client]
    private void OnTriggerExit(Collider other)// client
    {
        if (other.gameObject.name == "ChampionSpawner" || other.gameObject.name == "ChampionSpawner (1)")
        {
            this.shopInRange = false;
        }
    }
}