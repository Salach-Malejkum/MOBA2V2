using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

namespace Enums
{
    public enum AttackResult
    {
        OnCooldown = -1,
        OutOfRange = 0,
        CanAttack = 1,
        Dead = 2,
        FriendlyFire = 3,
    }

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


