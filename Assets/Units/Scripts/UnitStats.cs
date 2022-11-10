using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

//TODO: [DAR-181] Napisać klasy dla EventHandler
//TODO: [DAR-182] Poprawić statystyki na synchronizację sieciową
public class UnitStats : MonoBehaviour
{
    [SerializeField] private float unitMaxHealth = 100f;
    private float unitCurrentHealth = 0f;
    [SerializeField] private float unitArmor = 0f;
    [SerializeField] private float unitMagicResist = 0f;
    [SerializeField] private float unitAttackDamage = 0f;
    [SerializeField] private float unitAbilityPower = 0f;
    [SerializeField] private float unitMovementSpeed = 0f;
    [SerializeField] private float unitCooldownReduction = 0f;

    //public static event EventHandler<DeathEventArgs> onDeath;
    //public event EventHandler<HealthEventArgs> onHealthChanged;
    //public event EventHandler<BuyEventArgs> onBuyItem;
    //public event EventHandler<SellEventArgs> onSellItem;

    public void Heal(float hpAmount) {
        this.unitCurrentHealth += hpAmount;
        if (this.unitCurrentHealth > this.unitMaxHealth) {
            //onHealthChanged?.Invoke(this, ...)
            this.unitCurrentHealth = this.unitMaxHealth;
        }
    }

    public void TakeDamage(float hpAmount) {
        this.unitCurrentHealth -= hpAmount;
        if(this.unitCurrentHealth <= 0) {
            //onDeath?.Invoke(this, ...)
            switch (this.GetComponent<UnitTypes>().unitType) {
                case UnitTypes.UnitType.Player:
                    this.gameObject.SetActive(false);
                    break;
                default:
                    Destroy(this.gameObject);
                    break;
            }
        }
    }

    //TODO: Poprawić oba po merge sklepu
    public void AddItemAttributes(ItemPlaceholder item) {
        //onBuyItem?.Invoke(this, ...)
        this.unitMaxHealth += item.health;
        this.unitArmor += item.armor;
        this.unitAbilityPower += item.abilityPower;
        this.unitAttackDamage += item.attack;
        this.unitMagicResist += item.magicResist;
        this.unitCooldownReduction += item.cooldownReduction;
    }
    
    public void RemoveItemAttributes(ItemPlaceholder item) {
        //onSellItem?.Invoke(this, ...)
        this.unitMaxHealth -= item.health;
        this.unitArmor -= item.armor;
        this.unitAbilityPower -= item.abilityPower;
        this.unitAttackDamage -= item.attack;
        this.unitMagicResist -= item.magicResist;
        this.unitCooldownReduction -= item.cooldownReduction;
    }
}
