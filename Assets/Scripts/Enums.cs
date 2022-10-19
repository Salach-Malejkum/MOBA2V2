using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

namespace Enums
{
    public enum MinionSpawnTime  // Can be changed for the debug option
    {
        FirstSpawnTimePeriod = 9,
        NormalSpawnTimePeriod = 3
    }

    public static class MinionPrefabs
    {
        public readonly static GameObject meleeMinion = Resources.Load<GameObject>("Prefabs/MeleeMinion");
        public readonly static GameObject rangedMinion = Resources.Load<GameObject>("Prefabs/RangedMinion");
        public readonly static GameObject cannonMinion = Resources.Load<GameObject>("Prefabs/CannonMinion");
    }

    public static class TeamSpawnerPosition
    {
        public readonly static Vector3 redTeamSpawn = new Vector3(-5, 0, 0);
        public readonly static Vector3 blueTeamSpawn = new Vector3(5, 0, 0);
    }
}