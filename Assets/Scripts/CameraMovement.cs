using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public GameObject player;

    private bool isLocked = false;
    private readonly float cameraSpeed = 3.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ManageCameraLock();
        TryToMoveCamera();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CenterOnPlayer();
        }
    }

    void CenterOnPlayer()
    {
        this.gameObject.transform.position = player.transform.position;
    }

    void ManageCameraLock()
    {
        if (Input.GetKey(KeyCode.Y))
        {
            isLocked = !isLocked;
            TrySetCameraParent();
        }
    }

    void TrySetCameraParent()
    {
        if (isLocked)
        {
            this.gameObject.transform.SetParent(player.transform, false);
        }
        else
        {
            this.gameObject.transform.SetParent(null);
        }
    }

    void TryToMoveCamera()
    {
        if (isLocked)
        {
            return;
        }

        System.Tuple<bool, Vector2> isOnEdgeAndDirection = IsCursorOnEdgeAndDirection();
        if (isOnEdgeAndDirection.Item1)
        {
            Vector2 moveDirection = isOnEdgeAndDirection.Item2;
            Debug.Log(moveDirection);
            this.gameObject.transform.position += new Vector3(moveDirection.x, moveDirection.y, 0) * Time.deltaTime * this.cameraSpeed;
        }
    }

    System.Tuple<bool, Vector2> IsCursorOnEdgeAndDirection()
    {
        bool isTouchTop = Input.mousePosition.y >= Screen.height;
        bool isTouchBottom = Input.mousePosition.y <= 0;
        bool isTouchRight = Input.mousePosition.x >= Screen.width;
        bool isTouchLeft = Input.mousePosition.x <= 0;

        if (isTouchTop || isTouchLeft || isTouchRight || isTouchBottom)
        {
            int xMovement = SetXMovementDirection(isTouchLeft, isTouchRight);
            int yMovement = SetYMovementDirection(isTouchTop, isTouchBottom);

            return new System.Tuple<bool, Vector2>(true, new Vector2(xMovement, yMovement));
        }
        return new System.Tuple<bool, Vector2>(false, new Vector2(0, 0));
    }

    int SetYMovementDirection(bool isTouchTop, bool isTouchBottom)
    {
        int yMovement = 0;

        if (isTouchTop)
        {
            yMovement = 1;
        }
        else if (isTouchBottom)
        {
            yMovement = -1;
        }
        return yMovement;
    }

    int SetXMovementDirection(bool isTouchLeft, bool isTouchRight)
    {
        int xMovement = 0;

        if (isTouchLeft)
        {
            xMovement = 1;
        }
        else if (isTouchRight)
        {
            xMovement = -1;
        }
        return xMovement;
    }
}
