using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingMissile: MonoBehaviour
{
    public GameObject target;
    private float speed = 2f;

    private void Start()
    {
        this.gameObject.transform.TransformPoint(Vector3.zero);
    }

    private void FixedUpdate()
    {
        this.gameObject.transform.position += (target.transform.position - gameObject.transform.position).normalized * speed * Time.deltaTime;
        this.gameObject.transform.LookAt(target.transform);
        //rb.velocity = transform.up * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Babang");
        Debug.Log(other.tag);
        if (other.gameObject == target)
        {
            Debug.Log("Bang");
            Destroy(gameObject);
        }
    }
}