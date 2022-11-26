using Mirror;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : NetworkBehaviour, IMovement
{
    private readonly int rayCastMaxDist = 100;
    private NavMeshAgent navMeshAgent;
    private Animator animator;

    [ClientCallback]
    public void Start()
    {
        this.navMeshAgent = this.GetComponent<NavMeshAgent>();
        this.animator = this.GetComponent<Animator>();
    }

    [ClientCallback]
    public void FixedUpdate()
    {
        this.animator.SetBool("IsWalking", this.IsMoving());
        this.StopMovingForSkillAnimations();


    }

    [ClientCallback]
    public void Move()
    {
        if (!hasAuthority)
        {
            return;
        }

        LayerMask terrainLayer = 1 << 8;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, this.rayCastMaxDist, terrainLayer, QueryTriggerInteraction.Ignore))
        {
            this.navMeshAgent.destination = hit.point;
        }
    }

    [ClientCallback]
    private bool IsMoving()
    {
        return navMeshAgent.velocity.magnitude > 0.2f;
    }

    private void StopMovingForSkillAnimations()
    {
        string animName = this.animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
        Debug.Log(animName);
        switch (animName)
        {
            case "Standing 2H Cast Spell 01":
            case "Standing 2H Magic Area Attack 02":
            case "Standing 2H Magic Attack 01":
                this.navMeshAgent.isStopped = true;
                break;
            default:
                this.navMeshAgent.isStopped = false;
                break;
        }
    }

    private void DashMovement()
    {

    }
}
