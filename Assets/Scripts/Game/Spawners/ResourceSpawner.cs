using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceSpawner : MonoBehaviour
{
    public GameObject resourcePrefab;
    public float nextSpawnTime = 5;
    public  float spawnDelay = 10;
    public float numberSpawned = 3;
    public Dictionary<int, GameObject> children = new Dictionary<int, GameObject>();

    private bool isSpawning = true;

    void Update()
    {
        if (this.isSpawning && Time.time > this.nextSpawnTime)
        {
            Spawn();
        }
    }
    private Vector3 GetSpawningPoints(int position, float radius)
    {
        Vector3 location = Vector3.zero;
        switch (numberSpawned)
        {
            case 1:
                location = this.transform.position;
                break;
            case 2:
                if (position == 0)
                {
                    location = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z - 2*radius);
                } else
                {
                    location = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z + 2*radius);
                }
                break;
            default:
                location = this.transform.position;
                break;
        }

        return location;
    }

    private void Spawn()
    {
        for (int i = 0; i < numberSpawned; i++)
        {
            float radius = this.resourcePrefab.GetComponent<CapsuleCollider>().radius;
            Vector3 spawningPoint = GetSpawningPoints(i, radius);

            GameObject go = Instantiate(this.resourcePrefab, spawningPoint, this.transform.rotation);
            MobController mc = go.GetComponent<MobController>();
            mc.spawnerResource = this.gameObject.GetComponent<ResourceSpawner>();
            mc.spawner = this.transform;
            mc.Id = i;

            children.Add(i, go);
        }

        this.isSpawning = false;
    }

    private void StartSpawner()
    {
        this.isSpawning = true;
        this.nextSpawnTime = Time.time + this.spawnDelay;
    }

    public void RemoveFromChildren(int id)
    {
        if (children.ContainsKey(id))
        {
            children.Remove(id);
        } else
        {
            Debug.Log("WTF");
        }

        if (children.Count == 0)
        {
            StartSpawner();
        }
    }
}
