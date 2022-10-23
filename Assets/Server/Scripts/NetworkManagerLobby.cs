using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using PlayFab;
using PlayFab.MultiplayerAgent.Model;
using Mirror;


public class NetworkManagerLobby : NetworkManager {

    [SerializeField] private int minPlayers = 2;
    [Scene] [SerializeField] private string menuScene = string.Empty;
    public string connType = "remote";

    [SerializeField] private NetworkRoomPlayer roomPlayerPrefab = default;

    [SerializeField] private NetworkInGamePlayer inGamePlayerPrefab = default;
    [SerializeField] private GameObject playerSpawnSystem = default;

    public static NetworkManagerLobby Instance { get; private set; }

    public PlayerEvent OnPlayerAdded = new PlayerEvent();
    public PlayerEvent OnPlayerRemoved = new PlayerEvent();
    
    public class PlayerEvent : UnityEvent<string> { }
    
    public static event Action OnClientConnected;
    public static event Action OnClientDisconnected;
    public static event Action<NetworkConnectionToClient> OnServerReadied;

    public List<NetworkRoomPlayer> RoomPlayers = new List<NetworkRoomPlayer>();
    public List<NetworkInGamePlayer> InGamePlayers = new List<NetworkInGamePlayer>();
    public List<PlayerConnection> playerConnections = new List<PlayerConnection>();

    public override void Awake()
    {
        base.Awake();
        Instance = this;
        NetworkServer.RegisterHandler<AuthenticateMessage>(OnReceiveAuthenticateMessage);
    }

    private void OnReceiveAuthenticateMessage(NetworkConnection nconn, AuthenticateMessage message) {
        Debug.Log("Authenticate message received--------------------------------PlayfabId: " + message.PlayfabId);
        var conn = playerConnections.Find(c => c.ConnectionId == nconn.connectionId);
        if(conn == null && connType == "remote") {
            conn.PlayfabId = message.PlayfabId;
            conn.Authenticated = true;
            OnPlayerAdded.Invoke(message.PlayfabId);
        }
    }

    //Region: ServerCode
    public override void OnStartServer() => spawnPrefabs = Resources.LoadAll<GameObject>("SpawnablePrefabs").ToList();
<<<<<<< HEAD
    //możliwy podział OnStartClient, OnClientConnect i OnClientDisconnect do osobnego NetworkManagera
    //komunikacja zamiast Event przy użyciu messages
        public override void OnStartClient() {
            var spawnablePrefabs = Resources.LoadAll<GameObject>("SpawnablePrefabs");
=======

    public override void OnStartClient() {
        var spawnablePrefabs = Resources.LoadAll<GameObject>("SpawnablePrefabs");
>>>>>>> parent of 5ff4675 (added comments)

            foreach (var prefab in spawnablePrefabs)
            {
                NetworkClient.RegisterPrefab(prefab);
            }
        }
<<<<<<< HEAD
    
    //Ewentualnie do zmiany Invoke jeżeli będzie podzial NetworkManagera na klienta i serwer
=======
    }

>>>>>>> parent of 5ff4675 (added comments)
    public override void OnClientConnect() {
        Debug.Log("Client connected-------------------------");
        base.OnClientConnect();
        OnClientConnected?.Invoke();
    }

    public override void OnClientDisconnect() {
        base.OnClientDisconnect();

        OnClientDisconnected?.Invoke();
    }

