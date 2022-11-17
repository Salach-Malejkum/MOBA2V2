using UnityEngine;
using UnityEngine.AI;

public class MinionMovement : IMinionMovement
{
    private int currentPointInd;
    private readonly Vector3[] pathPoints;
    private readonly NavMeshAgent agent;

    public MinionMovement(Vector3[] pathPoints, NavMeshAgent agent)
    {
        this.pathPoints = pathPoints;
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
