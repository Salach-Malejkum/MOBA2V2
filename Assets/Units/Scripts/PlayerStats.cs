using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerStats : UnitStats
{
    [SerializeField] private float playerHealthRegen = 2.5f;
    private float timer;
    [SyncVar] public string playerLane;
    [SyncVar] public string playerSide;
    [SerializeField] protected float playerGold;
    public float PlayerGold
    {
        get { return playerGold; }
        set { playerGold = value; }
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

    public void AddGold(int goldAmount)
    {
        this.playerGold += goldAmount;
        Debug.Log("Gold: " + this.playerGold);
    }

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
}
