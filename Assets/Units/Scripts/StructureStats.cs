using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class StructureStats : UnitStats
{
    public override void OnStartAuthority() {
        this.unitCurrentHealth = this.unitMaxHealth;
        this.onUnitDeath += HandleStructureDestroyed;
    }

    [Server]
    private void HandleStructureDestroyed() {
        NetworkServer.Destroy(this.gameObject);
    }
}
