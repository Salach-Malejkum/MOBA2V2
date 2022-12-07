using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.Linq;

public class StructureStats : UnitStats
{
    [SyncVar][SerializeField] protected List<StructureUpgradeSo> activeUpgrades = new();
    [SyncVar][SerializeField] protected List<StructureUpgradeSo> upgrades = new();

    public override void OnStartServer() {
        this.unitCurrentHealth = this.unitMaxHealth;
        this.onUnitDeath += HandleStructureDestroyed;
    }

    public void TakeDamage(float damageAmount, GameObject agressor) {
        base.RemoveHealthOnNormalAttack(damageAmount, agressor);
    }

    [Server]
    public void ActivateUpgrade(int index)
    {
        StructureUpgradeSo upgrade = upgrades.ElementAt(index);
        this.activeUpgrades.Add(upgrade);
        this.ModifyStats(upgrade);
    }

    [Server]
    private void HandleStructureDestroyed() {
        NetworkServer.Destroy(this.gameObject);
    }

    [Server]
    public virtual void ModifyStats(StructureUpgradeSo structureUpgradeSo)
    {
        if (this.gameObject == null) { return; }

        this.unitMaxHealth += structureUpgradeSo.HealthModifier;
        this.unitCurrentHealth += structureUpgradeSo.HealthModifier;
        this.unitArmor += structureUpgradeSo.ArmorModifier;
        this.unitMagicResist += structureUpgradeSo.MagicResistModifier;
        this.unitAttackDamage += structureUpgradeSo.AttackDamageModifier;
        this.unitMovementSpeed += structureUpgradeSo.MovementSpeedModifier;
    }
}
