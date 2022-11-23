using Mirror;
using Mirror.Examples.Pong;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSkills : NetworkBehaviour
{
    public GameObject qPrefab;
    private float qCooldown = 2.0f;
    private bool qOnCooldown = false;
    private float qTimer = 0f;

    public GameObject wPrefab;
    private float wCooldown = 0.5f;
    private bool wOnCooldown = false;
    private float wTimer = 0f;

    public GameObject rPrefab;
    private float rCooldown = 2.0f;
    private bool rOnCooldown = false;
    private float rTimer = 0f;


    [Client]
    private void FixedUpdate()
    {
        if (this.qOnCooldown)
        {
            this.qTimer += Time.deltaTime;
        }

        if (this.qCooldown < this.qTimer)
        {
            this.qOnCooldown = false;
            this.qTimer = 0f;
        }

        if (this.wOnCooldown)
        {
            this.wTimer += Time.deltaTime;
        }

        if (this.wCooldown < this.wTimer)
        {
            this.wOnCooldown = false;
            this.wTimer = 0f;
        }

        if (this.rOnCooldown)
        {
            this.rTimer += Time.deltaTime;
        }

        if (this.rCooldown < this.rTimer)
        {
            this.rOnCooldown = false;
            this.rTimer = 0f;
        }
    }

    [ClientCallback]
    public void Skills(InputAction.CallbackContext context)
    {
        if (!hasAuthority)
        {
            return;
        }

        switch (context.control.displayName)
        {
            case "Q":
                this.QSkill();
                break;
            case "W":
                this.CmdWSkill();
                break;
            case "E":
                this.ESkill();
                break;
            case "R":
                this.RSkill();
                break;

        }
    }

    [Client]
    private void QSkill()
    {
        if (this.qOnCooldown)
        {
            return;
        }

        // AOE
        Debug.Log("Q");

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100, Physics.AllLayers, QueryTriggerInteraction.Ignore))
        {
            GameObject go = Instantiate(this.qPrefab, hit.point, Quaternion.identity);
            go.GetComponent<QScript>();
            go.layer = this.gameObject.layer;
        }

        this.qOnCooldown = true;
    }

    [Command]
    private void CmdWSkill()
    {
        this.RpcWSkill();
    }

    [ClientRpc]
    private void RpcWSkill()
    {
        if (this.wOnCooldown)
        {
            return;
        }
        // W kaisy
        Debug.Log("W");

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100, Physics.AllLayers, QueryTriggerInteraction.Ignore))
        {
            GameObject ball = Instantiate(this.wPrefab, transform.position, transform.rotation);
            ball.GetComponent<WProjectileScript>().Direction = (hit.point - this.transform.position).normalized;
            ball.layer = this.gameObject.layer;
        }

        this.wOnCooldown = true;
        // rzucenie czegos i koliduje tylko z danym layerem
    }

    [Client]
    private void ESkill()
    {
        Debug.Log("E");
        // dodac animacje dasha :like:
    }

    [Client]
    private void RSkill()
    {
        // stun
        if (this.rOnCooldown)
        {
            return;
        }

        Debug.Log("R");

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100, Physics.AllLayers, QueryTriggerInteraction.Ignore))
        {
            GameObject ball = Instantiate(this.rPrefab, transform.position, transform.rotation);
            ball.layer = this.gameObject.layer;
            HomingMissileController missile = ball.GetComponent<HomingMissileController>();
            missile.target = hit.transform.gameObject;
            missile.owner = this.gameObject;
            missile.stun = true;
            missile.stunTime = 2.0f;
        }

        this.rOnCooldown = true;
    }
}
