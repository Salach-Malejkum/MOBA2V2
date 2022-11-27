using Mirror;
using Telepathy;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;

public class PlayerSkills : NetworkBehaviour
{
    public GameObject qPrefab;
    private Vector3 qPoint;
    private float qCooldown = 2.0f;
    private bool qOnCooldown = false;
    private float qTimer = 0f;

    public GameObject wPrefab;
    private Vector3 wDirection;
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


    [ClientCallback]
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


    public void Skills(InputAction.CallbackContext context)
    {
        if (!isLocalPlayer)
        {
            return;
        }

        switch (context.control.displayName)
        {
            case "Q":
                this.QSkill();
                break;
            case "W":
                this.ClientWSkill();
                break;
            case "E":
                this.ESkill();
                break;
            case "R":
                this.RSkillAnim();
                break;

        }
    }

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
            this.CmdQSkill(hit.point, this.gameObject.layer, this.gameObject);
        }
    }

    [Command]
    private void CmdQSkill(Vector3 point, LayerMask layer, GameObject owner)
    {
        GameObject go = Instantiate(this.qPrefab, point, Quaternion.identity);
        QScript qScript = go.GetComponent<QScript>();
        go.layer = layer;
        qScript.Owner = owner;
        NetworkServer.Spawn(go);
    }


    private void ClientWSkill()
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
        this.CmdWSkill(this.wDirection, this.gameObject);
    }

    [Command]
    private void CmdWSkill(Vector3 direction, GameObject owner)
    {
        GameObject ball = Instantiate(this.wPrefab, transform.position, transform.rotation);
        WProjectileScript wProjectileScript = ball.GetComponent<WProjectileScript>();
        wProjectileScript.Direction = direction;
        wProjectileScript.Owner = owner;
        ball.layer = this.gameObject.layer;
        NetworkServer.Spawn(ball);
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

    [ClientCallback]
    public void RSkill()
    {
        if (!isOwned)
        {
            return;
        }

        this.CmdRSkill(this.gameObject.layer, this.rTarget, this.gameObject);
    }

    [Command]
    private void CmdRSkill(LayerMask layer, GameObject target, GameObject owner) 
    {
        GameObject ball = Instantiate(this.rPrefab, transform.position + 2 * Vector3.up, transform.rotation);
        ball.layer = layer;
        HomingMissileController missile = ball.GetComponent<HomingMissileController>();
        missile.target = target;
        missile.owner = owner;
        missile.stun = true;
        missile.stunTime = 2.0f;
        NetworkServer.Spawn(ball);
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