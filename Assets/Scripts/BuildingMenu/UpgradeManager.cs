using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class UpgradeManager : NetworkBehaviour
{
    [SerializeField]
    private GameObject buildingMenuCanva;

    public GameObject firstTierTurret;
    public GameObject secondTierTurret;
    public bool found = false;

    public void SetTurrets()
    {
        PlayerStats playerStats = GetComponent<PlayerStats>();
        string lane = playerStats.lane;
        string side = playerStats.side;
        string string_FTT = "1stTierTurret" + lane + side;
        string string_STT = "2ndTierTurret" + lane + side;
        this.firstTierTurret = GameObject.Find(string_FTT);
        this.secondTierTurret = GameObject.Find(string_STT);
    }

    [ClientCallback]
    public void OpenMenu(InputAction.CallbackContext context)
    {
        if (!hasAuthority)
        {
            return;
        }

        if (context.control.displayName == "O")
        {
            ToggleBuildingMenu();
        }
    }

    [Client]
    public void ToggleBuildingMenu()
    {
        if (this.buildingMenuCanva.activeSelf)
        {
            this.buildingMenuCanva.SetActive(false);
        }
        else
        {
            this.buildingMenuCanva.SetActive(true);
        }
    }

    [Client]
    public void ToggleActivationUpgrade(GameObject gameObject)
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }

    [Command]
    public void Upgrade1stTier(int index)
    {
        firstTierTurret.GetComponent<StructureStats>().ActivateUpgrade(index);
    }

    [Command]
    public void Upgrade2ndTier(int index)
    {
        secondTierTurret.GetComponent<StructureStats>().ActivateUpgrade(index);
    }
}