    public override void OnServerConnect(NetworkConnectionToClient conn) {
        Debug.Log("Client connected-------------------------");
        if(numPlayers >= maxConnections) {
            conn.Disconnect();
            return;
        }

        if (SceneManager.GetActiveScene().path != menuScene) {
            conn.Disconnect();
            return;
        }

        var playerConn = playerConnections.Find(c => c.ConnectionId == conn.connectionId);
<<<<<<< HEAD
        //customowe atrybuty połączenia do listy połączeń
        if(playerConn == null && connType == "remote") {
=======

        if(playerConn == null) {
>>>>>>> parent of 5ff4675 (added comments)
            playerConnections.Add(new PlayerConnection() {
                Connection = conn,
                ConnectionId = conn.connectionId,
                LobbyId = PlayFabMultiplayerAgentAPI.SessionConfig.SessionId
            });
        }
    }
    
    public override void OnServerAddPlayer(NetworkConnectionToClient conn) {
        Debug.Log("Player added on server------------------------- Scene: " + SceneManager.GetActiveScene().path);
        if (SceneManager.GetActiveScene().path == menuScene) {

            NetworkRoomPlayer roomPlayerInstance = Instantiate(roomPlayerPrefab);

            NetworkServer.AddPlayerForConnection(conn, roomPlayerInstance.gameObject);
<<<<<<< HEAD
            if(connType == "remote") {
                RoomPlayers.Add(conn.identity.GetComponent<NetworkRoomPlayer>());
            }
            
=======

            RoomPlayers.Add(conn.identity.GetComponent<NetworkRoomPlayer>());

>>>>>>> parent of 5ff4675 (added comments)
            Debug.Log("Player added for connection, player count-------------------------" + Instance.RoomPlayers.Count.ToString());
            Debug.Log("List of players-------------------------");
            foreach(NetworkRoomPlayer rplayer in Instance.RoomPlayers) {
                Debug.Log(rplayer.DisplayName);
            }
        }
    }

    public override void OnServerDisconnect(NetworkConnectionToClient conn) {
        if(conn.identity != null) {
            var player = conn.identity.GetComponent<NetworkRoomPlayer>();

            RoomPlayers.Remove(player);

            NotifyPlayersOfReadyState();
        }
        var playerConn = playerConnections.Find(c => c.ConnectionId == conn.connectionId);

        if(playerConn != null && connType == "remote") {
            if(!string.IsNullOrEmpty(playerConn.PlayfabId)) {
                OnPlayerRemoved.Invoke(playerConn.PlayfabId);
            }
            playerConnections.Remove(playerConn);
        }
        base.OnServerDisconnect(conn);
    }

    public override void OnStopServer() {
        Instance.RoomPlayers.Clear();
    }

    public void NotifyPlayersOfReadyState() {
        foreach (var player in Instance.RoomPlayers) {
            player.HandleReadyToStart(IsReadyToStart());
        }
    }

    private bool IsReadyToStart() {
        if (numPlayers < minPlayers) { return false; }

        foreach (var player in Instance.RoomPlayers) {
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
            for (int i = Instance.RoomPlayers.Count - 1; i >= 0; i--) {
                var conn = Instance.RoomPlayers[i].connectionToClient;
                var inGamePlayerInstance = Instantiate(inGamePlayerPrefab);
                inGamePlayerInstance.SetDisplayName(Instance.RoomPlayers[i].DisplayName);
                Debug.Log("Room player " + Instance.RoomPlayers[i].DisplayName + " changed to inGamePlayer ");
                NetworkServer.Destroy(conn.identity.gameObject);
                NetworkServer.ReplacePlayerForConnection(conn, inGamePlayerInstance.gameObject);
                if(connType == "remote") {
                    NetworkManagerLobby.Instance.InGamePlayers.Add(conn.identity.GetComponent<NetworkInGamePlayer>());
                }
                
            }

            base.ServerChangeScene(mapName);
        }
    }

    public override void OnServerSceneChanged(string newSceneName)
    {
        //jest blad po stronie clienta ze niby nie ma tego obiektu, ale on sie respi po stronie serwera i nie ma z tym problemów bo respi graczy normalnie
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

[Serializable]
public class PlayerConnection {
    public bool Authenticated;
    public string PlayfabId;
    public string LobbyId;
    public int ConnectionId;
    public NetworkConnection Connection;
}

public struct AuthenticateMessage : NetworkMessage {
    public string PlayfabId;
}
