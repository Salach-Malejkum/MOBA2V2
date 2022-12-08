using Mirror;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class MobController : NetworkBehaviour, IOutlinable
{
    public float range;

    public GameObject target;
    public ResourceSpawner spawnerResource;
    public Transform spawner;

    private NavMeshAgent agent;
    private Outline outline;
    private Animator animator;
    private NetworkAnimator networkAnimator;
    private UnitStats stats;

    public float maximumDistance = 15f; // Erwin czy to nie lepiej wyciagnac dla kazdego moba gdzies jako stala? nawet tu czy cos | OK
    public bool isChasing = false;
    private Vector3 spawnPosition;

    private void Awake()
    {
        this.spawnPosition = this.transform.position;
        this.agent = GetComponent<NavMeshAgent>();
        this.outline = GetComponent<Outline>();
        this.animator = GetComponent<Animator>();
        this.networkAnimator = GetComponent<NetworkAnimator>();
        this.stats = this.gameObject.GetComponent<UnitStats>();
    }

    [ServerCallback]
    private void Update()
    {
        this.RemovePlayerWhenNotActive();

        if (this.isChasing == true)
        {
            if (Vector3.Distance(this.spawnPosition, this.target.transform.position) > this.maximumDistance)
            {
                this.isChasing = false;
            }
            else if (Vector3.Distance(this.gameObject.transform.position, this.target.transform.position) <= this.range)
            {
                this.networkAnimator.SetTrigger("Attack");
                this.animator.speed = this.stats.UnitAttackSpeed;
                this.transform.LookAt(this.target.transform);
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

        this.animator.SetBool("IsWalking", this.IsMoving());

        this.StopMovingForRootedAnimations();
    }

    [ServerCallback]
    public void Attack()
    {
        this.target.GetComponent<UnitStats>().RemoveHealthOnNormalAttack(this.stats.UnitAttackDamage, this.gameObject);
    }

    [ServerCallback]
    private bool IsMoving()
    {
        return this.agent.velocity.magnitude > 0.2f;
    }

    [ServerCallback]
    private void StopMovingForRootedAnimations()
    {
        string animName = this.animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
        this.agent.isStopped = animName switch
        {
            "Bear_Attack1" or "Bear_StunnedLoop"
                or "Eat" or "Turn Head"
                or "Fox_Attack_Tail" or "Fox_Attack_Paws"
                or "Jump W Root" or "Smash Attack"
                or "Take Damage" or "Attack 01" => true,
            _ => false,
        };
    }

    [ClientCallback]
    public IEnumerator DeleteOutline(float timeDelay)
    {
        yield return new WaitForSeconds(timeDelay);
        this.outline.OutlineWidth = 0f;
    }

    [ServerCallback]
    public void ChasePlayer(GameObject assaulter)
    {
        this.target = assaulter;
        this.spawnerResource.NotifyAllChildren(this.target);
    }

    [ServerCallback]
    public void SetupSpawner(ResourceSpawner spawner)
    {
        this.spawner = spawner.transform;
        this.spawnerResource = spawner;

        this.range = (this.agent.radius * this.spawnerResource.scale) + 3f;
        Debug.Log(this.range);
    }

    private void SetTargetPosition()
    {
        this.agent.SetDestination(this.target.transform.position);
    }

    private void RemovePlayerWhenNotActive()
    {
        if (this.target != null && !this.target.activeSelf)
        {
            this.target = null;
            this.isChasing = false;
        }
    }
}
