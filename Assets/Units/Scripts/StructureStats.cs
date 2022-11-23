using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class StructureStats : UnitStats
{
    public override void OnStartServer() {
        this.unitCurrentHealth = this.unitMaxHealth;
        this.onUnitDeath += HandleStructureDestroyed;
    }

    public void TakeDamage(float damageAmount) {
        base.RemoveHealthOnNormalAttack(damageAmount);
    }

    [Server]
    private void HandleStructureDestroyed() {
        NetworkServer.Destroy(this.gameObject);
    }
}