using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerConnection : NetworkBehaviour
{
    public static PlayerConnection localPlayerConnection;

    void Start() {
        if(isLocalPlayer) {
            localPlayerConnection = this;
        }
    }

    public void HostGame() {
        string matchID = Matchmaking.GenerateMatchID();
        CmdHostGame(matchID);
    }

    [Command]
    void CmdHostGame (string matchID) {
        if (Matchmaking.instance.HostGame()) {
            Debug.Log("Hosted");
        } else {
            Debug.Log("Failed to host");
        }
    }
}
