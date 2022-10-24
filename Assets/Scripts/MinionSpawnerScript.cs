using UnityEditor;
using UnityEngine;

//TODO: Dodac interfejs z powrotem i poprawic PrefabUtility
public class MinionSpawnerScript
{
    // might change some variables and methods to static
    // private readonly GameObject meleeMinionPrefab;
    // private readonly GameObject rangedMinionPrefab;
    // private readonly GameObject cannonMinionPrefab;
    // private int spawnsFinished;
    // private Vector3 spawnPosition;
    // private bool wasFirstWaveSpawned; // should be false, could be true for the debug option
    // private readonly int cannonWave;

    // public MinionSpawnerScript(Vector3 spawnPosition, string tagName)
    // {
    //     this.meleeMinionPrefab = PrefabUtility.LoadPrefabContents(Enums.MinionPrefabs.meleeMinionPath);
    //     this.meleeMinionPrefab.tag = tagName;
    //     this.rangedMinionPrefab = PrefabUtility.LoadPrefabContents(Enums.MinionPrefabs.rangedMinionPath); ;
    //     this.rangedMinionPrefab.tag = tagName;
    //     this.cannonMinionPrefab = PrefabUtility.LoadPrefabContents(Enums.MinionPrefabs.cannonMinionPath); ;
    //     this.cannonMinionPrefab.tag = tagName;

    //     this.spawnsFinished = 0;
    //     this.spawnPosition = spawnPosition;
    //     this.wasFirstWaveSpawned = false;
    //     this.cannonWave = 3;
    // }

    // private bool CheckIfCanFirstSpawn(float timePassed)
    // {
    //     bool result = !this.wasFirstWaveSpawned
    //         && (timePassed / (float)Enums.MinionSpawnTime.FirstSpawnTimePeriod >= this.spawnsFinished + 1);

    //     if (result)
    //     {
    //         this.wasFirstWaveSpawned = true;
    //     }

    //     return result;
    // }

    // private bool CheckIfCanNormalSpawn(float timePassed)
    // {
    //     return this.wasFirstWaveSpawned
    //         && (timePassed / (float)Enums.MinionSpawnTime.NormalSpawnTimePeriod >= this.spawnsFinished + 1);
    // }

    // public bool CheckIfCanSpawm(float timePassed)
    // {
    //     bool result = this.CheckIfCanFirstSpawn(timePassed) || this.CheckIfCanNormalSpawn(timePassed);
    //     this.TryIncreaseSpawnsFinished(result);
    //     return result; 
    // }

    // private void TryIncreaseSpawnsFinished(bool result)
    // {
    //     if (result)
    //     {
    //         this.spawnsFinished += 1;
    //     }
    // }

    // public bool CheckIfSpawnCannon()
    // {
    //     return this.spawnsFinished % this.cannonWave == 0;
    // }
    
    // public Vector3 GetSpawnPosition()
    // {
    //     return this.spawnPosition;
    // }

    // public GameObject GetMeleeMinionPrefab()
    // {
    //     return this.meleeMinionPrefab;
    // }

    // public GameObject GetRangedMinionPrefab()
    // {
    //     return this.rangedMinionPrefab;
    // }

    // public GameObject GetCannonMinionPrefab()
    // {
    //     return this.cannonMinionPrefab;
    // }
}
