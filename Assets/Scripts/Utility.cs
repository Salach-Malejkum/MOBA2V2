using UnityEngine;

public static class Utility
{
    public static float GetDistanceBetweenGameObjects(GameObject o1, GameObject o2)
    {
        return Vector3.Distance(o1.transform.position, o2.transform.position);
    }
}
