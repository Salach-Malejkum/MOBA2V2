using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MinionScript : MonoBehaviour
{
    public GameObject projectile;
    private IMinionMovement minionMovement;
    private MinionAttack minionAttack;
    private bool followAttack = false;
    [SerializeField]
    private HashSet<GameObject> objectsInRangeHashSet;
    private NavMeshAgent navMeshAgent;
    private float timer = 0;
    [SerializeField]
    private GameObject targetEnemy;

    // Start is called before the first frame update
    void Start()
    {
        this.navMeshAgent = this.GetComponent<NavMeshAgent>();
        this.minionMovement = new MinionMovement(Enums.MinionPaths.topPathPoints, this.navMeshAgent, this.gameObject.tag);
        this.minionAttack = new MinionAttack();
        this.objectsInRangeHashSet = new HashSet<GameObject>();
    }

    // Update is called once per frame
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
            int attackResult = this.minionAttack.Attack(this.targetEnemy);

            if (attackResult == 1)
            {
                GameObject instProjectile = (GameObject)Instantiate(this.projectile, this.transform);
                instProjectile.GetComponent<HomingMissile>().target = this.targetEnemy;
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
        switch (this.gameObject.tag)
        {
            case "Blue_Team":
                if (other.tag == "Red_Team")
                {
                    this.objectsInRangeHashSet.Add(other.gameObject);
                }
                break;
            case "Red_Team":
                if (other.tag == "Blue_Team")
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

    private GameObject GetTheClosestEnemy()
    {
        GameObject closestEnemy = null;

        foreach (GameObject currEnemy in this.objectsInRangeHashSet)
        {
            if (closestEnemy == null)
            {   closestEnemy = currEnemy;   }
            else if (Utility.GetDistanceBetweenGameObjects(currEnemy, this.gameObject) < Utility.GetDistanceBetweenGameObjects(closestEnemy, this.gameObject))
            {   closestEnemy = currEnemy;   }
        }
        return closestEnemy;
    }
}
