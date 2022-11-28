using UnityEngine;
using Mirror;
using System;

public class PlayerStats : UnitStats
{
    [SerializeField] protected float playerGold = 100f;
    public float PlayerGold
    {
        get { return playerGold; }
        set { playerGold = value; }
    }
    [SyncVar] [SerializeField] protected float playerExp = 0f;
    [SyncVar(hook = nameof(OnHealthRegenChanged))] [SerializeField] private float playerHealthRegen = 2.5f;
    public float PlayerHealthRegen
    {
        get { return playerHealthRegen; }
    }

    [SerializeField]
    private string playerName;
    public string PlayerName
    {
        get { return playerName; }
        set { playerName = value; }
    }

    private float timer;
    [SyncVar] public string lane;
    [SyncVar] public string side;

    public event Action<float> OnHealthRegenUptade;
    public event Action<float, float> OnHealthUptade;
    public event Action<float, float> OnMaxHealthUptade;
    public event Action<float> OnAttackUptade;
    public event Action<float> OnAbilityPowerUptade;
    public event Action<float> OnArmorUptade;
    public event Action<float> OnMagicResistUptade;
    public event Action<float> OnMovementSpeedUptade;
    public event Action<float> OnAttackSpeedUptade;
    public event Action<float> OnCooldownReductionUptade;

    public override void OnStartAuthority() {
        this.unitCurrentHealth = this.unitMaxHealth;
        this.timer = this.regenerationIntervalSeconds;
        this.onUnitDeath += CmdReadyToRespawn;
    }

    public override void RemoveHealthOnNormalAttack(float damageAmount, GameObject agressor) {
        base.RemoveHealthOnNormalAttack(damageAmount, agressor);
    }

    private void OnHealthRegenChanged(float oldHPRegen, float newHPRegen)
    {
        OnHealthRegenUptade?.Invoke(newHPRegen);
    }

    [Command]
    private void CmdReadyToRespawn() {
        RpcReadyToRespawn();
    }

    [ClientRpc]
    private void RpcReadyToRespawn() {
        this.gameObject.SetActive(false);
    }
    
    public void AddItemStatsToPlayer(ShopItemSo item)
    {
        if (item.Health != 0)
        {
            this.unitMaxHealth += item.Health;
            this.OnMaxHealthUptade?.Invoke(this.unitCurrentHealth, this.unitMaxHealth);
        }

        if (item.Armor != 0)
        {
            this.unitArmor += item.Armor;
            this.OnArmorUptade?.Invoke(this.unitArmor);
        }


        if (item.MagicResist != 0)
        {
            this.unitMagicResist += item.MagicResist;
            this.OnMagicResistUptade?.Invoke(this.unitMagicResist);
        }


        if (item.Attack != 0)
        {
            this.unitAttackDamage += item.Attack;
            this.OnAttackUptade?.Invoke(this.unitAttackDamage);
        }


        if (item.AbilityPower != 0)
        {
            this.unitAbilityPower += item.AbilityPower;
            this.OnAbilityPowerUptade?.Invoke(this.unitAbilityPower);
        }


        if (item.CooldownReduction != 0)
        {
            this.unitCooldownReduction += item.CooldownReduction;
            this.OnCooldownReductionUptade?.Invoke(this.unitCooldownReduction);
        }

    }

    public void SubtractItemStatsFromPlayer(ShopItemSo item)
    {
        if (item.Health != 0)
        {
            this.unitMaxHealth -= item.Health;
            this.OnMaxHealthUptade?.Invoke(this.unitCurrentHealth, this.unitMaxHealth);
            if (this.unitMaxHealth > this.unitCurrentHealth)
            {
                this.unitCurrentHealth = this.unitMaxHealth;
            }
        }

        if (item.Armor != 0)
        {
            this.unitArmor -= item.Armor;
            this.OnArmorUptade?.Invoke(this.unitArmor);
        }


        if (item.MagicResist != 0)
        {
            this.unitMagicResist -= item.MagicResist;
            this.OnMagicResistUptade?.Invoke(this.unitMagicResist);
        }


        if (item.Attack != 0)
        {
            this.unitAttackDamage -= item.Attack;
            this.OnAttackUptade?.Invoke(this.unitAttackDamage);
        }


        if (item.AbilityPower != 0)
        {
            this.unitAbilityPower -= item.AbilityPower;
            this.OnAbilityPowerUptade?.Invoke(this.unitAbilityPower);
        }


        if (item.CooldownReduction != 0)
        {
            this.unitCooldownReduction -= item.CooldownReduction;
            this.OnCooldownReductionUptade?.Invoke(this.unitCooldownReduction);
        }
    }
}
