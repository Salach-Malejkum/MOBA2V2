using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingMissileController : NetworkBehaviour
{
    public GameObject target;
    public GameObject owner;
    public float damage;
    private float speed = 10f;

    private void Awake()
    {
        gameObject.transform.TransformPoint(Vector3.zero);
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
            gameObject.transform.position += (target.transform.position - gameObject.transform.position).normalized * speed * Time.deltaTime;
            gameObject.transform.LookAt(target.transform);
        }                
    }


    [Server]
    private void OnTriggerEnter(Collider other)
    {
        if (!other.isTrigger && other.gameObject == target)
        {
            other.gameObject.GetComponent<UnitStats>().TakeDamage(this.damage);
            NetworkServer.Destroy(this.gameObject);
        }
    }
}
