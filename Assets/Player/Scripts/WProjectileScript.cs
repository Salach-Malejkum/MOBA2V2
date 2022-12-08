using Mirror;
using UnityEngine;

public class WProjectileScript : NetworkBehaviour
{
    private float damage = 200f;
    private float traveledDist = 0f;
    private readonly float distLimit = 100f;
    private float speed = 0.5f;

    public float apMultipliers = 100f;

    [SerializeField]
    private Vector3 direction;
    public Vector3 Direction
    {
        get { return direction; }
        set { direction = value; }
    }
    [SerializeField]
    private GameObject owner;
    public GameObject Owner
    {
        get { return owner; }
        set { owner = value; }
    }
    [SerializeField]
    private LayerMask attackableLayer;
    public LayerMask AttackableLayer
    {
        get { return attackableLayer; }
        set { attackableLayer = value; }
    }


    [ServerCallback]
    void FixedUpdate()
    {
        this.transform.Translate(this.direction.x * this.speed, 0f, this.direction.z * this.speed, Space.World);
        this.traveledDist += this.direction.magnitude * this.speed;

        if (this.traveledDist > this.distLimit)
        {
            NetworkServer.Destroy(this.gameObject);
        }
    }

    [ServerCallback]
    private void OnTriggerEnter(Collider other)
    {
        if (!other.isTrigger && (this.attackableLayer == (this.attackableLayer | (1 << other.gameObject.layer))))
        {
            other.gameObject.GetComponent<UnitStats>().RemoveHealthOnNormalAttack((this.damage * this.apMultipliers) / 100, this.Owner);
            NetworkServer.Destroy(this.gameObject);
        }
    }
}
