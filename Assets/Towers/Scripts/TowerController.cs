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

    public HashSet<GameObject> objectsInRangeHashSet;
    [SerializeField] private GameObject targetEnemy;
    private readonly float missile_delay = 5f;
    private int targetedLayer;

    private void Awake()
    {
        int redLayer = LayerMask.NameToLayer("Red");
        int blueLayer = LayerMask.NameToLayer("Blue");

        this.objectsInRangeHashSet = new HashSet<GameObject>();
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
    private void Update()
    {
        GameObject? closestEnemy = this.GetTheClosestEnemy();
        RemovePlayerWhenNotActive();

        if (this.objectsInRangeHashSet.Count > 0 && this.missile_timer < 0)
        {
            this.SetTargetEnemy(closestEnemy);
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
        if (!other.isTrigger
            && !other.gameObject.CompareTag("Missile")
            && other.gameObject.layer == this.targetedLayer
            )
        {
            this.objectsInRangeHashSet.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        this.objectsInRangeHashSet.Remove(other.gameObject);
    }

    private void SetTargetEnemy(GameObject gameObject)
    {
        this.targetEnemy = gameObject;
    }

    [ServerCallback]
    GameObject GetTheClosestEnemy()
    {
        GameObject closestEnemy = null;

        foreach (GameObject currEnemy in this.objectsInRangeHashSet)
        {
            if (currEnemy != null && closestEnemy == null)
            { closestEnemy = currEnemy; }
            else if (currEnemy != null && Utility.GetDistanceBetweenGameObjects(currEnemy, this.gameObject) < Utility.GetDistanceBetweenGameObjects(closestEnemy, this.gameObject))
            { closestEnemy = currEnemy; }
        }
        return closestEnemy;
    }

    private void RemovePlayerWhenNotActive()
    {
        if (this.targetEnemy != null && this.targetEnemy.tag == "Player" && !this.targetEnemy.activeSelf)
        {
            this.objectsInRangeHashSet.Remove(this.targetEnemy);
        }
    }

    [ServerCallback]
    private void Shoot()
    {
        if (this.gameObject == null) { return; }

        GameObject go = Instantiate(this.missile, this.transform.position, Quaternion.identity);
        HomingMissileController hmc = go.GetComponent<HomingMissileController>();
        hmc.target = this.targetEnemy;
        hmc.owner = this.gameObject;
        hmc.damage = this.gameObject.GetComponent<StructureStats>().UnitAttackDamage;

        NetworkServer.Spawn(go);

        this.missile_timer = this.missile_delay;
    }
}
