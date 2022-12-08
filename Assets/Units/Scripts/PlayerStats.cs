using UnityEngine;
using Mirror;
using System;
using UnityEngine.AI;

public class PlayerStats : UnitStats
{
    private float timer;
    [SyncVar] public string playerLane;
    [SyncVar] public string playerSide;
    [SyncVar] public string playerName;

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
    
    [SyncVar] [SerializeField] protected float playerExp = 0f;
    [SyncVar(hook = nameof(OnHealthRegenChanged))] [SerializeField] private float playerHealthRegen = 2.5f;
    public float PlayerHealthRegen
    {
        get { return playerHealthRegen; }
    }

    public event Action<float> OnHealthRegenUpdate;

    public override void OnStartAuthority()
    {
        this.timer = this.unitRegenerationIntervalSeconds;
        this.onUnitDeath += CmdReadyToRespawn;
        NetworkManagerLobby.Instance.PlayerSide = this.playerSide;
        NetworkManagerLobby.Instance.PlayersLoadedToScene.Add(this.gameObject.GetComponent<NetworkIdentity>());
    }

    public override void OnStartServer() {
        this.unitCurrentHealth = this.unitMaxHealth;
    }

    [ServerCallback]
    private void Update() {
        this.timer -= Time.deltaTime;

        if (unitRegenerationIntervalSeconds <= 0) {
            base.AddHealth(playerHealthRegen);
            this.timer += this.unitRegenerationIntervalSeconds;
        }
    }

    public override void RemoveHealthOnNormalAttack(float damageAmount, GameObject aggressor)
    {
        base.RemoveHealthOnNormalAttack(damageAmount, aggressor);
    }

    private void OnHealthRegenChanged(float oldHPRegen, float newHPRegen)
    {
        OnHealthRegenUpdate?.Invoke(newHPRegen);
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

    [Server]
    public void OnRespawnHeal() {
        this.unitCurrentHealth = this.unitMaxHealth;
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

    public void UsedESkill()
    {
        GetComponent<NavMeshAgent>().acceleration += 200;
        GetComponent<NavMeshAgent>().speed += 10;

        Invoke("RevertESkill", 1.2f);
    }

    public void RevertESkill()
    {
        GetComponent<NavMeshAgent>().acceleration -= 200;
        GetComponent<NavMeshAgent>().speed -= 10;
    }
}
