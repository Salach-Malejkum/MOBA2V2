using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mirror;

public class NetworkRoomPlayer : NetworkBehaviour
{
    [SerializeField] private GameObject lobbyUI = null;
    [SerializeField] private TMP_Text[] playerNameTexts = new TMP_Text[4];
    [SerializeField] private TMP_Text[] playerReadyTexts = new TMP_Text[4];
    [SerializeField] private Button startGameButton = null;

    [SyncVar(hook = nameof(HandleReadyStatusChanged))]
    public bool IsReady = false;
    [SyncVar]
    public string DisplayName = "Loading...";
    
    private bool isLeader;
    public bool IsLeader {
        set {
            isLeader = value;
            startGameButton.gameObject.SetActive(value);
        }
    }

    private NetworkManagerLobby room;
    private NetworkManagerLobby Room {
        get {
            if (room != null) { return room; }
            return room = NetworkManager.singleton as NetworkManagerLobby;
        }
    }

    public override void OnStartAuthority() {
        CmdSetDisplayName("PlaceholderName");
        lobbyUI.SetActive(true);
    }

    public override void OnStartClient()
    {
        Room.RoomPlayers.Add(this);

        UpdateDisplay();
    }

    public override void OnStopClient()
    {
        Room.RoomPlayers.Remove(this);

        UpdateDisplay();
    }

    public void HandleReadyStatusChanged(bool oldVal, bool newVal) => UpdateDisplay();

    private void UpdateDisplay() {
        for(int i = 0; i < playerNameTexts.Length; i++) {
            playerNameTexts[i].text = "Waiting for player...";
            playerReadyTexts[i].text = string.Empty;
        }

        for(int i = 0; i < Room.RoomPlayers.Count; i++) {
            playerNameTexts[i].text = Room.RoomPlayers[i].DisplayName;
            playerReadyTexts[i].text = Room.RoomPlayers[i].IsReady ? "Ready" : "Not ready";
        }

        if (!hasAuthority) {
            foreach(var player in Room.RoomPlayers) {
                int i = 0;
                if (player.hasAuthority) {
                    player.UpdateDisplay();
                    player.DisplayName = "Player" + i.ToString();
                    i++;
                    break;
                }
            }
            return;
        }
    }

    public void HandleReadyToStart(bool readyToStart) {
        if (!isLeader) { return; }

        startGameButton.interactable = readyToStart;
    }

    [Command]
    private void CmdSetDisplayName(string displayName) {
        DisplayName = displayName;
    }

    [Command]
    public void CmdReadyUp() {
        IsReady = !IsReady;
        Room.NotifyPlayersOfReadyState();
    }

    [Command]
    public void CmdStartGame() {
        if (Room.RoomPlayers[0].connectionToClient != connectionToClient) { return; }

        Room.StartGame();
    }

}
