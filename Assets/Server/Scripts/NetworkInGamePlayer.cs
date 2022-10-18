using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class NetworkInGamePlayer : NetworkBehaviour
{
    [SyncVar]
    public string DisplayName = "Loading...";
    

    private NetworkManagerLobby room;
    private NetworkManagerLobby Room {
        get {
            if (room != null) { return room; }
            return room = NetworkManager.singleton as NetworkManagerLobby;
        }
    }

    public override void OnStartClient()
    {
        DontDestroyOnLoad(gameObject);
        Room.InGamePlayers.Add(this);
    }

    public override void OnStopClient()
    {
        Room.InGamePlayers.Remove(this);
    }

    [Server]
    public void SetDisplayName(string displayName) {
        this.DisplayName = displayName;
    }
}
