using UnityEngine;
using UnityEngine.AI;

public class MobController : MonoBehaviour
{
    public GameObject target;
    public ResourceSpawner spawnerResource;
    public Transform spawner;
    private NavMeshAgent agent;
    private Outline outline;

    public float deleteOutlineTimer = 0f;

    public LayerMask whatIsGround, whatIsPlayer;

    private int id;

    private float hitPoints = 3f;
    public float maximumDistance = 15f;
    public bool isChasing = false;
    private Vector3 spawnPosition;

    public int Id
    {
        get { return id; }
        set
        {
            if (value < 0)
            {
                this.id = 0;
            }
            else
            {
                this.id = value;
            }
        }
    }

    private void Awake()
    {
        this.spawnPosition = this.transform.position;
        this.agent = GetComponent<NavMeshAgent>();
        this.outline = GetComponent<Outline>();
    }

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
                this.ChasePlayer();
            }
        }
        else if (this.isChasing == false && Vector3.Distance(this.spawnPosition, this.transform.position) > 0)
        {
            this.agent.SetDestination(this.spawnPosition);
        }
    }

    //private void OnMouseOver()
    //{
    //    this.deleteOutlineTimer = Time.time + componentDeleteDelay;

    //    if (!GetComponent<Outline>())
    //    {
    //        var outline = this.gameObject.AddComponent<Outline>();

    //        outline.OutlineMode = Outline.Mode.OutlineVisible;
    //        outline.OutlineColor = Color.red;
    //        outline.OutlineWidth = 3f;
    //    }
    //}

    public void TakeDamage(float damage, GameObject assaulter)
    {
        Debug.Log("Pozyskano surowce");
        this.target = assaulter;
        this.hitPoints -= damage;
        this.spawnerResource.NotifyAllChildren(this.target);
        if (this.hitPoints <= 0)
        {
            this.spawnerResource.RemoveFromChildren(this.Id);
            Destroy(this.gameObject);
        }
    }

    private void ChasePlayer()
    {
        agent.SetDestination(this.target.transform.position);
    }
}
