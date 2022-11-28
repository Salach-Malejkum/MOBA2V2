using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerStats : UnitStats
{
    [SyncVar] [SerializeField] protected float playerExp = 0f;
    [SerializeField] private float playerHealthRegen = 2.5f;
    private float timer;
    [SyncVar] public string playerLane;
    [SyncVar] public string playerSide;

    [SyncVar][SerializeField] protected float playerGold = 100f;
    public float PlayerGold
    {
        get { return playerGold; }
        set { playerGold = value; }
    }

    [SyncVar][SerializeField] protected float playerResources = 3f;
    public float PlayerResources
    {
        get { return playerResources; }
        set { playerResources = value; }
    }

    public override void OnStartAuthority() {
        this.unitCurrentHealth = this.unitMaxHealth;
        this.timer = this.unitRegenerationIntervalSeconds;
        this.onUnitDeath += CmdReadyToRespawn;
    }

    [ServerCallback]
    private void Update() {
        this.timer -= Time.deltaTime;

        if (unitRegenerationIntervalSeconds <= 0) {
            base.AddHealth(playerHealthRegen);
            this.timer += this.unitRegenerationIntervalSeconds;
        }
    }

    //public void AddGold(int goldAmount)
    //{
    //    this.playerGold += goldAmount;
    //    Debug.Log("Gold: " + this.playerGold);
    //}

    public override void RemoveHealthOnNormalAttack(float damageAmount, GameObject aggressor)
    {
        base.RemoveHealthOnNormalAttack(damageAmount, aggressor);
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
