using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionAttack
{
    private float aaCooldown;
    private float timeAfterAA = 0;
    private float timer = 0;

    public int Attack(GameObject enemy)
    {
        if (enemy.Equals(null))
            return -2;
        return 0;
    }
}
