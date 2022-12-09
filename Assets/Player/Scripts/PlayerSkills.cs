using Mirror;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;

public class PlayerSkills : NetworkBehaviour
{
    public PlayerStats stats;

    public GameObject qPrefab;
    private float qCooldownBase = 8.0f;
    private float qCooldown;
    private bool qOnCooldown = false;
    private float qTimer = 0f;

    public GameObject wPrefab;
    private Vector3 wDirection;
    private float wCooldownBase = 5f;
    private float wCooldown;
    private bool wOnCooldown = false;
    private float wTimer = 0f;

    private float eCooldownBase = 10.0f;
    private float eCooldown;
    private bool eOnCooldown = false;
    private float eTimer = 0f;

    public GameObject rPrefab;
    private GameObject rTarget;
    private float rCooldownBase = 10.0f;
    private float rCooldown;
    private bool rOnCooldown = false;
    private float rTimer = 0f;

    [SerializeField]
    private LayerMask attackableLayer;
    private NetworkAnimator networkAnimator;

    public event Action QUsed;
    public event Action WUsed;
    public event Action EUsed;
    public event Action RUsed;
    public event Action QRedy;
    public event Action WRedy;
    public event Action ERedy;
    public event Action RRedy;

    private void Start()
    {
        this.stats = GetComponent<PlayerStats>();
        this.networkAnimator = GetComponent<NetworkAnimator>();
    }


    [ClientCallback]
    private void FixedUpdate()
    {

        this.qCooldown = this.qCooldownBase - this.qCooldownBase * this.stats.UnitCooldownReduction / 100;
        this.wCooldown = this.wCooldownBase - this.wCooldownBase * this.stats.UnitCooldownReduction / 100;
        this.eCooldown = this.eCooldownBase - this.eCooldownBase * this.stats.UnitCooldownReduction / 100;
        this.rCooldown = this.rCooldownBase - this.rCooldownBase * this.stats.UnitCooldownReduction / 100;

        if (this.attackableLayer == 0)
        {
            this.attackableLayer = GetComponent<PlayerAttack>().AttackableLayer;
        }

        if (this.qOnCooldown)
        {
            this.qTimer += Time.deltaTime;
        }

        if (this.qCooldown < this.qTimer)
        {
            this.qOnCooldown = false;
            this.qTimer = 0f;
            this.QRedy?.Invoke();
        }

        if (this.wOnCooldown)
        {
            this.wTimer += Time.deltaTime;
        }

        if (this.wCooldown < this.wTimer)
        {
            this.wOnCooldown = false;
            this.wTimer = 0f;
            this.WRedy?.Invoke();
        }

        if (this.eOnCooldown)
        {
            this.eTimer += Time.deltaTime;
        }

        if (this.eCooldown < this.eTimer)
        {
            Debug.Log("E timer: " + this.eTimer);
            Debug.Log("Expected timer: " + this.eCooldown);
            Debug.Log("Base timer: " + this.eCooldownBase);
            this.eOnCooldown = false;
            this.eTimer = 0f;
            this.ERedy?.Invoke();
        }

        if (this.rOnCooldown)
        {
            this.rTimer += Time.deltaTime;
        }

        if (this.rCooldown < this.rTimer)
        {
            this.rOnCooldown = false;
            this.rTimer = 0f;
            this.RRedy?.Invoke();
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
                this.EUsed?.Invoke();
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
        this.QUsed?.Invoke();

        this.networkAnimator.SetTrigger("QSkill");

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100, Physics.AllLayers, QueryTriggerInteraction.Ignore))
        {
            this.transform.LookAt(hit.point);
            this.CmdQSkill(hit.point, this.gameObject, this.attackableLayer);
        }
    }

    [Command]
    private void CmdQSkill(Vector3 point, GameObject owner, int attackableLayer)
    {
        GameObject go = Instantiate(this.qPrefab, new Vector3(point.x, 0.5f, point.z), Quaternion.identity);
        QScript qScript = go.GetComponent<QScript>();
        qScript.apMultipliers += this.stats.UnitAbilityPower;
        go.layer = owner.layer;
        qScript.Owner = owner;
        qScript.AttackableLayer = attackableLayer;
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
        this.WUsed?.Invoke();

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
        if (!isOwned)
        {
            return;
        }

        this.CmdWSkill(this.wDirection, this.gameObject, this.attackableLayer);
    }

    [Command]
    private void CmdWSkill(Vector3 direction, GameObject owner, int attackableLayer)
    {
        GameObject ball = Instantiate(this.wPrefab, transform.position, transform.rotation);
        WProjectileScript wProjectileScript = ball.GetComponent<WProjectileScript>();
        wProjectileScript.apMultipliers += this.stats.UnitAbilityPower;
        wProjectileScript.Direction = direction;
        wProjectileScript.Owner = owner;
        wProjectileScript.AttackableLayer = attackableLayer;
        ball.layer = owner.layer;
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
            this.stats.UsedESkill();
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

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100, this.attackableLayer, QueryTriggerInteraction.Ignore))
        {
            this.RUsed?.Invoke();
            this.transform.LookAt(hit.point);
            this.networkAnimator.SetTrigger("RSkill");
            this.rTarget = hit.transform.gameObject;
            this.rOnCooldown = true;
        }
    }

    [ClientCallback]
    public void RSkill()
    {
        if (!isOwned)
        {
            return;
        }

        this.CmdRSkill(this.rTarget, this.gameObject);
    }

    [Command]
    private void CmdRSkill(GameObject target, GameObject owner) 
    {
        GameObject ball = Instantiate(this.rPrefab, transform.position + 2 * Vector3.up, transform.rotation);
        ball.layer = owner.layer;
        HomingMissileController missile = ball.GetComponent<HomingMissileController>();
        missile.target = target;
        missile.owner = owner;
        missile.stun = true;
        missile.stunTime = 3.0f;
        missile.damage = 50f + (this.stats.UnitAttackDamage / 5) + (this.stats.UnitAbilityPower);
        NetworkServer.Spawn(ball);
    }
}
