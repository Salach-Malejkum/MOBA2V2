using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class UpgradeManager : NetworkBehaviour
{
    [SerializeField] private GameObject buildingMenuCanva;
    [SerializeField] private GameObject popupResources;
    [SerializeField] private GameObject popupTowerDestroyed;
    [SerializeField] public List<GameObject> buttonsListed;
    [SerializeField] private TMP_Text recourcesText;

    [SyncVar] public GameObject firstTierTurret;
    [SyncVar] public GameObject secondTierTurret;

    private float popupTimer = 0;
    private float popupDuration = 3;

    private void Update()
    {
        this.popupTimer += Time.deltaTime;
        this.recourcesText.text = "Recources: " + GetComponent<PlayerStats>().PlayerResources.ToString();
        if (this.popupTimer >= this.popupDuration && (this.popupTowerDestroyed.activeSelf || this.popupResources.activeSelf))
        {
            this.popupTowerDestroyed.SetActive(false);
            this.popupResources.SetActive(false);
            this.popupTimer = 0f;
        }
    }

    public void SetTurrets()
    {
        PlayerStats playerStats = GetComponent<PlayerStats>();
        string lane = playerStats.playerLane;
        string side = playerStats.playerSide;
        string string_FTT = "1stTierTurret" + lane + side;
        string string_STT = "2ndTierTurret" + lane + side;
        this.firstTierTurret = GameObject.Find(string_FTT);
        this.secondTierTurret = GameObject.Find(string_STT);
    }

    [ClientCallback]
    public void OpenMenu(InputAction.CallbackContext context)
    {
        if (!isOwned)
        {
            return;
        }

        if (context.control.displayName == "O")
        {
            this.ToggleBuildingMenu();
        }
    }

    [Client]
    public void ToggleBuildingMenu()
    {
        Debug.Log(this.buildingMenuCanva.activeSelf);
        if (this.buildingMenuCanva.activeSelf)
        {
            Debug.Log("Goblin mode deactivated");
            this.buildingMenuCanva.SetActive(false);
        }
        else
        {
            Debug.Log("Goblin mode ACTIVATED");
            this.buildingMenuCanva.SetActive(true);
        }
    }

    [Client]
    public void ToggleActivationUpgrade(GameObject gameObject)
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }

    [Client]
    public void UpgradeTurretFFS(string tier_index) // First character is turret tier, second is upgrade Id
    {
        if (this.gameObject.GetComponent<PlayerStats>().PlayerResources >= 20)
        {
            int turretTier = Int32.Parse(tier_index.Substring(0, 1));
            int upgradeIndex = Int32.Parse(tier_index.Substring(tier_index.Length - 1));
            string turretPrefix = "";

            switch (turretTier)
            {
                case 1:
                    if (this.firstTierTurret != null)
                    {
                        turretPrefix = "1st";

                        this.Upgrade1stTier(upgradeIndex);
                    }
                    else
                    {
                        this.popupTowerDestroyed.SetActive(true);
                        return;
                    }
                    break;
                case 2:
                    if (this.secondTierTurret != null)
                    {
                        turretPrefix = "2nd";

                        this.Upgrade2ndTier(upgradeIndex);
                    }
                    else
                    {
                        this.popupTowerDestroyed.SetActive(true);
                        return;
                    }
                    break;
            }

            switch (upgradeIndex)
            {
                case 0:
                case 1:
                    buttonsListed.Where(x => x.name == $"{turretPrefix}TierBase").SingleOrDefault().SetActive(false);
                    break;
                case 2:
                case 3:
                    buttonsListed.Where(x => x.name == $"{turretPrefix}TierDefensive").SingleOrDefault().SetActive(false);
                    break;
                case 4:
                case 5:
                    buttonsListed.Where(x => x.name == $"{turretPrefix}TierOffensive").SingleOrDefault().SetActive(false);
                    break;

            }

            switch (upgradeIndex)
            {
                case 0:
                    buttonsListed.Where(x => x.name == $"{turretPrefix}TierDefensive").SingleOrDefault().SetActive(true);
                    break;
                case 1:
                    buttonsListed.Where(x => x.name == $"{turretPrefix}TierOffensive").SingleOrDefault().SetActive(true);
                    break;
                case 2:
                case 3:
                case 4:
                case 5:
                    buttonsListed.Where(x => x.name == $"{turretPrefix}TierUpgradesDone").SingleOrDefault().SetActive(true);
                    break;

            }
        }
        else
        {
            this.popupResources.SetActive(true);
        }
    }

    [Command]
    public void Upgrade1stTier(int index)
    {
        this.firstTierTurret
            .GetComponent<StructureStats>()
            .ActivateUpgrade(index);

        this.gameObject.GetComponent<PlayerStats>().PlayerResources -= 20f;
    }

    [Command]
    public void Upgrade2ndTier(int index)
    {
        this.secondTierTurret
            .GetComponent<StructureStats>()
            .ActivateUpgrade(index);

        this.gameObject.GetComponent<PlayerStats>().PlayerResources -= 20f;
    }
}
