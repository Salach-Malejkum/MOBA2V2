using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MobStats : UnitStats
{
    public override void OnStartAuthority() {
        this.unitCurrentHealth = this.unitMaxHealth;
        this.onUnitDeath += HandleMobDeath;
    }

    public void onMonsterLoseAggro() {
        base.AddHealth(this.unitMaxHealth);
    }

    [Server]
    private void HandleMobDeath() {
        NetworkServer.Destroy(this.gameObject);
    }
}
