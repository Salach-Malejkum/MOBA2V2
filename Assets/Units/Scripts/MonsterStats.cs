using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;

public class MonsterStats : MobStats
{
    public ResourceSpawner spawnerResource;
    public int Id;

    [Server]
    protected override void HandleMobDeath()
    {
        this.spawnerResource.RemoveFromChildren(this.Id);
        base.HandleMobDeath();
    }

    public override void RemoveHealthOnNormalAttack(float hpAmount, GameObject agressor)
    {
        this.spawnerResource.NotifyAllChildren(agressor);
        base.RemoveHealthOnNormalAttack(hpAmount, agressor);
    }
}
