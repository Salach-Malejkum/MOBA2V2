using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public abstract class UnitStats : NetworkBehaviour
{
    [SerializeField] protected float unitMaxHealth = 100f;
    [SyncVar(hook = nameof(OnHealthChanged))][SerializeField] protected float unitCurrentHealth = 0f;
    [SyncVar][SerializeField] protected float unitArmor = 0f;
    [SyncVar][SerializeField] protected float unitMagicResist = 0f;
    [SyncVar][SerializeField] protected float unitAttackDamage = 0f;
    public float UnitAttackDamage
    {
        get { return unitAttackDamage; }
    }
    [SyncVar][SerializeField] protected float attackSpeed = 1f;
    public float AttackSpeed
    {
        get { return attackSpeed; }
    }
    [SyncVar][SerializeField] protected float unitAbilityPower = 0f;
    [SyncVar][SerializeField] protected float unitMovementSpeed = 0f;
    [SyncVar][SerializeField] protected float unitCooldownReduction = 0f;
    [SerializeField] private bool IsWinCondition = false;
    protected int currentLevel = 1;
    protected float regenerationIntervalSeconds = 1f;

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
    
    public virtual void RemoveHealthOnNormalAttack(float hpAmount, GameObject aggresor)
    {
        this.unitCurrentHealth -= (hpAmount - this.unitArmor);
        this.OnDeathCheck();
    }

    public virtual void RemoveHealthOnMagicAttack(float hpAmount)
    {
        this.unitCurrentHealth -= (hpAmount - this.unitMagicResist);
        this.OnDeathCheck();
    }

    private void OnHealthChanged(float oldHP, float newHP) {
        this.OnDeathCheck();
    }

    protected void OnDeathCheck() {
        if (this.unitCurrentHealth <= 0) {
            Debug.Log("Unit death");
            if (this.IsWinCondition) {
                Debug.Log("End game");
                NetworkManagerLobby.Instance.whichSideLost = LayerMask.LayerToName(this.gameObject.layer);
                onWinConditionMet?.Invoke();
            }
            onUnitDeath?.Invoke();
        }
    }
}