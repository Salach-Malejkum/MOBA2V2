using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ResourceSpawner : NetworkBehaviour
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

    [ServerCallback]
    private void Awake()
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

    [ServerCallback]
    private void FixedUpdate()
    {
        this.time = Time.time;
        if (this.isSpawning && Time.time > this.nextSpawnTime)
        {
            this.Spawn();
        }
    }

    [Server]
    public double ConvertToRadians(double angle)
    {
        return (Math.PI / 180) * angle;
    }

    [Server]
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

    [ServerCallback]
    private void Spawn()
    {
        for (int i = 0; i < this.numberSpawned; i++)
        {
            Vector3 spawningPoint = new Vector3(this.spawningX[i]
                , this.transform.position.y
                , this.spawningZ[i]
                );
            int indexPrefab = Random.Range(0, this.resourcePrefabs.Length - 1);

            GameObject go = Instantiate(this.resourcePrefabs[indexPrefab]
                , spawningPoint
                , this.transform.rotation
                );
            go.transform.localScale = new Vector3(1, 1, 1) * this.scale;

            MobController mobController = go.GetComponent<MobController>();
            mobController.spawner = this.transform;
            mobController.spawnerResource = this.GetComponent<ResourceSpawner>();

            MonsterStats monsterStats = go.GetComponent<MonsterStats>();
            monsterStats.Id = i;

            NetworkServer.Spawn(go);
            go.GetComponent<MonsterStats>().OnMobSpawned();

            this.children.Add(i, go);
        }

        this.isSpawning = false;
    }

    [Server]
    private void StartSpawner()
    {
        this.isSpawning = true;
        this.nextSpawnTime = Time.time + this.spawnDelay;
    }

    [ServerCallback]
    public void NotifyAllChildren(GameObject target)
    {
        foreach (var child in this.children)
        {
            MobController mc = child.Value.GetComponent<MobController>();
            mc.target = target;
            mc.isChasing = true;
        }
    }

    [Server]
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
