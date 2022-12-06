using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerController : NetworkBehaviour, IOutlinable
{
    public Outline outline;
    public GameObject missile;
    public float missile_timer = 0f;

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

    [ServerCallback]
    private void FixedUpdate()
    {
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

    [ClientCallback]
    public IEnumerator DeleteOutline(float timeDelay)
    {
        yield return new WaitForSeconds(timeDelay);
        this.outline.OutlineWidth = 0f;
    }

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

    [ServerCallback]
    GameObject GetClosestEnemy(List<GameObject> enemies)
    {
        GameObject tMin = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = this.transform.position;
        foreach (GameObject go in enemies)
        {
            if (go.tag == "Player" && !go.active)
            {
                continue;
            }

            float dist = Vector3.Distance(go.transform.position, currentPos);
            if (dist < minDist)
            {
                tMin = go;
                minDist = dist;
            }
        }
        return tMin;
    }

    [ServerCallback]
    private void Shoot()
    {
        GameObject go = Instantiate(this.missile, this.transform.position, Quaternion.identity);
        HomingMissileController hmc = go.GetComponent<HomingMissileController>();
        hmc.target = this.GetClosestEnemy(this.enemies);
        hmc.owner = this.gameObject;
        hmc.damage = this.gameObject.GetComponent<StructureStats>().UnitAttackDamage;

        NetworkServer.Spawn(go);

        this.missile_timer = this.missile_delay;
    }
}
