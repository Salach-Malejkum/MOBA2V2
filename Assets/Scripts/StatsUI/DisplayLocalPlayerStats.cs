using UnityEngine;

public class DisplayLocalPlayerStats : MonoBehaviour
{
    [SerializeField]
    private UIStatsTemplate template = null;
    [SerializeField]
    private PlayerStats playerStats = null;

    private void Awake()
    {
        Debug.Log("Listeners added");
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
        Debug.Log("Listeners removed");
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
        Debug.Log("Base Stats Loaded");
        LoadLocalPlayerStats();
    }

    private void LocalPlayerHealthUpdated(float newCurrHP, float newMaxHP)
    {
        if (newCurrHP > newMaxHP)
        {
            newCurrHP = newMaxHP;
        }
        this.template.HealthBarImage.fillAmount = newCurrHP / newMaxHP;
        this.template.HealthVal.text = newCurrHP + "/" + newMaxHP;
    }

    private void LocalPlayerAttackUpdated(float newStat)
    {
        Debug.Log("Attack updated");
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
        this.template.HealthBarImage.fillAmount = this.playerStats.UnitCurrentHealth / playerStats.UnitMaxHealth;
        this.template.HealthVal.text = this.playerStats.UnitCurrentHealth + "/" + playerStats.UnitMaxHealth;
        this.template.AttackVal.text = "Attack: " + this.playerStats.UnitAttackDamage.ToString();
        this.template.AbilityPowerVal.text = "Abiliti Power: " + this.playerStats.UnitAbilityPower.ToString();
        this.template.ArmorVal.text = "Armor: " + this.playerStats.UnitArmor.ToString();
        this.template.MagicResistVal.text = "Magic Resist: " + this.playerStats.UnitMagicResist.ToString();
        this.template.MovmentSpeedVal.text = "Movement Speed: " + this.playerStats.UnitMovementSpeed.ToString();
        this.template.AttackSpeed.text = "Attack Speed: " + this.playerStats.AttackSpeed.ToString();
        this.template.CooldownReductionVal.text = "Cooldown Reduction: " + this.playerStats.UnitCooldownReduction.ToString();
        this.template.HealthRegenVal.text = "+" + this.playerStats.PlayerHealthRegen.ToString();
    }
}
