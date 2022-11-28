using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerStats : UnitStats
{
    [SerializeField] private float playerHealthRegen = 2.5f;
    private float timer;
    [SyncVar] public string lane;
    [SyncVar] public string side;

    public override void OnStartAuthority() {
        this.unitCurrentHealth = this.unitMaxHealth;
        this.timer = this.regenerationIntervalSeconds;
        this.onUnitDeath += CmdReadyToRespawn;
        NetworkManagerLobby.Instance.PlayerSide = this.side;
    }

    public override void RemoveHealthOnNormalAttack(float damageAmount, GameObject agressor) {
        base.RemoveHealthOnNormalAttack(damageAmount, agressor);
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
}
