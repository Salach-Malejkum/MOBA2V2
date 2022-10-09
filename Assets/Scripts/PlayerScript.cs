using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerScript : MonoBehaviour
{
    private IAttack attackScript;
    private IMovement movementScript;
    private new Camera camera;
    private NavMeshAgent navMeshAgent;

    // Start is called before the first frame update
    void Start()
    {
        attackScript = new AttackPlayerScript(500.0f, 0.5f);
        movementScript = new PlayerMovement(this.GetComponent<NavMeshAgent>());
        camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100))
            {
                switch (hit.transform.gameObject.tag)
                {
                    case "Terrain":
                        //Move
                        Debug.Log("Move");
                        this.movementScript.Move();
                        break;
                    default:
                        //Attack
                        int attackResult = this.attackScript.TryAttack(this.transform.position, hit.transform.position);
                        switch (attackResult)
                        {
                            case (int)Enums.AttackResult.Attacked:
                                //Attack
                                break;
                            case (int)Enums.AttackResult.OutOfRange:
                                this.movementScript.Move();
                                break;
                            case (int)Enums.AttackResult.OnCooldown:
                                //CD
                            default :
                                break;
                        }
                        break;
                }
            }
        }
    }
}
