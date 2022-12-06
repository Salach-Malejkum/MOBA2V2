using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HoverInfoPanelTemplate : MonoBehaviour
{
    [SerializeField]
    private TMP_Text titleTxt;
    public TMP_Text TitleTxt
    {
        get { return titleTxt; }
    }

    [SerializeField]
    private Image itemIm;
    public Image ItemIm
    {
        get { return itemIm; }
    }

    [SerializeField]
    private TMP_Text attackVal;
    public TMP_Text AttackVal
    {
        get { return attackVal; }
    }

    [SerializeField]
    private TMP_Text abilityPowerVal;
    public TMP_Text AbilityPowerVal
    {
        get { return abilityPowerVal; }
    }

    [SerializeField]
    private TMP_Text armorVal;
    public TMP_Text ArmorVal
    {
        get { return armorVal; }
    }

    [SerializeField]
    private TMP_Text magicResistVal;
    public TMP_Text MagicResistVal
    {
        get { return magicResistVal; }
    }

    [SerializeField]
    private TMP_Text cooldownReductionVal;
    public TMP_Text CooldownReductionVal
    {
        get { return cooldownReductionVal; }
    }

    [SerializeField]
    private TMP_Text healthVal;
    public TMP_Text HealthVal
    {
        get { return healthVal; }
    }
}
