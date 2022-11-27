using System;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;

public class LobbyTimer : NetworkBehaviour
{
    [SyncVar] private float timeLeft = 5f;
    private bool LobbyNotReady = true;
    public static event Action<float> OnTimeUpdated;

    [ServerCallback]
    void Update()
    {
        if(NetworkManagerLobby.Instance.RoomPlayers.Count > 0)
        {
            if (NetworkManagerLobby.Instance.IsReadyToStart() && LobbyNotReady) {
                timeLeft = 10f;
                LobbyNotReady = false;
            }
            
            if (timeLeft > 0) {
                timeLeft -= Time.deltaTime;
                OnTimeUpdated?.Invoke(timeLeft);
            } else {
                timeLeft = 0f;
                OnTimeUpdated?.Invoke(timeLeft);
            }

            if(!LobbyNotReady && timeLeft == 0f) {
                NetworkManagerLobby.Instance.StartGame();
            } else if (timeLeft == 0f) {
                NetworkManagerLobby.Instance.OnApplicationQuit();
            }
        }
    }

}
