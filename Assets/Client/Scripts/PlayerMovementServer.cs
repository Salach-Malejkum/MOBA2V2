using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.AI;

public class PlayerMovementServer : NetworkBehaviour
{
    // Start is called before the first frame update
    private new Camera camera;
    private NavMeshAgent navMeshAgent;

    void Start()
    {
        camera = Camera.main;
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    [ClientCallback]
    void Update()
    {
        if(hasAuthority)
        {
            if (Input.GetMouseButtonDown(1))
            {
                Vector3 clickedPlace = GetClickedMapPoint();
                if (clickedPlace != new Vector3(0, -5, 0))
                {
                    CmdMove(clickedPlace);
                }
            }
        }
    }

    Vector3 GetClickedMapPoint()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100, Physics.AllLayers, QueryTriggerInteraction.Ignore))
        {
            return hit.point;
        }
        return new Vector3(0, -5, 0);
    }

    [Command]
    private void CmdMove(Vector3 movementDestination)
    {
        RpcMove(movementDestination);
    }

    [ClientRpc]
    private void RpcMove(Vector3 movementDestination) 
    {
        navMeshAgent.destination = movementDestination;
    } 
}
