using Mirror;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerMovement : NetworkBehaviour, IMovement
{
    private readonly int rayCastMaxDist = 100;
    private NavMeshAgent navMeshAgent;
    private Animator animator;
    private NetworkAnimator networkAnimator;
    public PlayerInput playerInput;


    public void Start()
    {
        this.navMeshAgent = this.GetComponent<NavMeshAgent>();
        this.animator = this.GetComponent<Animator>();
        this.networkAnimator = this.GetComponent<NetworkAnimator>();
        this.playerInput.enabled = true;
    }

    [ClientCallback]
    public void FixedUpdate()
    {
        if (!isLocalPlayer) { return; }
        this.animator.SetBool("IsWalking", this.IsMoving());
        this.StopMovingForSkillAnimations();
    }

    [Command]
    public void Move()
    {
        this.RpcMove();
    }

    [ClientRpc]
    private void RpcMove()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        if (!EventSystem.current.IsPointerOverGameObject())
        {
            LayerMask terrainLayer = 1 << 8;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, this.rayCastMaxDist, terrainLayer, QueryTriggerInteraction.Ignore))
            {
                this.networkAnimator.SetTrigger("CancelAutos");
                this.navMeshAgent.destination = hit.point;
            }
        }
    }

    [Command]
    public void MoveToPoint(Vector3 destinationPoint)
    {
        this.RpcMoveToPoint(destinationPoint);
    }

    [ClientRpc]
    private void RpcMoveToPoint(Vector3 destinationPoint)
    {
        this.navMeshAgent.destination = destinationPoint;
    }

    [ClientCallback]
    private bool IsMoving()
    {
        return navMeshAgent.velocity.magnitude > 0.2f;
    }

    [ClientCallback]
    private void StopMovingForSkillAnimations()
    {
        string animName = this.animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
        this.navMeshAgent.isStopped = animName switch
        {
            "Standing 2H Cast Spell 01" or "Standing 2H Magic Area Attack 02" or "Standing 2H Magic Attack 01" or "Standing 1H Magic Attack 01" => true,
            _ => false,
        };
    }

    private void DashMovement()
    {

    }
}
