using UnityEngine;

public interface IMinionMovement
{
    // Start is called before the first frame update
    void Move();
    void SetPathPoints(Vector3[] path);
}
