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
    private float checkRadius = 2f;

    void Update()
    {
        CheckColliders();

        if (isSpawning && Time.time > nextSpawnTime)
        {
            Spawn();
        }
    }

    private void Spawn()
    {
        nextSpawnTime = Time.time + spawnDelay;
        Instantiate(resourcePrefab, transform.position, transform.rotation);
        isSpawning = false;
    }

    private void CheckColliders()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, checkRadius);

        isSpawning = true;

        foreach (Collider col in colliders)
        {
            if (col.tag == "Resource")
            {
                isSpawning = false;
                nextSpawnTime = Time.time + spawnDelay;
            }
        }
    }
}
