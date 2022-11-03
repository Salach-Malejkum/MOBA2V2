using UnityEngine;
using UnityEngine.AI;

public class MobController : MonoBehaviour
{
    public GameObject target;
    public ResourceSpawner spawnerResource;
    public Transform spawner;
    private NavMeshAgent agent;

    public LayerMask whatIsGround, whatIsPlayer;

    private int id;
    private readonly float componentDeleteDelay = .5f;
    private float deleteTime = 0f;

    private int hitPoints = 3;
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
        //target = GameObject.Find("Cube");
        this.spawnPosition = this.transform.position;
        this.agent = GetComponent<NavMeshAgent>();
    }

    private void FixedUpdate()
    {
        if (Time.time > this.deleteTime)
        {
            Destroy(GetComponent<Outline>());
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
            this.agent.SetDestination(spawnPosition);
        }
    }

    private void OnMouseOver()
    {
        this.deleteTime = Time.time + componentDeleteDelay;

        if (!GetComponent<Outline>())
        {
            var outline = this.gameObject.AddComponent<Outline>();

            outline.OutlineMode = Outline.Mode.OutlineVisible;
            outline.OutlineColor = Color.red;
            outline.OutlineWidth = 3f;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Pozyskano surowce");
            this.hitPoints -= 1;
            this.spawnerResource.NotifyAllChildren(target);
            if (this.hitPoints <= 0)
            {
                this.spawnerResource.RemoveFromChildren(this.Id);
                Destroy(this.gameObject);
            }
        }
    }

    private void ChasePlayer()
    {
        agent.SetDestination(this.target.transform.position);
    }
}
