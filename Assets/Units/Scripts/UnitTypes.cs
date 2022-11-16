using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitTypes : MonoBehaviour
{
    [Header("Unit Type")]
    public UnitType unitType = UnitType.Minion;

    public enum UnitType
    {
        Minion,
        Turret,
        Player,
    }
}
