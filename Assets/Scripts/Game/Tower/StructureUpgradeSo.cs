using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "StructureUpgrade", menuName = "Scriptable Objects/New Structure Upgrade", order = 3)]
public class StructureUpgradeSo : ScriptableObject
{
    [SerializeField]
    private int structureUpgradeId;
    public int StructureUpgradeId
    {
        get { return structureUpgradeId; }
    }

    [SerializeField]
    private float healthModifier;
    public float HealthModifier
    {
        get { return healthModifier; }
    }

    [SerializeField]
    private float armorModifier;
    public float ArmorModifier
    {
        get { return armorModifier; }
    }

    [SerializeField]
    private float magicResistModifier;
    public float MagicResistModifier
    {
        get { return magicResistModifier; }
    }

    [SerializeField]
    private float attackDamageModifier;
    public float AttackDamageModifier
    {
        get { return attackDamageModifier; }
    }

    [SerializeField]
    private float attackSpeedModifier;
    public float AttackSpeedModifier
    {
        get { return attackSpeedModifier; }
    }

    [SerializeField]
    private float movementSpeedModifier;
    public float MovementSpeedModifier
    {
        get { return movementSpeedModifier; }
    }
}
