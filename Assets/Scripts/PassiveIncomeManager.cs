using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveIncomeManager : NetworkBehaviour
{
    float timePassed = 1f;
    public float goldIncome = 2f;

    [SyncVar] public float resourceIncomeFromCampsBlue = 0f;
    [SyncVar] public float resourceIncomeFromCampsRed = 0f;

    [ServerCallback]
    void Update()
    {
        this.timePassed += Time.deltaTime;

        if (this.timePassed > 1f)
        {
            this.timePassed = 0f;
            foreach(NetworkIdentity conn in NetworkManagerLobby.Instance.PlayersLoadedToScene)
            {
                PlayerStats stats = conn.gameObject.GetComponent<PlayerStats>();
                switch (LayerMask.LayerToName(conn.gameObject.layer))
                {
                    case "Blue":
                        stats.PlayerResources += this.resourceIncomeFromCampsBlue;
                        break;
                    case "Red":
                        stats.PlayerResources += this.resourceIncomeFromCampsRed;
                        break;
                }
                stats.PlayerGold += this.goldIncome;
                stats.AddHealth(stats.PlayerHealthRegen);
            }
        }
    }
}
