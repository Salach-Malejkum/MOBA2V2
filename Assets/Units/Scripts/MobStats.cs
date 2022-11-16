using System;

using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MobStats : UnitStats
{
    //TODO: na spawn minona musi być event Action który robi Invoke 
    public event Action OnMobSpawn;

    public override void OnStartServer()
    {
        base.OnStartServer();
        this.OnMobSpawn += OnMobSpawnEvent;
    }

    public override void OnStopServer()
    {
        base.OnStopServer();
        this.OnMobSpawn -= OnMobSpawnEvent;
    }

    public void OnMobSpawnEvent() {
        this.unitCurrentHealth = this.unitMaxHealth;
        this.onUnitDeath += HandleMobDeath;
    }

    public void onMonsterLoseAggro() {
        base.AddHealth(this.unitMaxHealth);
    }

    public void TakeDamage(float damageAmount) {
        base.RemoveHealthOnNormalAttack(damageAmount);
    }
    
    [Server]
    private void HandleMobDeath() {
        NetworkServer.Destroy(this.gameObject);
    }
}
