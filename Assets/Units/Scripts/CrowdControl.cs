using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CrowdControl : NetworkBehaviour
{
    private NavMeshAgent navMesh;
    private Animator animator;
    public bool stunned = false;
    public float stunDuration = 0f;
    public float stunTime = 0f;

    // Start is called before the first frame update
    void Awake()
    {
        this.navMesh = GetComponent<NavMeshAgent>();
        this.animator = GetComponent<Animator>();
    }

    [ServerCallback]
    // Update is called once per frame
    void FixedUpdate()
    {
        if (this.stunned)
        {
            this.stunTime += Time.deltaTime;
        }

        if (this.stunned && this.stunTime > this.stunDuration)
        {
            this.navMesh.isStopped = false;
            this.stunned = false;
            this.animator.SetBool("IsStunned", false);
        }
    }

    [ServerCallback]
    public void Stun(float time)
    {
        if (time > this.stunDuration - this.stunTime)
        {
            this.stunned = true;
            this.stunDuration = time;
            this.navMesh.isStopped = true;
            this.animator.SetBool("IsStunned", true);
        }
    }
}
