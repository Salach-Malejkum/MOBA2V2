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
        this.onUnitDeath -= HandleMobDeath;
    }

    public void OnMobSpawnEvent() {
        this.unitCurrentHealth = this.unitMaxHealth;
        this.onUnitDeath += HandleMobDeath;
    }

    public void OnMobSpawned()
    {
        OnMobSpawn?.Invoke();
    }

    public void TakeDamage(float damageAmount, GameObject aggresor) {
        base.RemoveHealthOnNormalAttack(damageAmount, aggresor);
    }
    
    [Server]
    protected virtual void HandleMobDeath()
    {
        if (this.lastAggressor.tag == "Player")
        {
            PlayerStats stats = this.lastAggressor.GetComponent<PlayerStats>();
            stats.PlayerGold += this.goldOnDeath;
            //this.lastAggressor.GetComponent<PlayerStats>().AddGold(this.goldOnDeath);
        }
        NetworkServer.Destroy(this.gameObject);
    }
}
