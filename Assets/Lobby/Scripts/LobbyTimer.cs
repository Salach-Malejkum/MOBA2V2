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
        this.timeLeft = 30f;
        base.OnStopServer();
    }

    void Update()
    {
        if (NetworkManagerLobby.Instance.RoomPlayers.Count >= NetworkManagerLobby.Instance.MinPlayers)
        {
            if (NetworkManagerLobby.Instance.IsReadyToStart() && LobbyNotReady) {
                this.timeLeft = 10f;
                LobbyNotReady = false;
            }
            
            if (timeLeft > 0.1) {
                this.timeLeft -= Time.deltaTime;
                OnTimeUpdated?.Invoke(timeLeft);
            } else {
                this.timeLeft = 0f;
                OnTimeUpdated?.Invoke(timeLeft);
            }
            if (isServer) {
                if (!LobbyNotReady && timeLeft == 0f) {
                    NetworkManagerLobby.Instance.StartGame();
                } else if (timeLeft == 0f) {
                    if (NetworkManagerLobby.Instance.connType == "remoteClient") {
                        NetworkManagerLobby.Instance.StopClient();
                    } else {
                        Application.Quit();
                    }
                }
            }
        }
    }

}
