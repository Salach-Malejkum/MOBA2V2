using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveIncomeManager : NetworkBehaviour
{
    float timePassed = 1f;
    public float goldIncome = 2f;

    [SyncVar] public List<GameObject> playersBlue;
    [SyncVar] public List<GameObject> playersRed;
    [SyncVar] public float resourceIncomeFromCampsBlue = 0f;
    [SyncVar] public float resourceIncomeFromCampsRed = 0f;

    [Server]
    void Awake()
    {
        this.playersBlue = new List<GameObject>();
        this.playersRed = new List<GameObject>();
    }

    [Server]
    void Update()
    {
        this.timePassed += Time.deltaTime;

        if (this.timePassed > 1f)
        {
            this.timePassed = 0f;
            foreach (GameObject p in this.playersBlue)
            {
                p.GetComponent<PlayerStats>().PlayerResources += this.resourceIncomeFromCampsBlue;
                p.GetComponent<PlayerStats>().PlayerGold += this.goldIncome;
            }
            foreach (GameObject p in this.playersRed)
            {
                p.GetComponent<PlayerStats>().PlayerResources += this.resourceIncomeFromCampsRed;
                p.GetComponent<PlayerStats>().PlayerGold += this.goldIncome;
            }
        }
    }

    public void AddPlayer(GameObject player)
    {
        switch (player.GetComponent<PlayerStats>().playerSide)
        {
            case "Blue":
                this.playersBlue.Add(player);
                break;
            case "Red":
                this.playersRed.Add(player);
                break;
        }
    }
}
