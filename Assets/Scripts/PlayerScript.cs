using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    private IAttack attackScript;
    // Start is called before the first frame update
    void Start()
    {
        attackScript = new AttackPlayerScript(500.0f, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        this.attackScript.Attack(this.transform.position);
    }
}
