using Mirror;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TabManager : NetworkBehaviour
{
    [SerializeField]
    private List<ShopItemSo> shopItemSo;
    [SerializeField]
    private List<GameObject> shopPanelsGo;
    [SerializeField]
    private List<ShopTemplate> shopPanels;
    [SerializeField]
    private List<Button> myBuyButtons;
    [SerializeField]
    private Inventory inventory;

    [SerializeField]
    private ShopManager shopManager;

    private GameObject activeInfoPanel = null;

    private readonly int delayAmount = 1;
    private float timer;

    public override void OnStartAuthority()//OnStartClient nie wywo³uje siê 
    {
        //Debug.Log("OnStartClient Start");
        for (int i = 0; i < this.shopItemSo.Count; i++)
        {
            this.shopPanelsGo[i].SetActive(true);
        }
        this.LoadPanels();
        this.CheckPurchasable();
        //Debug.Log("OnStartClient End");
    }

    [ServerCallback]
    void FixedUpdate()// przenieœæ gdzieœ indziej---------------------------------------------------------------------------------------------------------------------------------------------
    {
        this.timer += Time.deltaTime;

        if (this.timer >= this.delayAmount)
        {
            this.timer = 0f;
            this.CheckPurchasable();
        }
    }

    [Client]
    private void CheckPurchasable()
    {
        //Debug.Log("CheckPurchasable Start");
        if (this.shopManager.IsInBorder())
        {
            if (!inventory.IsEqFull())
            {
                for (int i = 0; i < this.shopItemSo.Count; i++)
                {
                    if (this.shopManager.PlayerStats.PlayerGold >= this.shopManager.CurrPrice(this.shopItemSo[i]))
                    {
                        this.myBuyButtons[i].interactable = true;
                    }
                    else
                    {
                        this.myBuyButtons[i].interactable = false;
                    }
                }
            }
            else
            {
                for (int i = 0; i < this.shopItemSo.Count; i++)
                {
                    if (this.shopItemSo[i].Components.Count > 0 && this.shopManager.PlayerStats.PlayerGold >= this.shopManager.CurrPrice(this.shopItemSo[i]))
                    {
                        if (inventory.OneComponentsBought(this.shopItemSo[i]))
                        {
                            this.myBuyButtons[i].interactable = true;
                        }
                        else
                        {
                            this.myBuyButtons[i].interactable = false;
                        }
                        
                    }
                    else
                    {
                        this.myBuyButtons[i].interactable = false;
                    }
                }
            }
        }
        else
        {
            for (int i = 0; i < this.shopItemSo.Count; i++)
            {
                this.myBuyButtons[i].interactable = false;
            }
        }
        this.RefreshInfoPanel();
        //Debug.Log("CheckPurchasable End");
    }

    [Client]
    public void GetItemNo(int itemNo)
    {
        this.CmdBuy(itemNo);
        this.CheckPurchasable();
    }

    [Client]
    public void InfoItem(int itemNo)
    {
        this.shopManager.DesactivateAllInfoPanels();

        //Obecnie ob³s³uguje przepisy tylko o g³êbokoœci 1
        int panelIndex = this.shopItemSo[itemNo].Components.Count;
        this.activeInfoPanel = this.shopManager.DetailsPanels[panelIndex];
        this.activeInfoPanel.SetActive(true);
        InfoPanelsTemplate infoPanelTemplate = this.activeInfoPanel.GetComponent<InfoPanelsTemplate>();
        infoPanelTemplate.BuyBtn.onClick.RemoveAllListeners();
        infoPanelTemplate.TitleTxt.text = this.shopItemSo[itemNo].Title;
        infoPanelTemplate.AttackVal.text = "Attack: " + this.shopItemSo[itemNo].Attack.ToString();
        infoPanelTemplate.AbilityPowerVal.text = "Ability Power: " + this.shopItemSo[itemNo].AbilityPower.ToString();
        infoPanelTemplate.ArmorVal.text = "Armor: " + this.shopItemSo[itemNo].Armor.ToString();
        infoPanelTemplate.MagicResistVal.text = "Magic Resist: " + this.shopItemSo[itemNo].MagicResist.ToString();
        infoPanelTemplate.CooldownReductionVal.text = "Cooldown Reduction: " + this.shopItemSo[itemNo].CooldownReduction.ToString();
        infoPanelTemplate.HealthVal.text = "Health: " + this.shopItemSo[itemNo].Health.ToString();
        infoPanelTemplate.PriceVal.text = this.shopManager.CurrPrice(this.shopItemSo[itemNo]).ToString();
        infoPanelTemplate.ItemIm.sprite = this.shopItemSo[itemNo].Image;

        for (int i = 0; i < this.shopItemSo[itemNo].Components.Count; i++)
        {

            infoPanelTemplate.ItemImComp[i].sprite = this.shopItemSo[itemNo].Components[i].Image;
            infoPanelTemplate.PriceValComp[i].text = this.shopItemSo[itemNo].Components[i].TotalPrice.ToString();
            if (inventory.ItemInEq(this.shopItemSo[itemNo].Components[i]))
            {
                Color newCompColor = infoPanelTemplate.ItemImComp[i].color;
                newCompColor.a = 0.5f;
                infoPanelTemplate.ItemImComp[i].color = newCompColor;
                infoPanelTemplate.PriceValComp[i].fontStyle = TMPro.FontStyles.Strikethrough;
            }
            else
            {
                Color newCompColor = infoPanelTemplate.ItemImComp[i].color;
                newCompColor.a = 1f;
                infoPanelTemplate.ItemImComp[i].color = newCompColor;
                infoPanelTemplate.PriceValComp[i].fontStyle = TMPro.FontStyles.Normal;
            }
        }
        infoPanelTemplate.BuyBtn.onClick.AddListener(delegate { GetItemNo(itemNo); });
        RefreshInfoPanel();
    }

    [Client]
    public void RefreshInfoPanel()
    {
        //Debug.Log("RefreshInfoPanel Start");
        if (this.activeInfoPanel != null)
        {
            InfoPanelsTemplate infoPanelTemplate = this.activeInfoPanel.GetComponent<InfoPanelsTemplate>();
            ShopItemSo item = this.shopItemSo.First(item => item.Title == this.activeInfoPanel.GetComponent<InfoPanelsTemplate>().TitleTxt.text);
            infoPanelTemplate.PriceVal.text = this.shopManager.CurrPrice(item).ToString();
            for (int i = 0; i < item.Components.Count; i++)
            {
                if (inventory.ItemInEq(item.Components[i]))
                {
                    infoPanelTemplate.PriceValComp[i].fontStyle = TMPro.FontStyles.Strikethrough;
                    Color newCompColor = infoPanelTemplate.ItemImComp[i].color;
                    newCompColor.a = 0.5f;
                    infoPanelTemplate.ItemImComp[i].color = newCompColor;
                }
                else
                {
                    infoPanelTemplate.PriceValComp[i].fontStyle = TMPro.FontStyles.Normal;
                    Color newCompColor = infoPanelTemplate.ItemImComp[i].color;
                    newCompColor.a = 1f;
                    infoPanelTemplate.ItemImComp[i].color = newCompColor;
                }
            }

            if (this.shopManager.IsInBorder())
            {
                if (!inventory.IsEqFull())
                {
                    if (this.shopManager.PlayerStats.PlayerGold >= this.shopManager.CurrPrice(item))
                    {
                        infoPanelTemplate.BuyBtn.interactable = true;
                    }
                    else
                    {
                        infoPanelTemplate.BuyBtn.interactable = false;
                    }
                }
                else
                {
                    if (item.Components.Count > 0 && this.shopManager.PlayerStats.PlayerGold >= this.shopManager.CurrPrice(item))
                    {
                        if (inventory.OneComponentsBought(item))
                        {
                            infoPanelTemplate.BuyBtn.interactable = true;
                        }
                        else
                        {
                            infoPanelTemplate.BuyBtn.interactable = false;
                        }
                    }
                    else
                    {
                        infoPanelTemplate.BuyBtn.interactable = false;
                    }
                }
            }
            else
            {
                infoPanelTemplate.BuyBtn.interactable = false;
            }
        }
        //Debug.Log("RefreshInfoPanel Start");
    }

    [Client]
    private void LoadPanels()
    {
        //Debug.Log("LoadPanels Start");
        for (int i = 0; i < this.shopItemSo.Count; i++)
        {
            this.shopPanels[i].TitleTxt.text = this.shopItemSo[i].Title;
            this.shopPanels[i].PriceVal.text = "Price: " + this.shopItemSo[i].TotalPrice.ToString();
            this.shopPanels[i].ItemIm.sprite = this.shopItemSo[i].Image;
        }
        //Debug.Log("LoadPanels End");
    }

    //Z Inventory------------------------------------------------------------------------------------------------------------------
    [Command]
    public void CmdAddToEquipment(int itemIndex)
    {
        RpcAddToEquipment(itemIndex);
    }

    [TargetRpc]
    public void RpcAddToEquipment(int itemIndex)
    {
        ShopItemSo item = this.shopItemSo[itemIndex];
        foreach (ShopItemSo component in item.Components)
        {
            for (int i = 0; i < this.inventory.Equipment.Length; i++)
            {
                if (this.inventory.Equipment[i] != null)
                {
                    if (this.inventory.Equipment[i].Title == component.Title)
                    {
                        this.inventory.CmdRemoveItem(i);
                        Debug.Log("usuniêcie przedmiotu");
                        break;
                    }
                }
            }
        }
        for (int i = 0; i < this.inventory.Equipment.Length; i++)
        {
            if (this.inventory.Equipment[i] == null)
            {
                this.inventory.Equipment[i] = item;
                this.inventory.CmdRefreshSlots();
                return;
            }
        }
    }

    //Z shopManagera------------------------------------------------------------------------------------------------------------------
    [Command]
    public void CmdBuy(int itemIndex)
    {
        RpcBuy(itemIndex);
    }

    [ClientRpc]
    public void RpcBuy(int itemIndex)
    {
        ShopItemSo item = this.shopItemSo[itemIndex];
        float currPrice = this.shopManager.CurrPrice(item);
        if (this.shopManager.PlayerStats.PlayerGold >= currPrice)
        {
            this.shopManager.CmdSubtractPurchasedItemCostFromOwnedGold(currPrice);
            this.CmdAddToEquipment(itemIndex);
            this.shopManager.PlayerStats.AddItemStatsToPlayer(item);
        }
    }
}
