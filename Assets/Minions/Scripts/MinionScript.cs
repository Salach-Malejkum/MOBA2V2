using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MinionScript : NetworkBehaviour
{
    public GameObject projectile;
    private IMinionMovement minionMovement;
    private MinionAttack minionAttack;
    private bool followAttack = false;
    private HashSet<GameObject> objectsInRangeHashSet;
    private NavMeshAgent navMeshAgent;
    private float timer = 0;
    private GameObject targetEnemy;

    // Start is called before the first frame update
    void Awake()
    {
        this.navMeshAgent = this.GetComponent<NavMeshAgent>();
        this.minionMovement = new MinionMovement(Enums.MinionPaths.topPathPoints, this.navMeshAgent, this.gameObject.layer);
        this.minionAttack = new MinionAttack();
        this.objectsInRangeHashSet = new HashSet<GameObject>();
    }

    [ServerCallback]
    void FixedUpdate()
    {
        this.timer += Time.fixedDeltaTime;

        GameObject? closestEnemy = this.GetTheClosestEnemy();
        if (closestEnemy == null)
        {
            this.followAttack = false;
            this.targetEnemy = null;
        }
        else if (this.targetEnemy == null)
        {
            this.followAttack = true;
            this.SetTargetEnemy(closestEnemy);
        }
        else if (!this.objectsInRangeHashSet.Contains(this.targetEnemy) && !this.targetEnemy.Equals(closestEnemy))
        {
            this.followAttack = true;
            this.SetTargetEnemy(closestEnemy);
        }

        if (this.followAttack)
        {
            // follow enemy
            int attackResult = this.minionAttack.CanAttack();

            if (attackResult == 1)
            {
                GameObject instProjectile = Instantiate(this.projectile, new Vector3(this.transform.position.x, this.transform.position.y + 0.4f, this.transform.position.z), Quaternion.identity);
                HomingMissileController missile = instProjectile.GetComponent<HomingMissileController>();
                missile.target = this.targetEnemy;
                missile.owner = this.gameObject;
                missile.damage = this.gameObject.GetComponent<UnitStats>().UnitAttackDamage;
            }
            navMeshAgent.destination = this.transform.position;
        }
        else
        {
            this.minionMovement.Move();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger)
            return;

        // dodac jakie tagi i layery wchodza w sklad tego
        switch (LayerMask.LayerToName(this.gameObject.layer))
        {
            case "Blue":
                if (LayerMask.LayerToName(other.gameObject.layer) == "Red")
                {
                    this.objectsInRangeHashSet.Add(other.gameObject);
                }
                break;
            case "Red":
                if (LayerMask.LayerToName(other.gameObject.layer) == "Blue")
                {
                    this.objectsInRangeHashSet.Add(other.gameObject);
                }
                break;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        this.objectsInRangeHashSet.Remove(other.gameObject);
    }

    private void SetTargetEnemy(GameObject gameObject)
    {
        this.targetEnemy = gameObject;
    }

    [ServerCallback]
    private GameObject GetTheClosestEnemy()
    {
        GameObject closestEnemy = null;

        foreach (GameObject currEnemy in this.objectsInRangeHashSet)
        {
            if (currEnemy != null && closestEnemy == null)
            {   closestEnemy = currEnemy;   }
            else if (currEnemy != null && Utility.GetDistanceBetweenGameObjects(currEnemy, this.gameObject) < Utility.GetDistanceBetweenGameObjects(closestEnemy, this.gameObject))
            {   closestEnemy = currEnemy;   }
        }
        return closestEnemy;
    }

    public void SetMinionPath(Vector3[] path)
    {
        this.minionMovement.SetPathPoints(path);
    }
}
