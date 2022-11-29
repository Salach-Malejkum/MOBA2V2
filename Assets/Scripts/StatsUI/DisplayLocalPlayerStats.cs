using Mirror;
using UnityEngine;

public class DisplayLocalPlayerStats : MonoBehaviour
{
    [SerializeField]
    private UIStatsTemplate template = null;
    private PlayerStats playerStats = null;
    [SerializeField]
    private PlayerSkills playerSkills = null;

    private void Awake()
    {
        foreach(NetworkIdentity player in NetworkManagerLobby.Instance.PlayersLoadedToScene)
        {
            if (player.isLocalPlayer)
            {
                playerStats = player.gameObject.GetComponent<PlayerStats>();
            }
        }
        this.playerStats.OnUnitHealthUptade += LocalPlayerHealthUpdated;
        this.playerStats.OnUnitMaxHealthUptade += LocalPlayerHealthUpdated;
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
        this.playerStats.OnUnitMaxHealthUptade -= LocalPlayerHealthUpdated;
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
        this.template.AttackVal.text = "AD " + newStat.ToString();
    }

    private void LocalPlayerAbilityPowerUpdated(float newStat)
    {
        this.template.AbilityPowerVal.text = "AP " + newStat.ToString();
    }

    private void LocalPlayerArmorUpdated(float newStat)
    {
        this.template.ArmorVal.text = "A " + newStat.ToString();
    }

    private void LocalPlayerMagicResistUpdated(float newStat)
    {
        this.template.MagicResistVal.text = "MR " + newStat.ToString();
    }

    private void LocalPlayerMovementSpeedUpdated(float newStat)
    {
        this.template.MovmentSpeedVal.text = "MS " + newStat.ToString();
    }

    private void LocalPlayerAttackSpeedUpdated(float newStat)
    {
        this.template.AttackSpeed.text = "AS " + newStat.ToString();
    }

    private void LocalPlayerCooldownReductionUpdated(float newStat)
    {
        this.template.CooldownReductionVal.text = "CD " + newStat.ToString();
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
        this.template.AttackVal.text = "AD " + this.playerStats.UnitAttackDamage.ToString();
        this.template.AbilityPowerVal.text = "AP " + this.playerStats.UnitAbilityPower.ToString();
        this.template.ArmorVal.text = "A " + this.playerStats.UnitArmor.ToString();
        this.template.MagicResistVal.text = "MR " + this.playerStats.UnitMagicResist.ToString();
        this.template.MovmentSpeedVal.text = "MS " + this.playerStats.UnitMovementSpeed.ToString();
        this.template.AttackSpeed.text = "AS " + this.playerStats.AttackSpeed.ToString();
        this.template.CooldownReductionVal.text = "CD " + this.playerStats.UnitCooldownReduction.ToString();
        this.template.HealthRegenVal.text = "+" + this.playerStats.PlayerHealthRegen.ToString();
    }
}
