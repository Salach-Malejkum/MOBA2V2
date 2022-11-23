using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TabManager : MonoBehaviour
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
    private List<Button> infoBuyButtons;

    [SerializeField]
    private ShopManager shopManager;

    private GameObject activeInfoPanel = null;

    private readonly int delayAmount = 1;
    private float timer;

    void Start()
    {
        for (int i = 0; i < this.shopItemSo.Count; i++)
        {
            this.shopPanelsGo[i].SetActive(true);
        }
        this.LoadPanels();
        this.CheckPurchasable();
    }

    void FixedUpdate()
    {
        this.timer += Time.deltaTime;

        if (this.timer >= this.delayAmount)
        {
            this.timer = 0f;
            this.CheckPurchasable();
        }
    }

    private void CheckPurchasable()
    {
        
        if (this.shopManager.IsInBorder())
        {
            if (!Inventory.instance.IsEqFull())
            {
                for (int i = 0; i < this.shopItemSo.Count; i++)
                {
                    if (this.shopManager.GoldValue >= this.shopManager.CurrPrice(this.shopItemSo[i]))
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
                    if (this.shopItemSo[i].Components.Count > 0 && this.shopManager.GoldValue >= this.shopManager.CurrPrice(this.shopItemSo[i]))
                    {
                        if (OneComponentsBought(this.shopItemSo[i]))
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
    }

    public bool OneComponentsBought(ShopItemSo item)
    {
        foreach (ShopItemSo component in item.Components)
        {
            if (Inventory.instance.ItemInEq(component))
            {
                return true;
            }
        }
        return false;
    }

    public void GetItemNo(int itemNo)
    {
        this.shopManager.Buy(this.shopItemSo[itemNo]);
        this.CheckPurchasable();
    }

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
            if (Inventory.instance.ItemInEq(this.shopItemSo[itemNo].Components[i]))
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
    }

    public void RefreshInfoPanel()
    {
        if (this.activeInfoPanel != null)
        {
            InfoPanelsTemplate infoPanelTemplate = this.activeInfoPanel.GetComponent<InfoPanelsTemplate>();
            ShopItemSo item = this.shopItemSo.First(item => item.Title == this.activeInfoPanel.GetComponent<InfoPanelsTemplate>().TitleTxt.text);
            infoPanelTemplate.PriceVal.text = this.shopManager.CurrPrice(item).ToString();
            for (int i = 0; i < item.Components.Count; i++)
            {
                if (Inventory.instance.ItemInEq(item.Components[i]))
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
                if (!Inventory.instance.IsEqFull())
                {
                    if (this.shopManager.GoldValue >= this.shopManager.CurrPrice(item))
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
                    if (item.Components.Count > 0 && this.shopManager.GoldValue >= this.shopManager.CurrPrice(item))
                    {
                        if (OneComponentsBought(item))
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
    }

    private void LoadPanels()
    {
        for (int i = 0; i < this.shopItemSo.Count; i++)
        {
            this.shopPanels[i].TitleTxt.text = this.shopItemSo[i].Title;
            this.shopPanels[i].PriceVal.text = "Price: " + this.shopItemSo[i].TotalPrice.ToString();
            this.shopPanels[i].ItemIm.sprite = this.shopItemSo[i].Image;
        }
    }
}
