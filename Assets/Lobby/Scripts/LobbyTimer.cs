using System;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;

public class LobbyTimer : NetworkBehaviour
{
    [SyncVar] public float timeLeft = 30f;
    private bool LobbyNotReady = true;
    public static event Action<float> OnTimeUpdated;

    public override void OnStopServer()
    {
        timeLeft = 30f;
        base.OnStopServer();
    }

    void Update()
    {
        if(NetworkManagerLobby.Instance.RoomPlayers.Count > 0)
        {
            if (NetworkManagerLobby.Instance.IsReadyToStart() && LobbyNotReady) {
                timeLeft = 10f;
                LobbyNotReady = false;
            }
            
            if (timeLeft > 0.1) {
                timeLeft -= Time.deltaTime;
                OnTimeUpdated?.Invoke(timeLeft);
            } else {
                timeLeft = 0f;
                OnTimeUpdated?.Invoke(timeLeft);
            }
            if(isServer) {
                if(!LobbyNotReady && timeLeft == 0f) {
                    NetworkManagerLobby.Instance.StartGame();
                } else if (timeLeft == 0f) {
                    NetworkManagerLobby.Instance.OnApplicationQuit();
                }
            }
        }
    }

}
