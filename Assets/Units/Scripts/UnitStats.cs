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
    [SyncVar(hook = nameof(OnAttackSpeedChanged))] [SerializeField] protected float unitAttackSpeed = 1f;
    public float UnitAttackSpeed
    {
        get { return unitAttackSpeed; }
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
    [SerializeField] private bool IsWinCondition = false;

    protected int unitCurrentLevel = 1;
    protected float unitRegenerationIntervalSeconds = 1f;

    [SyncVar][SerializeField] public float resourcesOnDeath;
    [SyncVar][SerializeField] public float goldOnDeath;
    [SyncVar] public GameObject lastAggressor;

    public event Action onUnitDeath;
    public static event Action onWinConditionMet;

    [Server]
    public virtual void AddHealth(float hpAmount)
    {
        this.unitCurrentHealth += hpAmount;
        if (this.unitCurrentHealth > this.unitMaxHealth)
        {
            this.unitCurrentHealth = this.unitMaxHealth;
        }
    }

    public event Action<float, float> OnUnitHealthUpdate;
    public event Action<float, float> OnUnitMaxHealthUpdate;
    public event Action<float> OnAttackUpdate;
    public event Action<float> OnAbilityPowerUpdate;
    public event Action<float> OnArmorUpdate;
    public event Action<float> OnMagicResistUpdate;
    public event Action<float> OnMovementSpeedUpdate;
    public event Action<float> OnAttackSpeedUpdate;
    public event Action<float> OnCooldownReductionUpdate;

    public virtual void RemoveHealthOnNormalAttack(float hpAmount, GameObject aggressor)
    {
        this.unitCurrentHealth -= (hpAmount - this.unitArmor);

        if (aggressor.tag == "Player")
        {
            this.lastAggressor = aggressor;
            Debug.Log("Aggressor: " + aggressor);
        }
        this.OnDeathCheck();
    }

    public virtual void RemoveHealthOnMagicAttack(float hpAmount)
    {
        this.unitCurrentHealth -= (hpAmount - this.unitMagicResist);
        this.OnDeathCheck();
    }

    private void OnHealthChanged(float oldHP, float newHP) {
        this.OnUnitHealthUpdate?.Invoke(newHP, unitMaxHealth);
        this.OnDeathCheck();
    }

    protected void OnDeathCheck() 
    {
        if (this.unitCurrentHealth <= 0) 
        {
            if (this.IsWinCondition) 
            {
                NetworkManagerLobby.Instance.whichSideLost = LayerMask.LayerToName(this.gameObject.layer);
                onWinConditionMet?.Invoke();
            }
            onUnitDeath?.Invoke();
        }
    }
    
    private void OnMaxHealthChanged(float oldMaxHP, float newMaxHP)
    {
        this.OnUnitMaxHealthUpdate?.Invoke(this.unitCurrentHealth, newMaxHP);
    }

    private void OnAttackChanged(float oldAttack, float newAttack)
    {
        this.OnAttackUpdate?.Invoke(newAttack);
    }

    private void OnAbilityPowerChanged(float oldAP, float newAP)
    {
        this.OnAbilityPowerUpdate?.Invoke(newAP);
    }

    private void OnArmorChanged(float oldArmor, float newArmor)
    {
        this.OnArmorUpdate?.Invoke(newArmor);
    }

    private void OnMagicResistChanged(float oldMR, float newMR)
    {
        this.OnMagicResistUpdate?.Invoke(newMR);
    }

    private void OnMovementSpeedChanged(float oldMS, float newMS)
    {
        this.OnMovementSpeedUpdate?.Invoke(newMS);
    }

    private void OnAttackSpeedChanged(float oldAS, float newAS)
    {
        this.OnAttackSpeedUpdate?.Invoke(newAS);
    }

    private void OnCooldownReductionChanged(float oldCD, float newCD)
    {
        this.OnCooldownReductionUpdate?.Invoke(newCD);
    }
}
