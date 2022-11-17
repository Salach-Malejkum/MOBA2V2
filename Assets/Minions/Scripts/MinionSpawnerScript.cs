using UnityEditor;
using UnityEngine;

//TODO: Dodac interfejs z powrotem i poprawic PrefabUtility
public class MinionSpawnerScript
{
    // might change some variables and methods to static
    private Enums.IMinionsPrefabs minionsPrefabs;
    private GameObject meleeMinionPrefab;
    private GameObject rangedMinionPrefab;
    private GameObject cannonMinionPrefab;
    private Vector3[] minionPath;
    private int spawnsFinished;
    private Vector3 spawnPosition;
    private bool wasFirstWaveSpawned; // should be false, could be true for the debug option
    private readonly int whenCannonWave;
    private readonly int layer;

    public MinionSpawnerScript(Vector3 spawnPosition, int layer, Vector3[] minionPath)
    {
        switch (LayerMask.LayerToName(layer))
        {
            case "Blue":
                this.minionsPrefabs = new Enums.BlueMinionsPrefabs();
                break;
            case "Red":
                this.minionsPrefabs = new Enums.RedMinionsPrefabs();
                break;
        }
        this.meleeMinionPrefab = Resources.Load<GameObject>(this.minionsPrefabs.GetRangedPath());
        this.rangedMinionPrefab = Resources.Load<GameObject>(this.minionsPrefabs.GetRangedPath());
        this.cannonMinionPrefab = Resources.Load<GameObject>(this.minionsPrefabs.GetRangedPath());

        this.minionPath = minionPath;

        this.spawnsFinished = 0;
        this.spawnPosition = spawnPosition;
        this.wasFirstWaveSpawned = false;
        this.whenCannonWave = 3;
        this.layer = layer;
        this.minionPath = minionPath;
    }

    private bool CheckIfCanFirstSpawn(float timePassed)
    {
        bool result = !this.wasFirstWaveSpawned
            && (timePassed / (float)Enums.MinionSpawnTime.FirstSpawnTimePeriod >= this.spawnsFinished + 1);

        if (result)
        {
            this.wasFirstWaveSpawned = true;
        }

        return result;
    }

    private bool CheckIfCanNormalSpawn(float timePassed)
    {
        return this.wasFirstWaveSpawned
            && (timePassed / (float)Enums.MinionSpawnTime.NormalSpawnTimePeriod >= this.spawnsFinished + 1);
    }

    public bool CheckIfCanSpawn(float timePassed)
    {
        bool result = this.CheckIfCanFirstSpawn(timePassed) || this.CheckIfCanNormalSpawn(timePassed);
        this.TryIncreaseSepawnsFinished(result);
        return result;
    }

    private void TryIncreaseSepawnsFinished(bool result)
    {
        if (result)
        {
            this.spawnsFinished += 1;
        }
    }

    public bool CheckIfCanSpawnCannon()
    {
        return this.spawnsFinished % this.whenCannonWave == 0;
    }

    public Vector3 GetSpawnPosition()
    {
        return this.spawnPosition;
    }

    public GameObject GetMeleeMinionPrefab()
    {
        return this.meleeMinionPrefab;
    }

    public GameObject GetRangedMinionPrefab()
    {
        return this.rangedMinionPrefab;
    }

    public GameObject GetCannonMinionPrefab()
    {
        return this.cannonMinionPrefab;
    }

    public int GetLayer()
    {
        return this.layer;
    }

    public Vector3[] GetMinionPath()
    {
        return this.minionPath;
    }
}
