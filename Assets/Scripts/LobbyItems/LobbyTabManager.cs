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
        ShopItemSo item = this.shopItemSo[itemNo];
        int panelIndex = item.Components.Count;
        this.activeInfoPanel = this.panels.detailsPanels[panelIndex];
        this.activeInfoPanel.SetActive(true);
        InfoPanelsTemplate infoPanelTemplate = this.activeInfoPanel.GetComponent<InfoPanelsTemplate>();
        infoPanelTemplate.TitleTxt.text = item.Title;
        infoPanelTemplate.AttackVal.text = "Attack: " + item.Attack.ToString();
        infoPanelTemplate.AbilityPowerVal.text = "Ability Power: " + item.AbilityPower.ToString();
        infoPanelTemplate.ArmorVal.text = "Armor: " + item.Armor.ToString();
        infoPanelTemplate.MagicResistVal.text = "Magic Resist: " + item.MagicResist.ToString();
        infoPanelTemplate.CooldownReductionVal.text = "Cooldown Reduction: " + item.CooldownReduction.ToString();
        infoPanelTemplate.HealthVal.text = "Health: " + item.Health.ToString();
        infoPanelTemplate.PriceVal.text = item.TotalPrice.ToString();
        infoPanelTemplate.ItemIm.sprite = item.Image;

        for (int i = 0; i < item.Components.Count; i++)
        {
            infoPanelTemplate.ItemImComp[i].sprite = item.Components[i].Image;
            infoPanelTemplate.PriceValComp[i].text = item.Components[i].TotalPrice.ToString();
        }
    }

    private void LoadPanels()
    {
        for (int i = 0; i < this.shopItemSo.Count; i++)
        {
            ShopItemSo item = this.shopItemSo[i];
            this.shopPanels[i].TitleTxt.text = item.Title;
            this.shopPanels[i].PriceVal.text = "Price: " + item.TotalPrice.ToString();
            this.shopPanels[i].ItemIm.sprite = item.Image;
        }
    }
}
