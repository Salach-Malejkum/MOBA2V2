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
    public ShopItemSo SellItem
    {
        get { return sellItem; }
        set { sellItem = value; }
    }
    private int sellItemIndex = -1;
    public int SellItemIndex
    {
        get { return sellItemIndex; }
        set { sellItemIndex = value; }
    }

    private Inventory inventory;

    [ClientCallback]
    private void Awake()
    {
        this.openShop.onClick.AddListener(this.ToggleShop);
        //this.openShop = GameObject.Find("OpenShop");
        //this.goldValueText = GameObject.Find("GoldCounter").GetComponent<TextMeshProUGUI>();
        //this.shopCanva = GameObject.Find("ShopCanvas");
        //this.detailsPanels.Add(GameObject.Find("DetailsPanel (1)"));
        //this.detailsPanels.Add(GameObject.Find("DetailsPanel (2)"));
        //this.detailsPanels.Add(GameObject.Find("DetailsPanel (3)"));
        //this.sellBtn = GameObject.Find("SellBtn").GetComponent<Button>();
        
        
        this.SellBtn.onClick.AddListener(this.CmdSell);
        this.shopCanva.SetActive(false);
        this.DetailsPanels[0].SetActive(false);
        this.DetailsPanels[1].SetActive(false);
        this.DetailsPanels[2].SetActive(false);
        this.playerStats = this.transform.GetComponent<PlayerStats>();
        this.inventory = this.transform.GetComponent<Inventory>();
    }

    [ServerCallback]
    private void FixedUpdate()
    {
        this.timer += Time.deltaTime;

        if (this.timer >= this.delayAmount)
        {
            this.timer = 0f;
            this.goldValueText.text = "G: " + this.PlayerStats.PlayerGold;
            this.openShop.GetComponentInChildren<TMP_Text>().text = this.PlayerStats.PlayerGold + " g";
        }
        
        if (!IsInBorder())
        {
            this.sellBtn.interactable = false;
        }
    }

    [Command]
    public void CmdSubtractPurchasedItemCostFromOwnedGold(float amount)
    {
        RpcSubtractPurchasedItemCostFromOwnedGold(amount);
    }

    [TargetRpc]
    public void RpcSubtractPurchasedItemCostFromOwnedGold(float amount)
    {
        this.PlayerStats.PlayerGold = this.PlayerStats.PlayerGold - amount;
        this.goldValueText.text = "G: " + this.PlayerStats.PlayerGold;
        this.openShop.GetComponentInChildren<TMP_Text>().text = this.PlayerStats.PlayerGold + " g";
    }

    [Client]
    public bool IsInBorder()
    {
        return this.shopInRange;
    }

    [Client]
    public void ToggleShop()
    {
        if (isLocalPlayer)
        {
            Debug.Log("toggle");
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
        else
        {
            Debug.Log("toggle 2");
        }
    }

    [Client]
    public void PrepareToSell(ShopItemSo itemToSell, int itemToSellIndex)
    {
        this.sellItem = itemToSell;
        this.sellItemIndex = itemToSellIndex;
        this.sellBtn.interactable = true;
    }


    ////przenieœ do inventory?
    //[Command]
    //public void CmdInstaSell(ShopItemSo itemToSell)//---------------------------------------------
    //{
    //    RpcInstaSell(itemToSell);
    //}
    ////przenieœ do inventory?
    //[TargetRpc]
    //public void RpcInstaSell(ShopItemSo itemToSell)//---------------------------------------------
    //{
    //    this.sellItemIndex = -1;
    //    this.sellItem = itemToSell;
    //    this.CmdSell();
    //}

    [Command]
    public void CmdSell()
    {
        RpcSell();
    }

    [ClientRpc]
    public void RpcSell()
    {
        float amount = (float)Math.Round(this.sellItem.TotalPrice * 0.8f);
        this.PlayerStats.PlayerGold = this.PlayerStats.PlayerGold + amount;
        this.goldValueText.text = "G: " + this.PlayerStats.PlayerGold;
        this.openShop.GetComponentInChildren<TMP_Text>().text = this.PlayerStats.PlayerGold + " g";
        this.sellBtn.interactable = false;
        if(this.sellItemIndex != -1)
        {
            inventory.CmdRemoveItem(this.sellItemIndex);
            this.sellItemIndex = -1;
        }
        this.PlayerStats.SubtractItemStatsFromPlayer(this.sellItem);
    }


    ////przenieœæ do tabManagera?
    //[Command]
    //public void CmdBuy(ShopItemSo item) //prawdopodobnie bedzie trzeba przenieœæ gdzieœ indziej bo przekazuje obiekt typu ShopItemSo----------------------------------------------
    //{
    //    RpcBuy(item);
    //}

    ////przenieœæ do tabManagera?
    //[ClientRpc]
    //public void RpcBuy(ShopItemSo item) // prawdopodobnie bedzie trzeba przenieœæ gdzieœ indziej bo przekazuje obiekt typu ShopItemSo----------------------------------------------
    //{
    //    float currPrice = CurrPrice(item);
    //    if (this.PlayerStats.PlayerGold >= currPrice)
    //    {
    //        this.CmdSubtractPurchasedItemCostFromOwnedGold(currPrice);
    //        inventory.CmdAddToEquipment(item);
    //        this.PlayerStats.AddItemStatsToPlayer(item);
    //    }
    //}

    [Client]
    public float CurrPrice(ShopItemSo itemToCheck)
    {
        float currPrice = itemToCheck.TotalPrice;

        foreach (ShopItemSo component in itemToCheck.Components.GroupBy(item => item.Title, (key, group) => group.First())) // je¿eli bed¹ przepisy o g³êbokoœci wiêkszej ni¿ 1 to bedzie to trzeba przerobiæ
        {
            if (inventory.ItemInEq(component))
            {
                currPrice -= component.TotalPrice;
            }
        }
        return currPrice;
    }

    [Client]
    public void DesactivateAllInfoPanels()
    {
        foreach (GameObject panel in detailsPanels)
        {
            panel.SetActive(false);
        }
    }

    [Client]
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "ChampionSpawner" || other.gameObject.name == "ChampionSpawner (1)")
        {
            this.shopInRange = true;
        }
    }

    [Client]
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "ChampionSpawner" || other.gameObject.name == "ChampionSpawner (1)")
        {
            this.shopInRange = false;
        }
    }
}
