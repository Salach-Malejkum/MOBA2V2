using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class TowerController : MonoBehaviour
{
    public GameObject missile;
    public float missile_timer = 0f;

    private List<GameObject> enemies = new List<GameObject>();
    private readonly float missile_delay = 5f;

    private void FixedUpdate()
    {
        if (this.enemies.Count > 0 && this.missile_timer < 0)
        {
            GameObject go = (GameObject)Instantiate(this.missile);
            HomingMissileController hmc = go.GetComponent<HomingMissileController>();

            hmc.target = GetClosestEnemy(enemies);

            this.missile_timer = this.missile_delay;
        } else if (this.missile_timer >= 0)
        {
            this.missile_timer -= Time.deltaTime;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Missile"))
        {
            UnityEngine.Debug.Log(other.gameObject.tag + " in");
            this.enemies.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.CompareTag("Missile"))
        {
            UnityEngine.Debug.Log(other.gameObject.tag + " out");
            this.enemies.Remove(other.gameObject);
        }
    }

    GameObject GetClosestEnemy(List<GameObject> enemies)
    {
        GameObject tMin = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;
        foreach (GameObject go in enemies)
        {
            float dist = Vector3.Distance(go.transform.position, currentPos);
            if (dist < minDist && !go.CompareTag("Missile"))
            {
                tMin = go;
                minDist = dist;
            }
        }
        return tMin;
    }
}
