using Mirror;
using UnityEngine;
using UnityEngine.AI;

public class MobController : NetworkBehaviour
{
    public GameObject target;
    public ResourceSpawner spawnerResource;
    public Transform spawner;
    private NavMeshAgent agent;
    private Outline outline;

    public float deleteOutlineTimer = 0f;

    public LayerMask whatIsGround, whatIsPlayer;

    public float maximumDistance = 15f; // Erwin czy to nie lepiej wyciagnac dla kazdego moba gdzies jako stala? nawet tu czy cos
    public bool isChasing = false;
    private Vector3 spawnPosition;

    private void Awake()
    {
        this.spawnPosition = this.transform.position;
        this.agent = GetComponent<NavMeshAgent>();
        this.outline = GetComponent<Outline>();
    }

    [ServerCallback]
    private void FixedUpdate()
    {
        if (this.outline.OutlineWidth > 0 && Time.time > this.deleteOutlineTimer)
        {
            this.outline.OutlineWidth = 0f;
        }
        if (this.isChasing == true)
        {
            if (Vector3.Distance(this.spawnPosition, this.target.transform.position) > this.maximumDistance)
            {
                this.isChasing = false;
            }
            else
            {
                this.SetTargetPosition();
            }
        }
        else if (this.isChasing == false && Vector3.Distance(this.spawnPosition, this.transform.position) > 0)
        {
            this.agent.SetDestination(this.spawnPosition);
        }
    }

    [ServerCallback]
    public void ChasePlayer(GameObject assaulter)
    {
        this.target = assaulter;
        this.spawnerResource.NotifyAllChildren(this.target);
    }

    private void SetTargetPosition()
    {
        agent.SetDestination(this.target.transform.position);
    }
}
