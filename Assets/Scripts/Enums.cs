using UnityEngine;

namespace Enums
{
    public enum AttackResult
    {
        CanAttack = 1,
        OutOfRange = 0,
        OnCooldown = -1
    }

    public static class MinionPaths
    {
        public static readonly Vector3[] topPathPoints = null;
        public static readonly Vector3[] botPathPoints = null;
    }
}
