using Mirror;
using UnityEngine;

public class DisplayLocalPlayerStats : MonoBehaviour
{
    [SerializeField]
    private UIStatsTemplate template;
    [SerializeField]
    private PlayerStats playerStats;

    private void Awake()
    {
        playerStats.OnHealthUptade += LocalPlayerHealthUpdated;
        playerStats.OnMaxHealthUptade += LocalPlayerHealthUpdated;
        playerStats.OnAttackUptade += LocalPlayerAttackUpdated;
        playerStats.OnAbilityPowerUptade += LocalPlayerAbilityPowerUpdated;
        playerStats.OnArmorUptade += LocalPlayerArmorUpdated;
        playerStats.OnMagicResistUptade += LocalPlayerMagicResistUpdated;
        playerStats.OnAttackSpeedUptade += LocalPlayerAttackSpeedUpdated;
        playerStats.OnMovementSpeedUptade += LocalPlayerMovementSpeedUpdated;
        playerStats.OnCooldownReductionUptade += LocalPlayerCooldownReductionUpdated;
        playerStats.OnHealthRegenUptade += LocalPlayerHealthRegenUpdated;
    }

    private void OnDestroy()
    {
        playerStats.OnHealthUptade -= LocalPlayerHealthUpdated;
        playerStats.OnMaxHealthUptade -= LocalPlayerHealthUpdated;
        playerStats.OnAttackUptade -= LocalPlayerAttackUpdated;
        playerStats.OnAbilityPowerUptade -= LocalPlayerAbilityPowerUpdated;
        playerStats.OnArmorUptade -= LocalPlayerArmorUpdated;
        playerStats.OnMagicResistUptade -= LocalPlayerMagicResistUpdated;
        playerStats.OnAttackSpeedUptade -= LocalPlayerAttackSpeedUpdated;
        playerStats.OnMovementSpeedUptade -= LocalPlayerMovementSpeedUpdated;
        playerStats.OnCooldownReductionUptade -= LocalPlayerCooldownReductionUpdated;
        playerStats.OnHealthRegenUptade -= LocalPlayerHealthRegenUpdated;
    }

    void Start()
    {
        LoadLocalPlayerStats();
    }

    private void LocalPlayerHealthUpdated(float newCurrHP, float newMaxHP)
    {
        if (newCurrHP > newMaxHP)
        {
            newCurrHP = newMaxHP;
        }
        template.HealthBarImage.fillAmount = newCurrHP / newMaxHP;
        template.HealthVal.text = newCurrHP + "/" + newMaxHP;
    }

    private void LocalPlayerAttackUpdated(float newStat)
    {
        this.template.AttackVal.text = "Attack: " + newStat.ToString();
    }

    private void LocalPlayerAbilityPowerUpdated(float newStat)
    {
        this.template.AbilityPowerVal.text = "Abiliti Power: " + newStat.ToString();
    }

    private void LocalPlayerArmorUpdated(float newStat)
    {
        this.template.ArmorVal.text = "Armor: " + newStat.ToString();
    }

    private void LocalPlayerMagicResistUpdated(float newStat)
    {
        this.template.MagicResistVal.text = "Magic Resist: " + newStat.ToString();
    }

    private void LocalPlayerMovementSpeedUpdated(float newStat)
    {
        this.template.MovmentSpeedVal.text = "Movement Speed: " + newStat.ToString();
    }

    private void LocalPlayerAttackSpeedUpdated(float newStat)
    {
        this.template.AttackSpeed.text = "Attack Speed: " + newStat.ToString();
    }

    private void LocalPlayerCooldownReductionUpdated(float newStat)
    {
        this.template.CooldownReductionVal.text = "Cooldown Reduction: " + newStat.ToString();
    }

    private void LocalPlayerHealthRegenUpdated(float newStat)
    {
        this.template.HealthRegenVal.text = "+" + newStat.ToString();
    }

    private void LoadLocalPlayerStats()
    {
        template.HealthBarImage.fillAmount = playerStats.UnitCurrentHealth / playerStats.UnitMaxHealth;
        template.HealthVal.text = playerStats.UnitCurrentHealth + "/" + playerStats.UnitMaxHealth;
        this.template.AttackVal.text = "Attack: " + playerStats.UnitAttackDamage.ToString();
        this.template.AbilityPowerVal.text = "Abiliti Power: " + playerStats.UnitAbilityPower.ToString();
        this.template.ArmorVal.text = "Armor: " + playerStats.UnitArmor.ToString();
        this.template.MagicResistVal.text = "Magic Resist: " + playerStats.UnitMagicResist.ToString();
        this.template.MovmentSpeedVal.text = "Movement Speed: " + playerStats.UnitMovementSpeed.ToString();
        this.template.AttackSpeed.text = "Attack Speed: " + playerStats.AttackSpeed.ToString();
        this.template.CooldownReductionVal.text = "Cooldown Reduction: " + playerStats.UnitCooldownReduction.ToString();
        this.template.HealthRegenVal.text = "+" + playerStats.PlayerHealthRegen.ToString();
    }
}
