using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : IMovement
{
    private NavMeshAgent navMeshAgent;

    public PlayerMovement(NavMeshAgent navMeshAgent)
    {
        this.navMeshAgent = navMeshAgent;
    }

    public void Move(Vector3 movementDestination)
    {
        this.navMeshAgent.destination = movementDestination;
    }
}
