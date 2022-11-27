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

    public void TakeDamage(float damageAmount, GameObject agressor) {
        base.RemoveHealthOnNormalAttack(damageAmount, agressor);
    }

    [Server]
    private void HandleStructureDestroyed() {
        NetworkServer.Destroy(this.gameObject);
    }
}
