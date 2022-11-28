using UnityEngine;

public class DisplayLocalPlayerStats : MonoBehaviour
{
    [SerializeField]
    private UIStatsTemplate template = null;
    [SerializeField]
    private PlayerStats playerStats = null;
    [SerializeField]
    private PlayerSkills playerSkills = null;

    private void Awake()
    {
        this.playerStats.OnUnitHealthUptade += LocalPlayerHealthUpdated;
        this.playerStats.OnHealthUptade += LocalPlayerHealthUpdated;
        this.playerStats.OnMaxHealthUptade += LocalPlayerHealthUpdated;
        this.playerStats.OnAttackUptade += LocalPlayerAttackUpdated;
        this.playerStats.OnAbilityPowerUptade += LocalPlayerAbilityPowerUpdated;
        this.playerStats.OnArmorUptade += LocalPlayerArmorUpdated;
        this.playerStats.OnMagicResistUptade += LocalPlayerMagicResistUpdated;
        this.playerStats.OnAttackSpeedUptade += LocalPlayerAttackSpeedUpdated;
        this.playerStats.OnMovementSpeedUptade += LocalPlayerMovementSpeedUpdated;
        this.playerStats.OnCooldownReductionUptade += LocalPlayerCooldownReductionUpdated;
        this.playerStats.OnHealthRegenUptade += LocalPlayerHealthRegenUpdated;
        this.playerSkills.QUsed += LocalPlayerUsedQ;
        this.playerSkills.WUsed += LocalPlayerUsedW;
        this.playerSkills.EUsed += LocalPlayerUsedE;
        this.playerSkills.RUsed += LocalPlayerUsedR;
        this.playerSkills.QRedy += LocalPlayerQRedy;
        this.playerSkills.WRedy += LocalPlayerWRedy;
        this.playerSkills.ERedy += LocalPlayerERedy;
        this.playerSkills.RRedy += LocalPlayerRRedy;
    }

    private void OnDestroy()
    {
        this.playerStats.OnUnitHealthUptade -= LocalPlayerHealthUpdated;
        this.playerStats.OnHealthUptade -= LocalPlayerHealthUpdated;
        this.playerStats.OnMaxHealthUptade -= LocalPlayerHealthUpdated;
        this.playerStats.OnAttackUptade -= LocalPlayerAttackUpdated;
        this.playerStats.OnAbilityPowerUptade -= LocalPlayerAbilityPowerUpdated;
        this.playerStats.OnArmorUptade -= LocalPlayerArmorUpdated;
        this.playerStats.OnMagicResistUptade -= LocalPlayerMagicResistUpdated;
        this.playerStats.OnAttackSpeedUptade -= LocalPlayerAttackSpeedUpdated;
        this.playerStats.OnMovementSpeedUptade -= LocalPlayerMovementSpeedUpdated;
        this.playerStats.OnCooldownReductionUptade -= LocalPlayerCooldownReductionUpdated;
        this.playerStats.OnHealthRegenUptade -= LocalPlayerHealthRegenUpdated;
        this.playerSkills.QUsed -= LocalPlayerUsedQ;
        this.playerSkills.WUsed -= LocalPlayerUsedW;
        this.playerSkills.EUsed -= LocalPlayerUsedE;
        this.playerSkills.RUsed -= LocalPlayerUsedR;
        this.playerSkills.QRedy -= LocalPlayerQRedy;
        this.playerSkills.WRedy -= LocalPlayerWRedy;
        this.playerSkills.ERedy -= LocalPlayerERedy;
        this.playerSkills.RRedy -= LocalPlayerRRedy;
    }

    void Start()
    {
        this.LoadLocalPlayerStats();
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

    private void LocalPlayerUsedQ()
    {
        this.template.QOverlay.SetActive(true);
    }

    private void LocalPlayerUsedW()
    {
        this.template.WOverlay.SetActive(true);
    }

    private void LocalPlayerUsedE()
    {
        this.template.EOverlay.SetActive(true);
    }

    private void LocalPlayerUsedR()
    {
        this.template.ROverlay.SetActive(true);
    }

    private void LocalPlayerQRedy()
    {
        this.template.QOverlay.SetActive(false);
    }

    private void LocalPlayerWRedy()
    {
        this.template.WOverlay.SetActive(false);
    }

    private void LocalPlayerERedy()
    {
        this.template.EOverlay.SetActive(false);
    }

    private void LocalPlayerRRedy()
    {
        this.template.ROverlay.SetActive(false);
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
