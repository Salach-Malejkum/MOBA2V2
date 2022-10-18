using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionSpawnerScript
{
    public GameObject minionPrefab;
    private float timePassedSinceSpawn = 0f;
    private Vector3 spawnPosition;
    private bool wasSpawningDelayed = false; // should be false, for the debug you can set true

    // Start is called before the first frame update

    public MinionSpawnerScript(Vector3 spawnPosition)
    {
        this.spawnPosition = spawnPosition;
    }

    // Update is called once per frame
    public void FixedUpdate()
    {
        this.timePassedSinceSpawn += Time.deltaTime;

        Debug.Log(Time.time);
        if (CheckIfCanFirstSpawn())
        {
            //Delayed spawn
            this.wasSpawningDelayed = true;
            Debug.Log("Delayed spawn");
            this.timePassedSinceSpawn = 0;
        }
        else if (CheckIfCanNormalSpawn())
        {
            //Normal spawn
            Debug.Log("Normal spawn");
            this.timePassedSinceSpawn = 0;
        }
    }

    private bool CheckIfCanFirstSpawn()
    {
        return !this.wasSpawningDelayed
            && this.timePassedSinceSpawn / (float)Enums.MinionSpawnTime.FirstSpawnTimePeriod >= 1f;
    }

    private bool CheckIfCanNormalSpawn()
    {
        return this.wasSpawningDelayed
            && this.timePassedSinceSpawn / (float)Enums.MinionSpawnTime.NormalSpawnTimePeriod >= 1f;
    }
}
