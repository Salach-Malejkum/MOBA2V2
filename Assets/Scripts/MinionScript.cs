using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MinionScript : MonoBehaviour
{
    private IMinionMovement minionMovement;
    // Start is called before the first frame update
    void Start()
    {
        this.minionMovement = new MinionMovement(Enums.MinionPaths.topPathPoints, this.GetComponent<NavMeshAgent>());
    }

    // Update is called once per frame
    void Update()
    {
        this.minionMovement.Move();
    }
}
