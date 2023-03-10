using Mirror;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class MinionMovement : NetworkBehaviour, IMinionMovement
{
    private int currentPointInd;
    private Vector3[] pathPoints;
    private readonly NavMeshAgent agent;

    public MinionMovement(Vector3[] pathPoints, NavMeshAgent agent, int layer)
    {
        this.pathPoints = pathPoints;
        this.agent = agent;
        this.agent.autoBraking = false;
        this.currentPointInd = 0;
    }

    public void Move()
    {
        if (!this.agent.pathPending && this.agent.remainingDistance < 0.5f)
        {
            this.GoToNextPoint();
        }
    }

    private void GoToNextPoint()
    {
        if (this.currentPointInd < this.pathPoints.Length - 1)
        {
            this.currentPointInd += 1;
            this.agent.destination = this.pathPoints[this.currentPointInd];
        }
    }

    public void SetPathPoints(Vector3[] path)
    {
        this.pathPoints = path;
    }
}
