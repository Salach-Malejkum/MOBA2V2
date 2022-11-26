using Mirror;
using Mirror.Examples.Pong;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WProjectileScript : NetworkBehaviour
{
    private float damage = 200f;
    private float traveledDist = 0f;
    private readonly float distLimit = 100f;
    private float speed = 0.5f;
    private Vector3 direction;
    public Vector3 Direction
    {
        get { return direction; }
        set { direction = value; }
    }

    [ServerCallback]
    void FixedUpdate()
    {
        this.transform.Translate(this.direction.x * this.speed, 0f, this.direction.z * this.speed, Space.World);
        this.traveledDist += this.direction.magnitude * this.speed;

        if (this.traveledDist > this.distLimit)
        {
            Destroy(this.gameObject);
        }
    }

    [Server]
    private void OnTriggerEnter(Collider other)
    {
        if (!other.isTrigger)
        {
            switch (this.gameObject.layer)
            {
                case Enums.Layers.blueTeamLayer:
                    if (other.gameObject.layer == Enums.Layers.redTeamLayer)
                    {
                        Debug.Log(other.gameObject.name + " " + LayerMask.LayerToName(other.gameObject.layer));
                        other.gameObject.GetComponent<UnitStats>().RemoveHealthOnNormalAttack(this.damage);
                        Destroy(this.gameObject);
                    }
                    break;
                case Enums.Layers.redTeamLayer:
                    if (other.gameObject.layer == Enums.Layers.blueTeamLayer)
                    {
                        Debug.Log(other.gameObject.name + " " + LayerMask.LayerToName(other.gameObject.layer));
                        other.gameObject.GetComponent<UnitStats>().RemoveHealthOnNormalAttack(this.damage);
                        Destroy(this.gameObject);
                    }
                    break;
            }
        }
    }
}
