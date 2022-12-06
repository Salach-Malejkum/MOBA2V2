using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ShopManager : NetworkBehaviour
{
    private int delayAmount = 1;
    [SerializeField]
    private TMP_Text goldValueText;
    public TMP_Text GoldValueText
    {
        get { return goldValueText; }
    }

    [SerializeField]
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
    public bool ShopInRange
    {
        get { return shopInRange; }
    }

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
    public Button OpenShop
    {
        get { return openShop; }
    }

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

    [SerializeField]
    private Inventory inventory;

    public override void OnStartLocalPlayer()
    {
        this.openShop.onClick.AddListener(this.ToggleShop);
    }

    [ClientCallback]
    private void Update()
    {
        this.timer += Time.deltaTime;

        if (this.timer >= this.delayAmount)
        {
            this.timer = 0f;
            this.GoldValueText.text = "G: " + this.PlayerStats.PlayerGold;
            this.OpenShop.GetComponentInChildren<TMP_Text>().text = this.PlayerStats.PlayerGold + " g";
        }

        if (!this.ShopInRange)
        {
            this.SellBtn.interactable = false;
        }
    }

    [Command]
    public void CmdSubtractPurchasedItemCostFromOwnedGold(float amount)
    {
        this.RpcSubtractPurchasedItemCostFromOwnedGold(amount);
    }

    [TargetRpc]
    private void RpcSubtractPurchasedItemCostFromOwnedGold(float amount)
    {
        this.PlayerStats.PlayerGold = this.PlayerStats.PlayerGold - amount;
        this.goldValueText.text = "G: " + this.PlayerStats.PlayerGold;
        this.openShop.GetComponentInChildren<TMP_Text>().text = this.PlayerStats.PlayerGold + " g";
    }

    [ClientCallback]
    public void OpenShopCanva(InputAction.CallbackContext context)
    {
        if (!isOwned)
        {
            return;
        }
        if (context.control.displayName == "P")
        {
            this.ToggleShop();
        }
    }

    [Client]
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

    [Client]
    public void PrepareToSell(ShopItemSo itemToSell, int itemToSellIndex, bool isNotInsta)
    {
        this.sellItem = itemToSell;
        this.sellItemIndex = itemToSellIndex;
        this.sellBtn.interactable = isNotInsta;
    }

    [Command]
    public void CmdSell()
    {
        this.RpcSell();
    }

    [TargetRpc]
    private void RpcSell()
    {
        float amount = (float)Math.Round(this.sellItem.TotalPrice * 0.8f);
        this.PlayerStats.PlayerGold = this.PlayerStats.PlayerGold + amount;
        this.goldValueText.text = "G: " + this.PlayerStats.PlayerGold;
        this.openShop.GetComponentInChildren<TMP_Text>().text = this.PlayerStats.PlayerGold + " g";
        this.sellBtn.interactable = false;
        this.inventory.CmdRemoveItem(this.sellItemIndex);
        this.PlayerStats.SubtractItemStatsFromPlayer(this.sellItem);
        this.inventory.CmdRefreshSlots();
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
        if (other.gameObject.name == "BlueChampionSpawner" || other.gameObject.name == "RedChampionSpawner")
        {
            this.shopInRange = true;
        }
    }

    [Client]
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "BlueChampionSpawner" || other.gameObject.name == "RedChampionSpawner")
        {
            this.shopInRange = false;
        }
    }
}
