using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingMissileController : MonoBehaviour
{
    public GameObject target;
    private float speed = 15f;

    private void Start()
    {
        gameObject.transform.TransformPoint(Vector3.zero);
    }

    private void FixedUpdate()
    {
        gameObject.transform.position += (target.transform.position - gameObject.transform.position).normalized * speed * Time.deltaTime;
        gameObject.transform.LookAt(target.transform);
        //rb.velocity = transform.up * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        UnityEngine.Debug.Log("Babang");
        UnityEngine.Debug.Log(other.tag);
        if (other.gameObject == target)
        {
            UnityEngine.Debug.Log("Bang");
            Destroy(gameObject);
        }
    }
}
