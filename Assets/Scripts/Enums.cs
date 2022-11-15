<<<<<<< HEAD
=======
using System.Runtime.CompilerServices;
using UnityEditor;
>>>>>>> main
using UnityEngine;

namespace Enums
{
    public enum AttackResult
    {
        CanAttack = 1,
        OutOfRange = 0,
        OnCooldown = -1
    }

<<<<<<< HEAD
    public static class MinionPaths
    {
        public static readonly Vector3[] topPathPoints = { new Vector3(-5f, 0.5f, 0f), new Vector3(-10f, 0.5f, 0f), new Vector3(-10f, 0.5f, 5f), new Vector3(-10f, 0.5f, 10f) }; // choose and assign point from the real map
        public static readonly Vector3[] botPathPoints = null; // choose and assign point from the real map
    }
}
=======
    public enum MinionSpawnTime  // Can be changed for the debug option
    {
        FirstSpawnTimePeriod = 90,
        NormalSpawnTimePeriod = 30
    }

    public static class MinionPrefabs
    {
        public readonly static string meleeMinionPath = "Assets/Prefabs/MeleeMinion.prefab";
        public readonly static string rangedMinionPath = "Assets/Prefabs/RangedMinion.prefab";
        public readonly static string cannonMinionPath = "Assets/Prefabs/CannonMinion.prefab";
    }

    public static class TeamMinionSpawnerPosition
    {
        public readonly static Vector3 redTeamSpawn = new Vector3(-5, 0, 0);
        public readonly static Vector3 blueTeamSpawn = new Vector3(5, 0, 0);
    }

    public static class TeamMinionSpawnerTags
    {
        public readonly static string blueTeamTag = "Blue Team";
        public readonly static string redTeamTag = "Red Team";
    }
}


>>>>>>> main
