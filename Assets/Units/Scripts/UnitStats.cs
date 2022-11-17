using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

//TODO: [DAR-182] Poprawić statystyki na synchronizację sieciową
public abstract class UnitStats : NetworkBehaviour
{
    [SerializeField] protected float unitMaxHealth = 100f;
    [SyncVar] protected float unitCurrentHealth = 0f;
    [SyncVar] [SerializeField] protected float unitArmor = 0f;
    [SyncVar] [SerializeField] protected float unitMagicResist = 0f;
    [SyncVar] [SerializeField] protected float unitAttackDamage = 0f;
    [SyncVar] [SerializeField] protected float unitAbilityPower = 0f;
    [SyncVar] [SerializeField] protected float unitMovementSpeed = 0f;
    [SyncVar] [SerializeField] protected float unitCooldownReduction = 0f;
    protected int currentLevel = 1;
    protected float regenerationIntervalSeconds = 1f;

    public event Action onUnitDeath;

    [Server]
    public virtual void AddHealth(float hpAmount) {
        this.unitCurrentHealth += hpAmount;
        if (this.unitCurrentHealth > this.unitMaxHealth) {
            this.unitCurrentHealth = this.unitMaxHealth;
        }
    }

    [Server]
    public virtual void RemoveHealthOnNormalAttack(float hpAmount) {
        this.unitCurrentHealth -= (hpAmount - this.unitArmor);

        if (this.unitCurrentHealth <= 0) {
            onUnitDeath?.Invoke();
        }
    }

    [Server]
    public virtual void RemoveHealthOnMagicAttack(float hpAmount) {
        this.unitCurrentHealth -= (hpAmount - this.unitMagicResist);

        if (this.unitCurrentHealth <= 0) {
            onUnitDeath?.Invoke();
        }
    }
}
