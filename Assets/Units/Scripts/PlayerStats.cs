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

    [SyncVar] public string playerName;

    private float timer;
    [SyncVar] public string lane;
    [SyncVar] public string side;

    public event Action<float> OnHealthRegenUptade;

    public override void OnStartAuthority() {
        this.unitCurrentHealth = this.unitMaxHealth;
        this.timer = this.regenerationIntervalSeconds;
        this.onUnitDeath += CmdReadyToRespawn;
        NetworkManagerLobby.Instance.PlayerSide = this.side;
        NetworkManagerLobby.Instance.PlayersLoadedToScene.Add(this.gameObject.GetComponent<NetworkIdentity>());
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
        this.gameObject.SetActive(false);
        RpcReadyToRespawn();
    }

    [ClientRpc]
    private void RpcReadyToRespawn() {
        this.gameObject.SetActive(false);
    }
    
    [Client]
    public void AddItemStatsToPlayer(ShopItemSo item)
    {
        this.CmdAddItemStatsToPlayer(item.Health, item.Attack, item.AbilityPower, item.Armor, item.MagicResist, item.CooldownReduction);
    }

    [Command]
    private void CmdAddItemStatsToPlayer(float health, float attack, float abilityPower, float armor, float magicResist, float cooldownReduction)
    {
        this.unitAttackDamage += attack;
        this.unitAbilityPower += abilityPower;
        this.unitArmor += armor;
        this.unitMagicResist += magicResist;
        this.unitMaxHealth += health;
        this.unitCooldownReduction += cooldownReduction;
    }

    [Client]
    public void SubtractItemStatsFromPlayer(ShopItemSo item)
    {
        this.CmdSubtractItemStatsFromPlayer(item.Health, item.Attack, item.AbilityPower, item.Armor, item.MagicResist, item.CooldownReduction);
    }

    [Command]
    private void CmdSubtractItemStatsFromPlayer(float health, float attack, float abilityPower, float armor, float magicResist, float cooldownReduction)
    {
        this.unitAttackDamage -= attack;
        this.unitAbilityPower -= abilityPower;
        this.unitArmor -= armor;
        this.unitMagicResist -= magicResist;
        this.unitMaxHealth -= health;
        this.unitCooldownReduction -= cooldownReduction;
    }
}
