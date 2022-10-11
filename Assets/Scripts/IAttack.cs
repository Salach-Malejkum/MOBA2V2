using UnityEngine;

public interface IAttack
{
    int TryAttack(Vector3 currentPosition, Vector3 targetPosition);
}
