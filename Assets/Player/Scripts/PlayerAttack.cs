using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.HID;

public class PlayerAttack : NetworkBehaviour, IAttack
{
    public GameObject projectile;
    private readonly int rayCastMaxDist = 100;
    private PlayerStats stats;
    private HashSet<GameObject> objectsInRangeHashSet;
    [SerializeField]
    private LayerMask attackableLayer;
    public LayerMask AttackableLayer
    {
        get { return attackableLayer; }
    }
    private bool followAttack = false;
    private GameObject targetEnemy;
    private Animator animator;
    private NetworkAnimator networkAnimator;
    private PlayerMovement playerMovement;

    void Start()
    {
        this.stats = GetComponent<PlayerStats>();
        this.objectsInRangeHashSet = new HashSet<GameObject>();
        this.attackableLayer = new LayerMask();
        this.AssignAttackableLayer();
        this.animator = GetComponent<Animator>();
        this.networkAnimator = GetComponent<NetworkAnimator>();
        this.playerMovement = GetComponent<PlayerMovement>();
    }

    [ClientCallback]
    void FixedUpdate()
    {
        this.FollowAttack();
    }

    [ClientCallback]
    public void AttackClick()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, this.rayCastMaxDist, this.attackableLayer, QueryTriggerInteraction.Ignore))
            {

                if (this.objectsInRangeHashSet.Contains(hit.transform.gameObject))
                {
                    this.networkAnimator.SetTrigger("Attack");
                    this.animator.speed = this.stats.UnitAttackSpeed;
                    this.transform.LookAt(hit.transform);
                }
                this.followAttack = true;
                this.targetEnemy = hit.transform.gameObject;
            }
            else
            {
                this.followAttack = false;
                this.targetEnemy = null;
            }
        }
    }

    
    public void Attack()
    {
        this.CmdAttack(this.targetEnemy, this.gameObject, this.stats.UnitAttackDamage);
    }

    [Command]
    private void CmdAttack(GameObject target, GameObject owner, float damage)
    {
        GameObject instProjectile = Instantiate(this.projectile, new Vector3(this.transform.position.x, this.transform.position.y + 0.4f, this.transform.position.z), Quaternion.identity);
        HomingMissileController missile = instProjectile.GetComponent<HomingMissileController>();
        missile.target = target;
        missile.owner = owner;
        missile.damage = damage;

        NetworkServer.Spawn(instProjectile);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger || other.gameObject.layer == this.gameObject.layer)
            return;

        this.objectsInRangeHashSet.Add(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        this.objectsInRangeHashSet.Remove(other.gameObject);
    }

    private void AssignAttackableLayer()
    {
        switch (this.gameObject.layer)
        {
            case Enums.Layers.blueTeamLayer:
                this.attackableLayer |= (1 << Enums.Layers.redTeamLayer);
                break;
            case Enums.Layers.redTeamLayer:
                this.attackableLayer |= (1 << Enums.Layers.blueTeamLayer);
                break;
        }
        this.attackableLayer |= 1 << Enums.Layers.neutral;
    }

    [ClientCallback]
    private void FollowAttack()
    {
        if (!this.followAttack)
        {
            return;
        }

        if (this.targetEnemy == null)
        {
            this.followAttack = false;
            return;
        }
        
        if (this.objectsInRangeHashSet.Contains(this.targetEnemy.gameObject))
        {
            this.networkAnimator.SetTrigger("Attack");
            this.animator.speed = this.stats.UnitAttackSpeed;
            this.transform.LookAt(this.targetEnemy.gameObject.transform);
        }
        else
        {
            this.playerMovement.MoveToPoint(this.targetEnemy.transform.position);
        }
    }
}
