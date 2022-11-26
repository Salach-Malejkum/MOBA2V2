using Mirror;
using Mirror.Examples.Pong;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;

public class PlayerSkills : NetworkBehaviour
{
    public GameObject qPrefab;
    private float qCooldown = 2.0f;
    private bool qOnCooldown = false;
    private float qTimer = 0f;

    public GameObject wPrefab;
    public Vector3 wDirection;
    private float wCooldown = 0.5f;
    private bool wOnCooldown = false;
    private float wTimer = 0f;

    private float eCooldown = 2.0f;
    private bool eOnCooldown = false;
    private float eTimer = 0f;

    public GameObject rPrefab;
    private GameObject rTarget;
    private float rCooldown = 2.0f;
    private bool rOnCooldown = false;
    private float rTimer = 0f;

    private LayerMask attackableLayer;
    private NetworkAnimator networkAnimator;

    private void Start()
    {
        this.AssignAttackableLayer();
        this.networkAnimator = GetComponent<NetworkAnimator>();
    }


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

        if (this.eOnCooldown)
        {
            this.eTimer += Time.deltaTime;
        }

        if (this.eCooldown < this.eTimer)
        {
            this.eOnCooldown = false;
            this.eTimer = 0f;
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
                this.RSkillAnim();
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
        this.qOnCooldown = true;

        this.networkAnimator.SetTrigger("QSkill");

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100, Physics.AllLayers, QueryTriggerInteraction.Ignore))
        {
            this.transform.LookAt(hit.point);
            GameObject go = Instantiate(this.qPrefab, hit.point, Quaternion.identity);
            go.GetComponent<QScript>();
            go.layer = this.gameObject.layer;
        }
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

        this.wOnCooldown = true;

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100, Physics.AllLayers, QueryTriggerInteraction.Ignore))
        {
            this.wDirection = (hit.point - this.transform.position).normalized;
            this.transform.LookAt(hit.point);
            this.networkAnimator.SetTrigger("WSkill");
        }
        // rzucenie czegos i koliduje tylko z danym layerem
    }

    public void WSkill()
    {
        GameObject ball = Instantiate(this.wPrefab, transform.position, transform.rotation);
        ball.GetComponent<WProjectileScript>().Direction = this.wDirection;
        ball.layer = this.gameObject.layer;
    }

    [Client]
    public void ESkill()
    {
        if (this.eOnCooldown)
        {
            return;
        }

        Debug.Log("E");

        this.eOnCooldown = true;

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100, Physics.AllLayers, QueryTriggerInteraction.Ignore))
        {
            this.transform.LookAt(hit.point);
            this.networkAnimator.SetTrigger("ESkill");
        }
        // dodac animacje dasha :like:
    }

    [Client]
    private void RSkillAnim()
    {
        // stun
        if (this.rOnCooldown)
        {
            return;
        }

        Debug.Log("R");

        this.rOnCooldown = true;

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100, this.attackableLayer, QueryTriggerInteraction.Ignore))
        {
            this.transform.LookAt(hit.point);
            this.networkAnimator.SetTrigger("RSkill");
            this.rTarget = hit.transform.gameObject;
        }
    }

    public void RSkill()
    {
        GameObject ball = Instantiate(this.rPrefab, transform.position + 2 * Vector3.up, transform.rotation);
        ball.layer = this.gameObject.layer;
        HomingMissileController missile = ball.GetComponent<HomingMissileController>();
        missile.target =this.rTarget;
        missile.owner = this.gameObject;
        missile.stun = true;
        missile.stunTime = 2.0f;
    }

    private void AssignAttackableLayer()
    {
        LayerMask enemyTeamLayer = this.gameObject.layer;
        switch (enemyTeamLayer)
        {
            case Enums.Layers.blueTeamLayer:
                enemyTeamLayer = 1 << Enums.Layers.redTeamLayer;
                break;
            case Enums.Layers.redTeamLayer:
                enemyTeamLayer = 1 << Enums.Layers.blueTeamLayer;
                break;
        }
        LayerMask neutralLayer = 1 << Enums.Layers.neutral;
        this.attackableLayer = neutralLayer | enemyTeamLayer;
    }
}
