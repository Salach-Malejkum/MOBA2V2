using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MinionAttack
{
    private float aaCooldown = 0.5f;
    private float timeAfterAA = 3f;
    private float timer = -1; 

    public int CanAttack()
    {
        if (!this.IsAttackOnCooldown())
        {
            return 1;
        }
        return 0;
    }

    private bool IsAttackOnCooldown()
    {
        this.UpdateTimer();
        bool isOnCd = this.timeAfterAA < this.aaCooldown;
        if (!isOnCd)
        {
            this.timeAfterAA = 0;
        }
        return isOnCd;
    }

    private void UpdateTimer()
    {
        if (this.timer > 0)
        {
            float deltaTime = Time.time - this.timer;
            this.timeAfterAA += deltaTime;
        }
        this.timer = Time.time;
    }
}
