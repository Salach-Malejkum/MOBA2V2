using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerScriptV2 : NetworkBehaviour
{
    private IAttack playerAttackScript;
    private IMovement playerMovementScript;

    private readonly float componentDeleteDelay = .5f;
    private float deleteOutlineTimer = 0f;

    private bool followAttack = false;
    private GameObject followAttackObject;

    [ClientCallback]
    private void Start()
    {
        playerAttackScript = new AttackPlayerScript(5.0f, 0.5f);
        playerMovementScript = new PlayerMovement(this.GetComponent<NavMeshAgent>());
    }

    [ClientCallback]
    private void FixedUpdate()
    {
        if (followAttack)
        {
            int attackResult = this.playerAttackScript.TryAttack(this.gameObject, this.followAttackObject);
            this.CmdActionBasedOnTryAttackResult(attackResult, this.followAttackObject);
        }

        AddOutlineToTarget();

        if (Input.GetMouseButton(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100, Physics.AllLayers, QueryTriggerInteraction.Ignore))
            {
                switch (hit.transform.gameObject.tag)
                {
                    case "Terrain":
                        //Move
                        this.playerMovementScript.Move(hit.point);
                        this.followAttack = false;
                        this.followAttackObject = null;
                        break;
                    default:
                        //Attack
                        int attackResult = this.playerAttackScript.TryAttack(this.gameObject, hit.transform.gameObject);
                        this.CmdActionBasedOnTryAttackResult(attackResult, hit.transform.gameObject);

                        break;
                }
            }
        }
    }

    [ClientCallback]
    private void AddOutlineToTarget()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        LayerMask layerMask = 1 << 8;
        layerMask = ~layerMask;

        if (Physics.Raycast(ray, out hit, 100, layerMask, QueryTriggerInteraction.Ignore))
        {
            this.deleteOutlineTimer = Time.time + componentDeleteDelay;

            if (hit.transform.gameObject.TryGetComponent<Outline>(out Outline outline))
            {
                Debug.Log(hit.transform.gameObject.name);
                switch (hit.transform.gameObject.tag)
                {
                    case "Monster":
                        outline.OutlineWidth = 3f;
                        hit.transform.gameObject.GetComponent<MobController>().deleteOutlineTimer = this.deleteOutlineTimer;
                        break;
                    case "Tower":
                        outline.OutlineWidth = 5f;
                        hit.transform.gameObject.GetComponent<TowerController>().deleteOutlineTimer = this.deleteOutlineTimer;
                        break;
                }
            }
        }
    }

    [ClientRpc]
    private void RpcActionBasedOnTryAttackResult(int attackResult, GameObject attackedObject)
    {
        switch (attackResult)
        {
            case (int)Enums.AttackResult.CanAttack:
                if (this.gameObject.layer != this.followAttackObject.layer)
                {
                    Debug.Log("Attack");
                }
                this.playerMovementScript.Move(this.transform.position);
                //Attack
                break;
            case (int)Enums.AttackResult.FriendlyFire:
            case (int)Enums.AttackResult.OutOfRange:
                this.playerMovementScript.Move(attackedObject.transform.position);
                break;
            case (int)Enums.AttackResult.Dead:
                this.followAttackObject = null;
                this.followAttack = false;
                return;
            case (int)Enums.AttackResult.OnCooldown:
            //CD
            default:
                break;
        }
        this.followAttack = true;
        this.followAttackObject = attackedObject;
    }

    [Command]
    private void CmdActionBasedOnTryAttackResult(int attackResult, GameObject attackedObject)
    {
        this.RpcActionBasedOnTryAttackResult(attackResult, attackedObject);
    }
}
