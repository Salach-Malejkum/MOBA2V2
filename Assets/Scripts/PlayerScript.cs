using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    private IAttack attackScript;

    // Start is called before the first frame update
    private void Start()
    {
        attackScript = new AttackPlayerScript(500.0f, 0.5f);
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (Input.GetMouseButton(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100))
            {
                int attackResult = this.attackScript.TryAttack(this.transform.position, hit.transform.position);
                // dodac jakie tagi/layery moga byc atackowane, reszta to idziesz w tamtym kierunku
                switch (attackResult)
                {
                    case (int) Enums.AttackResult.Attacked:
                        //Attack
                        break;
                    case (int) Enums.AttackResult.OnCooldown:
                        //CD
                        break;
                    case (int) Enums.AttackResult.OutOfRange:
                        //Move
                        break;
                }
            }
        }
    }
}
