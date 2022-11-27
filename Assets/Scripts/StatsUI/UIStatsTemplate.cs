using TMPro;
using UnityEngine;

public class UIStatsTemplate : MonoBehaviour
{
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
    private TMP_Text movmentSpeedVal;
    public TMP_Text MovmentSpeedVal
    {
        get { return movmentSpeedVal; }
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

    [SerializeField]
    private TMP_Text healthRegenVal;
    public TMP_Text HealthRegenVal
    {
        get { return healthRegenVal; }
    }
}
