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
        public static readonly Vector3[] topPathPoints = { new Vector3(122f, 0.5f, -15.5f), new Vector3(97f, 0.5f, -14.5f), new Vector3(77f, 0.5f, -12.7f), new Vector3(63f, 0.5f, -12.5f)
                , new Vector3(49f, 0.5f, -10.8f), new Vector3(35f, 0.5f, -8f), new Vector3(20f, 0.5f, -6.4f), new Vector3(0f, 0.5f, -5.7f), new Vector3(-20f, 0.5f, -6.4f)
                , new Vector3(-35f, 0.5f, -8f), new Vector3(-49f, 0.5f, -10.8f), new Vector3(-63f, 0.5f, -12.5f), new Vector3(-77f, 0.5f, -12.7f), new Vector3(-97f, 0.5f, -14.5f)
                , new Vector3(-122f, 0.5f, -15.5f) }; // choose and assign point from the real map
        public static readonly Vector3[] botPathPoints = { new Vector3(120f, 0.5f, -10f), new Vector3(105f, 0.5f, 8f), new Vector3(87f, 0.5f, 20f), new Vector3(67f, 0.5f, 29f)
                , new Vector3(48f, 0.5f, 38f), new Vector3(29f, 0.5f, 46f), new Vector3(14f, 0.5f, 48f), new Vector3(-14f, 0.5f, 48f)
                , new Vector3(-29f, 0.5f, 46f), new Vector3(-48f, 0.5f, 38f), new Vector3(-67f, 0.5f, 29f), new Vector3(-87f, 0.5f, 20f), new Vector3(-105f, 0.5f, 8f)
                , new Vector3(-120f, 0.5f, -10f)}; // choose and assign point from the real map
    }

    public enum MinionSpawnTime  // Can be changed for the debug option
    {
        FirstSpawnTimePeriod = 9,
        NormalSpawnTimePeriod = 30
    }

    public interface IMinionsPrefabs {
        string GetMeleePath();
        string GetRangedPath();
        string GetCannonPath();
    }
    public class BlueMinionsPrefabs : IMinionsPrefabs
    {
        private readonly string meleeMinionBluePath = "MeleeMinion.prefab"; 
        private readonly string rangedMinionBluePath = "MinionBlue"; // 4 testing
        private readonly string cannonMinionBluePath = "CannonMinion.prefab";

        public string GetMeleePath()
        {
            return this.meleeMinionBluePath;
        }
        public string GetRangedPath()
        {
            return this.rangedMinionBluePath;
        }
        public string GetCannonPath()
        {
            return this.cannonMinionBluePath;
        }
    }

    public class RedMinionsPrefabs : IMinionsPrefabs
    {
        private readonly string meleeMinionRedPath = "MeleeMinion.prefab";
        private readonly string rangedMinionRedPath = "MinionRed"; // 4 testing
        private readonly string cannonMinionRedPath = "CannonMinion.prefab";

        public string GetMeleePath()
        {
            return this.meleeMinionRedPath;
        }
        public string GetRangedPath()
        {
            return this.rangedMinionRedPath;
        }
        public string GetCannonPath()
        {
            return this.cannonMinionRedPath;
        }
    }

    public static class TeamMinionSpawnerPosition
    {
        public readonly static Vector3 redTeamSpawn = new Vector3(-116f, 0.5f, -15f);
        public readonly static Vector3 blueTeamSpawn = new Vector3(116f, 0.5f, -15f);
    }

    public static class Layers
    {
        public const int blueTeamLayer = 7;
        public const int redTeamLayer = 6;
        public const int terrainLayer = 8;
    }
}