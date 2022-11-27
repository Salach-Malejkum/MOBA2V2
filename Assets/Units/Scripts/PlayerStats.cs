using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerStats : UnitStats
{
    [SerializeField] private float playerHealthRegen = 2.5f;
    private float timer;

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

    public void TakeDamage(float damageAmount, GameObject agressor) {
        base.RemoveHealthOnNormalAttack(damageAmount, agressor);
    }

    [ClientRpc]
    private void RpcHandlePlayerDeath() {
        this.gameObject.SetActive(false);
    }
}
