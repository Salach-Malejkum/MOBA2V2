using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceSpawner : MonoBehaviour
{
    public GameObject resourcePrefab;
    public float nextSpawnTime = 5;
    public  float spawnDelay = 10;

    private bool isSpawning = true;

    void Update()
    {
        if (this.isSpawning && Time.time > this.nextSpawnTime)
        {
            Spawn();
        }
    }

    private void Spawn()
    {
        GameObject go = Instantiate(this.resourcePrefab, this.transform.position, this.transform.rotation);
        DestroyResource dr = go.GetComponent<DestroyResource>();
        dr.spawnerResource = this.gameObject.GetComponent<ResourceSpawner>();

        this.isSpawning = false;
    }

    public void StartSpawner()
    {
        this.isSpawning = true;
        this.nextSpawnTime = Time.time + this.spawnDelay;
    }
}
