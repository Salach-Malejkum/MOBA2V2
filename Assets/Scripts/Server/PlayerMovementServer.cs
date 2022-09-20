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
    [Client]
    void Update()
    {
        if(hasAuthority)
        {
            if (Input.GetMouseButtonDown(1))
            {
                Vector3 clickedPlace = GetClickedMapPoint();
                if (clickedPlace != new Vector3(0, -5, 0))
                {
                    MovePlayer(clickedPlace);
                }
            }
        }
    }

    Vector3 GetClickedMapPoint()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100))
        {
            return hit.point;
        }
        return new Vector3(0, -5, 0);
    }

    void MovePlayer(Vector3 movementDestination)
    {
        navMeshAgent.destination = movementDestination;
    }


}
