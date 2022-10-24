using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MobController : MonoBehaviour
{
    public GameObject target;
    public ResourceSpawner spawnerResource;
    public Transform spawner;
    public NavMeshAgent agent;

    public LayerMask whatIsGround, whatIsPlayer;

    private readonly float componentDeleteDelay = 1f;
    private float deleteTime = 0f;

    private int hitPoints = 3;
    public float maximumDistance = 15f;
    public bool isChasing = false;

    private void Awake()
    {
        target = GameObject.Find("Cube");
        agent = GetComponent<NavMeshAgent>();
    }

    private void FixedUpdate()
    {
        if (Time.time > this.deleteTime)    Destroy(GetComponent<Outline>());

        if (this.isChasing == true)
        {
            if (Vector3.Distance(spawner.position, target.transform.position) > maximumDistance)
                this.isChasing = false;
            else
                ChasePlayer();
        }
        else if (isChasing == false && Vector3.Distance(spawner.position, this.transform.position) > 0)
            agent.SetDestination(spawner.position);
    }

    private void OnMouseOver()
    {
        this.deleteTime = Time.time + componentDeleteDelay;

        if (!GetComponent<Outline>())
        {
            var outline = gameObject.AddComponent<Outline>();

            outline.OutlineMode = Outline.Mode.OutlineVisible;
            outline.OutlineColor = Color.red;
            outline.OutlineWidth = 5f;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Pozyskano Unity-chan");
            this.hitPoints -= 1;
            isChasing = true;
            if (this.hitPoints <= 0)
            {
                this.spawnerResource.StartSpawner();
                Destroy(this.gameObject);
            }
        }
    }

    private void ChasePlayer()
    {
        agent.SetDestination(target.transform.position);
    }
}
