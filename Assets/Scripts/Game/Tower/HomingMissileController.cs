using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingMissileController : MonoBehaviour
{
    public GameObject target;
    public GameObject owner;
    public float damage;
    private float speed = 10f;

    private void Start()
    {
        gameObject.transform.TransformPoint(Vector3.zero);
    }

    private void FixedUpdate()
    {
        if (this.target == null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            gameObject.transform.position += (target.transform.position - gameObject.transform.position).normalized * speed * Time.deltaTime;
            gameObject.transform.LookAt(target.transform);
        }                
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.isTrigger && other.gameObject == target)
        {
            other.gameObject.GetComponent<UnitStats>().TakeDamage(this.damage);
            Destroy(this.gameObject);
        }
    }
}
