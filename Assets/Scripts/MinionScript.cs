using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MinionScript : MonoBehaviour
{
    private IMinionMovement minionMovement;
    private MinionAttack minionAttack;
    private bool followAttack = false;
    private HashSet<GameObject> objectsInRangeHashSet;

    // Start is called before the first frame update
    void Start()
    {
        this.minionMovement = new MinionMovement(Enums.MinionPaths.topPathPoints, this.GetComponent<NavMeshAgent>());
        this.minionAttack = new MinionAttack();
        this.objectsInRangeHashSet = new HashSet<GameObject>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        this.minionMovement.Move();
        this.minionAttack.Attack(this.GetTheClosestEnemy());
    }

    // zamiast tego moze byc lista obiektow, aktualizowana na onenter i onexit,
    // mozna od razu dodawac tylko te, ktore spelniaja okreslony warunek

    private void OnTriggerEnter(Collider other)
    {
        if (true) // dodac jakie tagi i layery wchodza w sklad tego
        {
            this.objectsInRangeHashSet.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        this.objectsInRangeHashSet.Remove(other.gameObject);
    }

    private GameObject GetTheClosestEnemy()
    {
        if (this.objectsInRangeHashSet.Count > 0)
        {
            return null;
        }

        GameObject resGameObject = null;

        foreach (GameObject enemy in this.objectsInRangeHashSet)
        {
            if (resGameObject.Equals(null))
            {
                resGameObject = enemy;
            }
            else if (
                Utility.GetDistanceBetweenGameObjects(enemy, this.gameObject) < Utility.GetDistanceBetweenGameObjects(resGameObject, this.gameObject)
                )
            {
                resGameObject = enemy;
            }
        }

        return resGameObject;
    }
}
