using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStats : MobStats
{
    public ResourceSpawner spawnerResource;
    public int Id;
    // resources

    [Server]
    protected override void HandleMobDeath()
    {
        this.spawnerResource.RemoveFromChildren(this.Id);
        base.HandleMobDeath();
    }
}
