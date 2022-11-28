using Mirror;
using UnityEngine;

public class QScript : NetworkBehaviour
{
    private float damage = 200f;
    private float timer = 0f;
    private float castTime = 0.5f;
    [SerializeField]
    private LayerMask attackableLayer;
    public LayerMask AttackableLayer
    {
        get { return attackableLayer; }
        set { attackableLayer = value; }
    }

    private GameObject owner;
    public GameObject Owner
    {
        get { return owner; }
        set { owner = value; }
    }
        
    [ServerCallback]
    void FixedUpdate()
    {
        this.timer += Time.deltaTime;

        if (this.timer >= this.castTime)
        {
            this.DealDamage();
            NetworkServer.Destroy(this.gameObject);
        }
    }

    [ServerCallback]
    private void DealDamage()
    {
        Collider[] hitColliders = Physics.OverlapBox(this.transform.position, transform.localScale / 2, Quaternion.identity, this.attackableLayer, QueryTriggerInteraction.Ignore);
        for (int i = 0; i < hitColliders.Length; i++)
        {
            hitColliders[i].gameObject.GetComponent<UnitStats>().RemoveHealthOnNormalAttack(this.damage, this.owner);
        }
    }
}
