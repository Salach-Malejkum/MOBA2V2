using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class TowerController : MonoBehaviour
{
    public Outline outline;
    public GameObject missile;
    public float missile_timer = 0f;

    public float deleteOutlineTimer = 0f;

    private List<GameObject> enemies = new List<GameObject>();
    private readonly float missile_delay = 5f;
    private int targetedLayer;

    private void Awake()
    {
        int redLayer = LayerMask.NameToLayer("Red");
        int blueLayer = LayerMask.NameToLayer("Blue");

        this.outline = GetComponent<Outline>();

        if (this.gameObject.layer == redLayer)
        {
            this.targetedLayer = blueLayer;
        } 
        else if (this.gameObject.layer == blueLayer)
        {
            this.targetedLayer = redLayer;
        }
    }

    private void FixedUpdate()
    {
        if (this.outline.OutlineWidth > 0 && Time.time > this.deleteOutlineTimer)
        {
            this.outline.OutlineWidth = 0f;
        }
        if (this.enemies.Count > 0 && this.missile_timer < 0)
        {
            foreach (GameObject enemy in this.enemies)
            {
                if (enemy == null)
                {
                    this.enemies.Remove(enemy);
                }
            }

            this.Shoot();
        }
        else if (this.missile_timer >= 0)
        {
            this.missile_timer -= Time.deltaTime;
        }
    }

    //private void OnMouseOver()
    //{
    //    this.deleteOutlineTimer = Time.time + componentDeleteDelay;

    //    if (!GetComponent<Outline>())
    //    {
    //        var outline = this.gameObject.AddComponent<Outline>();

    //        outline.OutlineMode = Outline.Mode.OutlineVisible;
    //        outline.OutlineColor = Color.red;
    //        outline.OutlineWidth = 5f;
    //    }
    //}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == this.targetedLayer && !other.gameObject.CompareTag("Missile"))
        {
            this.enemies.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == this.targetedLayer && !other.gameObject.CompareTag("Missile"))
        {
            this.enemies.Remove(other.gameObject);
        }
    }

    GameObject GetClosestEnemy(List<GameObject> enemies)
    {
        GameObject tMin = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = this.transform.position;
        foreach (GameObject go in enemies)
        {
            float dist = Vector3.Distance(go.transform.position, currentPos);
            if (dist < minDist)
            {
                tMin = go;
                minDist = dist;
            }
        }
        return tMin;
    }

    private void Shoot()
    {
        GameObject go = Instantiate(this.missile, this.transform);
        HomingMissileController hmc = go.GetComponent<HomingMissileController>();

        hmc.target = this.GetClosestEnemy(this.enemies);

        this.missile_timer = this.missile_delay;
    }
}
