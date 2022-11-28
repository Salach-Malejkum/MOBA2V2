using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public abstract class UnitStats : NetworkBehaviour
{
    [SyncVar(hook = nameof(OnMaxHealthChanged))][SerializeField] protected float unitMaxHealth = 100f;
    public float UnitMaxHealth
    {
        get { return unitMaxHealth; }
    }
    [SyncVar(hook = nameof(OnHealthChanged))][SerializeField] protected float unitCurrentHealth = 0f;
    public float UnitCurrentHealth
    {
        get { return unitCurrentHealth; }
    }
    [SyncVar(hook = nameof(OnArmorChanged))] [SerializeField] protected float unitArmor = 0f;
    public float UnitArmor
    {
        get { return unitArmor; }
    }
    [SyncVar(hook = nameof(OnMagicResistChanged))] [SerializeField] protected float unitMagicResist = 0f;
    public float UnitMagicResist
    {
        get { return unitMagicResist; }
    }
    [SyncVar(hook = nameof(OnAttackChanged))] [SerializeField] protected float unitAttackDamage = 0f;
    public float UnitAttackDamage
    {
        get { return unitAttackDamage; }
    }
    [SyncVar(hook = nameof(OnAttackSpeedChanged))] [SerializeField] protected float attackSpeed = 1f;
    public float AttackSpeed
    {
        get { return attackSpeed; }
    }
    [SyncVar(hook = nameof(OnAbilityPowerChanged))] [SerializeField] protected float unitAbilityPower = 0f;
    public float UnitAbilityPower
    {
        get { return unitAbilityPower; }
    }
    [SyncVar(hook = nameof(OnMovementSpeedChanged))] [SerializeField] protected float unitMovementSpeed = 0f;
    public float UnitMovementSpeed
    {
        get { return unitMovementSpeed; }
    }
    [SyncVar(hook = nameof(OnCooldownReductionChanged))] [SerializeField] protected float unitCooldownReduction = 0f;
    public float UnitCooldownReduction
    {
        get { return unitCooldownReduction; }
    }
    protected int currentLevel = 1;
    protected float regenerationIntervalSeconds = 1f;

    public event Action onUnitDeath;

    [Server]
    public virtual void AddHealth(float hpAmount)
    {
        this.unitCurrentHealth += hpAmount;
        if (this.unitCurrentHealth > this.unitMaxHealth)
        {
            this.unitCurrentHealth = this.unitMaxHealth;
        }
    }

    public event Action<float, float> OnHealthUptade;
    public event Action<float, float> OnMaxHealthUptade;
    public event Action<float> OnAttackUptade;
    public event Action<float> OnAbilityPowerUptade;
    public event Action<float> OnArmorUptade;
    public event Action<float> OnMagicResistUptade;
    public event Action<float> OnMovementSpeedUptade;
    public event Action<float> OnAttackSpeedUptade;
    public event Action<float> OnCooldownReductionUptade;


    public virtual void RemoveHealthOnNormalAttack(float hpAmount, GameObject aggresor)
    {
        this.unitCurrentHealth -= (hpAmount - this.unitArmor);
    }

    public virtual void RemoveHealthOnMagicAttack(float hpAmount)
    {
        this.unitCurrentHealth -= (hpAmount - this.unitMagicResist);
    }

    private void OnHealthChanged(float oldHP, float newHP) {
        this.OnHealthUptade?.Invoke(newHP, unitMaxHealth);
        if (this.unitCurrentHealth <= 0)
        {
            this.onUnitDeath?.Invoke();
        }
    }

    private void OnMaxHealthChanged(float oldMaxHP, float newMaxHP)
    {
        this.OnMaxHealthUptade?.Invoke(this.unitCurrentHealth, newMaxHP);
    }

    private void OnAttackChanged(float oldAttack, float newAttack)
    {
        Debug.Log("AttackChanged");
        //Debug.Log(this.OnAttackUptade.GetInvocationList().Length);
        Debug.Log("old attack: " + oldAttack + "new attack" + newAttack);
        this.OnAttackUptade?.Invoke(newAttack);
    }

    private void OnAbilityPowerChanged(float oldAP, float newAP)
    {
        this.OnAbilityPowerUptade?.Invoke(newAP);
    }

    private void OnArmorChanged(float oldArmor, float newArmor)
    {
        this.OnArmorUptade?.Invoke(newArmor);
    }

    private void OnMagicResistChanged(float oldMR, float newMR)
    {
        this.OnMagicResistUptade?.Invoke(newMR);
    }

    private void OnMovementSpeedChanged(float oldMS, float newMS)
    {
        this.OnMovementSpeedUptade?.Invoke(newMS);
    }

    private void OnAttackSpeedChanged(float oldAS, float newAS)
    {
        this.OnAttackSpeedUptade?.Invoke(newAS);
    }

    private void OnCooldownReductionChanged(float oldCD, float newCD)
    {
        this.OnCooldownReductionUptade?.Invoke(newCD);
    }
}