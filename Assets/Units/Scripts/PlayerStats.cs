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
    private float timer;
    [SyncVar] public string lane;
    [SyncVar] public string side;

    public event Action<float> OnHealthRegenUptade;

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
    
    //[Client]
    public void AddItemStatsToPlayer(ShopItemSo item)
    {
        this.unitMaxHealth += item.Health;
        this.unitArmor += item.Armor;
        this.unitMagicResist += item.MagicResist;
        this.unitAttackDamage += item.Attack;
        this.unitAbilityPower += item.AbilityPower;
        this.unitCooldownReduction += item.CooldownReduction;
    }

    //[Client]
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
