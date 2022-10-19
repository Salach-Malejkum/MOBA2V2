using UnityEngine;

public class MinionSpawnerScript : MonoBehaviour, ISpawner
{
    // might change some variables and methods to static
    private GameObject meleeMinionPrefab;
    private GameObject rangedMinionPrefab;
    private GameObject cannonMinionPrefab;
    private int spawnsFinished;
    private Vector3 spawnPosition;
    private bool wasFirstWaveSpawned; // should be false, could be true for the debug option
    private readonly int cannonWave;

    public MinionSpawnerScript(Vector3 spawnPosition, string tagName)
    {
        this.meleeMinionPrefab = Instantiate(Enums.MinionPrefabs.meleeMinion);
        this.meleeMinionPrefab.tag = tagName;
        this.rangedMinionPrefab = Instantiate(Enums.MinionPrefabs.rangedMinion);
        this.rangedMinionPrefab.tag = tagName;
        this.cannonMinionPrefab = Instantiate(Enums.MinionPrefabs.cannonMinion);
        this.cannonMinionPrefab.tag = tagName;

        this.spawnsFinished = 0;
        this.spawnPosition = spawnPosition;
        this.wasFirstWaveSpawned = false;
        this.cannonWave = 3;
    }

    private bool CheckIfCanFirstSpawn(float timePassed)
    {
        return !this.wasFirstWaveSpawned
            && timePassed / (float)Enums.MinionSpawnTime.FirstSpawnTimePeriod >= this.spawnsFinished + 1;
    }

    private bool CheckIfCanNormalSpawn(float timePassed)
    {
        return this.wasFirstWaveSpawned
            && timePassed / (float)Enums.MinionSpawnTime.NormalSpawnTimePeriod >= this.spawnsFinished + 1;
    }

    public bool CheckIfCanSpawm(float timePassed)
    {
        bool result = this.CheckIfCanFirstSpawn(timePassed) || this.CheckIfCanNormalSpawn(timePassed);
        this.TryIncreaseSpawnsFinished(result);
        return result; 
    }

    private void TryIncreaseSpawnsFinished(bool result)
    {
        if (result)
            this.spawnsFinished += 1;
    }

    public bool CheckIfSpawnCannon()
    {
        return (this.spawnsFinished) % this.cannonWave == 0;
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
}
