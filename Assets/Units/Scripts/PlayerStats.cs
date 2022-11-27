using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerStats : UnitStats
{
    [SerializeField] protected float playerGold = 100f;
    public float PlayerGold
    {
        get { return playerGold; }
        set { playerGold = value; }
    }
    [SyncVar] [SerializeField] protected float playerExp = 0f;
    [SerializeField] private float playerHealthRegen = 2.5f;
    private float timer;
    [SyncVar] public string lane;
    [SyncVar] public string side;

    public override void OnStartAuthority() {
        this.unitCurrentHealth = this.unitMaxHealth;
        this.onUnitDeath += RpcHandlePlayerDeath;
        this.timer = this.regenerationIntervalSeconds;
    }

    [ServerCallback]
    private void Update() {
        this.timer -= Time.deltaTime;

        if (regenerationIntervalSeconds <= 0) {
            base.AddHealth(playerHealthRegen);
            this.timer += this.regenerationIntervalSeconds;
        }
    }

    public void TakeDamage(float damageAmount) {
        base.RemoveHealthOnNormalAttack(damageAmount);
    }

    [ClientRpc]
    private void RpcHandlePlayerDeath() {
        this.gameObject.SetActive(false);
    }

    public void AddItemStatsToPlayer(ShopItemSo item)
    {
        this.unitMaxHealth += item.Health;
        this.unitArmor += item.Armor;
        this.unitMagicResist += item.MagicResist;
        this.unitAttackDamage += item.Attack;
        this.unitAbilityPower += item.AbilityPower;
        this.unitCooldownReduction += item.CooldownReduction;
    }

    public void SubtractItemStatsFromPlayer(ShopItemSo item)
    {
        this.unitMaxHealth -= item.Health;

        if (this.unitMaxHealth > this.unitCurrentHealth)
        {
            this.unitCurrentHealth = this.unitMaxHealth;
        }

        this.unitArmor -= item.Armor;
        this.unitMagicResist -= item.MagicResist;
        this.unitAttackDamage -= item.Attack;
        this.unitAbilityPower -= item.AbilityPower;
        this.unitCooldownReduction -= item.CooldownReduction;
    }
}
