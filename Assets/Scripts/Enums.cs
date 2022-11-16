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
        public static readonly Vector3[] topPathPoints = { new Vector3(122f, 0.5f, -15.5f), new Vector3(97f, 0.5f, -14.5f), new Vector3(77f, 0.5f, -12.7f), new Vector3(63f, 0.5f, -12.5f), new Vector3(49f, 0.5f, -10.8f), new Vector3(35f, 0.5f, -8f), new Vector3(20f, 0.5f, -6.4f), new Vector3(0f, 0.5f, -5.7f), new Vector3(-20f, 0.5f, -6.4f), new Vector3(-35f, 0.5f, -8f), new Vector3(-49f, 0.5f, -10.8f), new Vector3(-63f, 0.5f, -12.5f), new Vector3(-77f, 0.5f, -12.7f), new Vector3(-97f, 0.5f, -14.5f), new Vector3(-122f, 0.5f, -15.5f) }; // choose and assign point from the real map
        public static readonly Vector3[] botPathPoints = null; // choose and assign point from the real map
    }

    public enum MinionSpawnTime  // Can be changed for the debug option
    {
        FirstSpawnTimePeriod = 9,
        NormalSpawnTimePeriod = 60
    }

    public static class MinionPrefabs
    {
        public readonly static string meleeMinionPath = "MeleeMinion.prefab";
        public readonly static string rangedMinionPath = "Minion"; // 4 testing
        public readonly static string cannonMinionPath = "CannonMinion.prefab";
    }

    public static class TeamMinionSpawnerPosition
    {
        public readonly static Vector3 redTeamSpawn = new Vector3(-125f, 0.5f, -15f);
        public readonly static Vector3 blueTeamSpawn = new Vector3(125f, 0.5f, -15f);
    }

    public static class TeamMinionSpawnerTags
    {
        public const string blueTeamTag = "Blue_Team";
        public const string redTeamTag = "Red_Team";
    }
}