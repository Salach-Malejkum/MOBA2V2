using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ResourceSpawner : MonoBehaviour
{
    public GameObject[] resourcePrefabs;
    public float scale = 1f;
    public float nextSpawnTime = 5;
    public float time = 0;
    public  float spawnDelay = 10;
    public int numberSpawned = 1;
    public float spawnRadius = 0;
    private float[] spawningX;
    private float[] spawningZ;
    public Dictionary<int, GameObject> children = new();

    private bool isSpawning = true;

    private void Start()
    {
        this.spawningX = new float[this.numberSpawned];
        this.spawningZ = new float[this.numberSpawned];

        if (this.numberSpawned == 1)
        {
            this.spawningX[0] = this.transform.position.x;
            this.spawningZ[0] = this.transform.position.z;
        }
        else
        {
            GetSpawningPoints();
        }
    }

    private void Update()
    {
        this.time = Time.time;
        if (this.isSpawning && Time.time > this.nextSpawnTime)
        {
            this.Spawn();
        }
    }

    public double ConvertToRadians(double angle)
    {
        return (Math.PI / 180) * angle;
    }

    private void GetSpawningPoints()
    {
        var x0 = this.transform.position.x;
        var z0 = this.transform.position.z;

        for (int i = 0; i < this.numberSpawned; i++)
        {
            double angle = i * (360 / this.numberSpawned);

            this.spawningX[i] = (float)(x0 + this.spawnRadius * Math.Cos(ConvertToRadians(angle)));
            this.spawningZ[i] = (float)(z0 + this.spawnRadius * Math.Sin(ConvertToRadians(angle)));
        {
            this.spawningX[0] = this.transform.position.x;
            this.spawningZ[0] = this.transform.position.z;
        }
        else
        {
            GetSpawningPoints();
        }
    }

    private void Update()
    {
        this.time = Time.time;
        if (this.isSpawning && Time.time > this.nextSpawnTime)
        {
            this.Spawn();
        }
    }

    public double ConvertToRadians(double angle)
    {
        return (Math.PI / 180) * angle;
    }

    private void GetSpawningPoints()
    {
        var x0 = this.transform.position.x;
        var z0 = this.transform.position.z;

        for (int i = 0; i < this.numberSpawned; i++)
        {
            double angle = i * (360 / this.numberSpawned);

            this.spawningX[i] = (float)(x0 + this.spawnRadius * Math.Cos(ConvertToRadians(angle)));
            this.spawningZ[i] = (float)(z0 + this.spawnRadius * Math.Sin(ConvertToRadians(angle)));
        }
    }

    private void Spawn()
    {
        for (int i = 0; i < this.numberSpawned; i++)
        {
            var spawningPoint = new Vector3(this.spawningX[i]
                , this.transform.position.y
                , this.spawningZ[i]
                );
            int indexPrefab = Random.Range(0, this.resourcePrefabs.Length - 1);

            GameObject go = Instantiate(this.resourcePrefabs[indexPrefab]
                , spawningPoint
                , this.transform.rotation
                );
            go.transform.SetParent(this.transform);
            go.transform.localScale = new Vector3(1, 1, 1) * this.scale;

            MobController mc = go.GetComponent<MobController>();
            mc.spawnerResource = this.gameObject.GetComponent<ResourceSpawner>();
            mc.spawner = this.transform;
            mc.Id = i;

            this.children.Add(i, go);
        }

        this.isSpawning = false;
    }

    private void StartSpawner()
    {
        this.isSpawning = true;
        this.nextSpawnTime = Time.time + this.spawnDelay;
    }

    public void NotifyAllChildren(GameObject target)
    {
        foreach (var child in this.children)
        {
            MobController mc = child.Value.GetComponent<MobController>();
            mc.target = target;
            mc.isChasing = true;
        }
    }

    public void RemoveFromChildren(int id)
    {
        if (this.children.ContainsKey(id))
        {
            this.children.Remove(id);
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
