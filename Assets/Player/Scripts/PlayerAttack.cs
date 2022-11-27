using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : NetworkBehaviour, IAttack
{
    private readonly int rayCastMaxDist = 100;
    private PlayerStats stats;
    private HashSet<GameObject> objectsInRangeHashSet;
    private LayerMask attackableLayer;
    private float aaCooldown;
    private float timeAfterAA = 0;
    private bool followAttack = false;
    private GameObject followAttackObject;

    void Start()
    {
        this.stats = GetComponent<PlayerStats>();
        this.objectsInRangeHashSet = new HashSet<GameObject>();
        this.AssignAttackableLayer();
    }
    void FixedUpdate()
    {
        this.FollowAttack();
    }

    public void Attack()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, this.rayCastMaxDist, this.attackableLayer, QueryTriggerInteraction.Ignore))
        {
            if (this.objectsInRangeHashSet.Contains(hit.transform.gameObject))
            {
                hit.transform.gameObject.GetComponent<UnitStats>().RemoveHealthOnNormalAttack(this.stats.UnitAttackDamage);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger || other.gameObject.layer == this.gameObject.layer)
            return;

        this.objectsInRangeHashSet.Add(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        this.objectsInRangeHashSet.Remove(other.gameObject);
    }

    private void AssignAttackableLayer()
    {
        LayerMask enemyTeamLayer = this.gameObject.layer;
        switch (enemyTeamLayer)
        {
            case Enums.Layers.blueTeamLayer:
                enemyTeamLayer = 1 << Enums.Layers.redTeamLayer;
                break;
            case Enums.Layers.redTeamLayer:
                enemyTeamLayer = 1 << Enums.Layers.blueTeamLayer;
                break;
        }
        LayerMask neutralLayer = 1 << Enums.Layers.neutral;
        this.attackableLayer = neutralLayer | enemyTeamLayer;
    }

    private void FollowAttack()
    {

    }
}
