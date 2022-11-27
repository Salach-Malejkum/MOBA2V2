using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingMissileController : NetworkBehaviour
{
    public GameObject target;
    public GameObject owner;
    public float damage;
    public bool stun = false;
    public float stunTime = 0f;
    private float speed = 20f;

    private void Awake()
    {
        this.gameObject.transform.TransformPoint(Vector3.zero);
    }

    [ServerCallback]
    private void FixedUpdate()
    {
        if (this.target == null)
        {
            NetworkServer.Destroy(this.gameObject);
        }
        else
        {
            this.gameObject.transform.position += (target.transform.position - this.gameObject.transform.position).normalized * this.speed * Time.deltaTime;
            this.gameObject.transform.LookAt(this.target.transform);
        }                
    }


    [Server]
    private void OnTriggerEnter(Collider other)
    {
        if (!other.isTrigger && other.gameObject == this.target)
        {
            other.gameObject.GetComponent<UnitStats>().RemoveHealthOnNormalAttack(this.damage, this.owner);
            if (this.stun && other.tag != Enums.Tags.tower)
            {
                other.gameObject.GetComponent<CrowdControl>().Stun(this.stunTime);
            }
            NetworkServer.Destroy(this.gameObject);
        }
    }
}
