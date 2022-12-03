using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverPannelHandler : MonoBehaviour
{
    [SerializeField]
    private HoverInfoPanelTemplate template;
    [SerializeField]
    private Canvas inventoryCanvas;

    //private void Update()
    //{
    //    if (this.gameObject.activeSelf)
    //    {
    //        Vector2 pos;
    //        RectTransformUtility.ScreenPointToLocalPointInRectangle(inventoryCanvas.transform as RectTransform, Input.mousePosition, inventoryCanvas.worldCamera, out pos);
    //        pos.x += this.GetComponent<RectTransform>().rect.width/2 + 2;
    //        pos.y += this.GetComponent<RectTransform>().rect.height/2 + 2;
    //        transform.position = inventoryCanvas.transform.TransformPoint(pos);
    //    }
    //}

    public void LoadPanel(ShopItemSo item)
    {
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
