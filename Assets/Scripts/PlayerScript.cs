using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerScript : MonoBehaviour
{
    private IAttack playerAttackScript;
    private IMovement playerMovementScript;

    private bool followAttack = false;
    private GameObject followAttackObject;

    private void Start()
    {
        playerAttackScript = new AttackPlayerScript(5.0f, 0.5f);
        playerMovementScript = new PlayerMovement(this.GetComponent<NavMeshAgent>());
    }

    private void FixedUpdate()
    {
        if (followAttack)
        {
            int attackResult = this.playerAttackScript.TryAttack(this.gameObject, this.followAttackObject);
            this.ActionBasedOnTryAttackResult(attackResult, this.followAttackObject);
        }

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
                        this.ActionBasedOnTryAttackResult(attackResult, hit.transform.gameObject);

                        break;
                }
            }
        }
    }

    private void ActionBasedOnTryAttackResult(int attackResult, GameObject attackedObject)
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
}
