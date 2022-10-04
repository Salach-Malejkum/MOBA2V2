using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPlayerScript : IAttack
{
    private float range;
    private float aaCooldown;
    private float timeAfterAA = 0;
    public AttackPlayerScript(float range, float aaCooldown)
    {
        this.range = range;
        this.aaCooldown = aaCooldown;
    }
    public void Attack(Vector3 currentPosition)
    {
        if (Input.GetMouseButton(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100))
            {
                if (!this.IsAttackOnCooldown() && this.IsInRange(currentPosition, hit))
                {
                    // deal dmg
                    //TODO warunek jakie GameObject'y moga byc atakowane
                    Debug.Log(hit.transform.gameObject.name);
                }
                Debug.Log("Nie mozesz atakowac");
            }
        }
    }

    private bool IsInRange(Vector3 currentPosition, RaycastHit hit)
    {
        return range >= Vector3.Distance(currentPosition, hit.transform.position);
    }

    private bool IsAttackOnCooldown()
    {
        bool isOnCd = this.timeAfterAA < this.aaCooldown;
        this.AdjustTimeAfterAA(isOnCd);
        return isOnCd;
    }

    private void AdjustTimeAfterAA(bool isOnCd)
    {
        timeAfterAA = isOnCd ? timeAfterAA + Time.deltaTime : 0;
    }

    // zadanie obrazen - done +/-
    // rzucenie jakiegos projectile, ktory tylko cel moze trafic
    // otwartosc na blockowanie projectile
    // jezeli poza zasiegiem to idz do celu, sprawdzenie czy nie za szybko (aa cd)
    // sprawdzenie czy w zasiegu - done
    // aa cooldown - done
    // nie rusza sie do czasu wypuszczenia projectile albo zaatakowania
}
