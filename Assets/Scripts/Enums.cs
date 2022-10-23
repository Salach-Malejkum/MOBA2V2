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
        public static readonly Vector3[] topPathPoints = { new Vector3(-5f, 0.5f, 0f), new Vector3(-10f, 0.5f, 0f), new Vector3(-10f, 0.5f, 5f), new Vector3(-10f, 0.5f, 10f) }; // choose and assign point from the real map
        public static readonly Vector3[] botPathPoints = null; // choose and assign point from the real map
    }
}
