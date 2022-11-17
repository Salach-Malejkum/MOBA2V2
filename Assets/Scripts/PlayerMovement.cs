using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : IMovement
{
    public Camera camera;
    private NavMeshAgent navMeshAgent;

    public PlayerMovement(NavMeshAgent navMeshAgent)
    {
        camera = Camera.main;
        this.navMeshAgent = navMeshAgent;
    }

    public void Move(Vector3 movementDestination)
    {
        this.navMeshAgent.destination = movementDestination;
    }
}
