using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Mirror;

public class PlayerEvent : UnityEvent<string> { }
public class NetworkManagerLobby : NetworkManager {
    
    [SerializeField] private int minPlayers = 2;
    [Scene] [SerializeField] private string menuScene = string.Empty;

    [SerializeField] private NetworkRoomPlayer roomPlayerPrefab = default;

    [SerializeField] private NetworkInGamePlayer inGamePlayerPrefab = default;
    [SerializeField] private GameObject playerSpawnSystem = default;

    public PlayerEvent OnPlayerAdded = new PlayerEvent();
    public PlayerEvent OnPlayerRemoved = new PlayerEvent();
    
    public static event Action OnClientConnected;
    public static event Action OnClientDisconnected;
    public static event Action<NetworkConnection> OnServerReadied;

    public List<NetworkRoomPlayer> RoomPlayers { get; } = new List<NetworkRoomPlayer>();
    public List<NetworkInGamePlayer> InGamePlayers { get; } = new List<NetworkInGamePlayer>();

    public override void OnStartServer() => spawnPrefabs = Resources.LoadAll<GameObject>("SpawnablePrefabs").ToList();

    public override void OnStartClient() {
        var spawnablePrefabs = Resources.LoadAll<GameObject>("SpawnablePrefabs");

        foreach (var prefab in spawnablePrefabs)
        {
            NetworkClient.RegisterPrefab(prefab);
        }
    }

    public override void OnClientConnect() {
        base.OnClientConnect();

        OnClientConnected?.Invoke();
    }

    public override void OnClientDisconnect() {
        base.OnClientDisconnect();

        OnClientDisconnected?.Invoke();
    }

    public override void OnServerConnect(NetworkConnectionToClient conn) {
        if(numPlayers >= maxConnections) {
            conn.Disconnect();
            return;
        }

        if (SceneManager.GetActiveScene().path != menuScene) {
            conn.Disconnect();
            return;
        }
    }

    public override void OnServerAddPlayer(NetworkConnectionToClient conn) {
        if (SceneManager.GetActiveScene().path == menuScene) {
            bool isLeader = RoomPlayers.Count == 0;

            NetworkRoomPlayer roomPlayerInstance = Instantiate(roomPlayerPrefab);

            roomPlayerInstance.IsLeader = isLeader;

            NetworkServer.AddPlayerForConnection(conn, roomPlayerInstance.gameObject);
        }
    }

    public override void OnServerDisconnect(NetworkConnectionToClient conn) {
        if(conn.identity != null) {
            var player = conn.identity.GetComponent<NetworkRoomPlayer>();

            RoomPlayers.Remove(player);

            NotifyPlayersOfReadyState();
        }

        base.OnServerDisconnect(conn);
    }

    public override void OnStopServer() {
        RoomPlayers.Clear();
    }

    public void NotifyPlayersOfReadyState() {
        foreach (var player in RoomPlayers) {
            player.HandleReadyToStart(IsReadyToStart());
        }
    }

    private bool IsReadyToStart() {
        if (numPlayers < minPlayers) { return false; }

        foreach (var player in RoomPlayers) {
            if (!player.IsReady) { return false; }
        }

        return true;
    }

    public void StartGame() {
        if(SceneManager.GetActiveScene().path == menuScene) {
            if(!IsReadyToStart()) { return; }

            ServerChangeScene("Map");
        }
    }

    public override void ServerChangeScene(string mapName) {
        if(SceneManager.GetActiveScene().path == menuScene) {
            for (int i = RoomPlayers.Count - 1; i >= 0; i--) {
                var conn = RoomPlayers[i].connectionToClient;
                var inGamePlayerInstance = Instantiate(inGamePlayerPrefab);
                inGamePlayerInstance.SetDisplayName(RoomPlayers[i].DisplayName);

                NetworkServer.Destroy(conn.identity.gameObject);
                NetworkServer.ReplacePlayerForConnection(conn, inGamePlayerInstance.gameObject);
            }

            base.ServerChangeScene(mapName);
        }
    }

    public override void OnServerSceneChanged(string newSceneName)
    {
        if(newSceneName.StartsWith("Map")) {
            GameObject playerSpawnSystemInstance = Instantiate(playerSpawnSystem);
            NetworkServer.Spawn(playerSpawnSystemInstance);
        }
    }

    public override void OnServerReady(NetworkConnectionToClient conn)
    {
        base.OnServerReady(conn);

        OnServerReadied?.Invoke(conn);
    }
}
