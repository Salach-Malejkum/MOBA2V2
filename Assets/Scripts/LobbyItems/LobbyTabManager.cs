using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyTabManager : MonoBehaviour
{
    [SerializeField]
    private List<ShopItemSo> shopItemSo;
    [SerializeField]
    private List<GameObject> shopPanelsGo;
    [SerializeField]
    private List<ShopTemplate> shopPanels;
    [SerializeField]
    private LobbyDetailsPanels panels;

    private GameObject activeInfoPanel = null;


    private void Start()
    {
        for (int i = 0; i < this.shopItemSo.Count; i++)
        {
            this.shopPanelsGo[i].SetActive(true);
        }
        this.LoadPanels();
    }

    public void InfoItem(int itemNo)
    {
        this.panels.DesactivateAllInfoPanels();
        int panelIndex = this.shopItemSo[itemNo].Components.Count;
        this.activeInfoPanel = this.panels.detailsPanels[panelIndex];
        this.activeInfoPanel.SetActive(true);
        InfoPanelsTemplate infoPanelTemplate = this.activeInfoPanel.GetComponent<InfoPanelsTemplate>();
        infoPanelTemplate.TitleTxt.text = this.shopItemSo[itemNo].Title;
        infoPanelTemplate.AttackVal.text = "Attack: " + this.shopItemSo[itemNo].Attack.ToString();
        infoPanelTemplate.AbilityPowerVal.text = "Ability Power: " + this.shopItemSo[itemNo].AbilityPower.ToString();
        infoPanelTemplate.ArmorVal.text = "Armor: " + this.shopItemSo[itemNo].Armor.ToString();
        infoPanelTemplate.MagicResistVal.text = "Magic Resist: " + this.shopItemSo[itemNo].MagicResist.ToString();
        infoPanelTemplate.CooldownReductionVal.text = "Cooldown Reduction: " + this.shopItemSo[itemNo].CooldownReduction.ToString();
        infoPanelTemplate.HealthVal.text = "Health: " + this.shopItemSo[itemNo].Health.ToString();
        infoPanelTemplate.PriceVal.text = this.shopItemSo[itemNo].TotalPrice.ToString();
        infoPanelTemplate.ItemIm.sprite = this.shopItemSo[itemNo].Image;

        for (int i = 0; i < this.shopItemSo[itemNo].Components.Count; i++)
        {

            infoPanelTemplate.ItemImComp[i].sprite = this.shopItemSo[itemNo].Components[i].Image;
            infoPanelTemplate.PriceValComp[i].text = this.shopItemSo[itemNo].Components[i].TotalPrice.ToString();
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
