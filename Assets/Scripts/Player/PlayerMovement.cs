using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    private new Camera camera;
    private NavMeshAgent navMeshAgent;
    private readonly int x = 15;
    private readonly int z = 1;

    void Start()
    {
        camera = Camera.main;
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Vector3 clickedPlace = GetClickedMapPoint();
            if (clickedPlace != new Vector3(0, -5, 0))
            {
                MovePlayer(clickedPlace);
            }
        }

        camera.transform.position = new Vector3(gameObject.transform.position.x - x, camera.transform.position.y, gameObject.transform.position.z - z);
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
