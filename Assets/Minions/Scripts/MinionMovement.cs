using System;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class MinionMovement : IMinionMovement
{
    private int currentPointInd;
    private readonly Vector3[] pathPoints;
    private readonly NavMeshAgent agent;

    public MinionMovement(Vector3[] pathPoints, NavMeshAgent agent, string tagName)
    {
        switch (tagName)
        {
            case Enums.TeamMinionSpawnerTags.blueTeamTag:
                this.pathPoints = pathPoints;
                break;
            case Enums.TeamMinionSpawnerTags.redTeamTag:
                this.pathPoints = Enumerable.Reverse(pathPoints).ToArray();
                break ;
        }
        this.agent = agent;
        this.agent.autoBraking = false;
        this.currentPointInd = 0;
    }

    public void Move()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            this.GoToNextPoint();
        }
    }

    private void GoToNextPoint()
    {
        if (this.currentPointInd < this.pathPoints.Length - 1)
        {
            this.currentPointInd += 1;
            agent.destination = this.pathPoints[this.currentPointInd];
        }
    }
}
