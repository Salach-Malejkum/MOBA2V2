using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverPannelHandler : MonoBehaviour
{
    [SerializeField]
    private HoverInfoPanelTemplate template;
    [SerializeField]
    private Canvas inventoryCanvas;

    public void LoadPanel(ShopItemSo item)
    {
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(this.inventoryCanvas.transform as RectTransform, Input.mousePosition, this.inventoryCanvas.worldCamera, out pos);
        transform.position = inventoryCanvas.transform.TransformPoint(pos);
        this.template.TitleTxt.text = item.Title;
        this.template.ItemIm.sprite = item.Image;
        this.template.AttackVal.text = "AD: " + item.Attack.ToString();
        this.template.AbilityPowerVal.text = "AP: " + item.AbilityPower.ToString();
        this.template.ArmorVal.text = "AR: " + item.Armor.ToString();
        this.template.MagicResistVal.text = "MR: " + item.MagicResist.ToString();
        this.template.CooldownReductionVal.text = "CDR: " + item.CooldownReduction.ToString();
        this.template.HealthVal.text = "HP: " + item.Health.ToString();
    }
}
