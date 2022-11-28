using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;

public class GameResultHold : NetworkBehaviour
{
    [SyncVar] public string gameResult = "";

    void Update() {
        if (gameResult == "") {
            gameResult = NetworkManagerLobby.Instance.whichSideLost;
        }
        
    }
}
