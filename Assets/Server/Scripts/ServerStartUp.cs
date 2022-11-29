using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.MultiplayerAgent.Model;

public class ServerStartUp : MonoBehaviour
{
    private List<ConnectedPlayer> connectedPlayers;
    public static ServerStartUp singleton { get; private set; }

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        if (singleton == null) {
            singleton = this;
        } else {
            Destroy(this.gameObject);
        }
        if(BuildType.singleton.chosenBuild == BuildType.Build.RemoteServer) {
            StartRemoteServer();
        }
    }

    private void StartRemoteServer() {
        Debug.Log("This is a test log. Please ignore.");
        PlayFabMultiplayerAgentAPI.Start();
        PlayFabMultiplayerAgentAPI.IsDebugging = BuildType.singleton.debugBuild;
		PlayFabMultiplayerAgentAPI.OnShutDownCallback += OnShutdown;
		PlayFabMultiplayerAgentAPI.OnServerActiveCallback += OnServerActive;
		PlayFabMultiplayerAgentAPI.OnAgentErrorCallback += OnAgentError;
        Debug.Log("Ready for players");
        StartCoroutine(ReadyForPlayers());

        Debug.Log("Server starting...");

        NetworkManagerLobby.Instance.OnPlayerAdded.AddListener(OnPlayerAdded);
        NetworkManagerLobby.Instance.OnPlayerRemoved.AddListener(OnPlayerRemoved);
    }

    IEnumerator ReadyForPlayers() {
        yield return new WaitForSeconds(.5f);
        PlayFabMultiplayerAgentAPI.ReadyForPlayers();
    }

    private void OnServerActive() {
        Debug.Log("Server has started");
        NetworkManagerLobby.Instance.StartServer();
    }

    private void OnPlayerRemoved(string playfabId) {
        ConnectedPlayer player = connectedPlayers.Find(x => x.PlayerId.Equals(playfabId, System.StringComparison.OrdinalIgnoreCase));
        connectedPlayers.Remove(player);
        PlayFabMultiplayerAgentAPI.UpdateConnectedPlayers(connectedPlayers);
    }

    private void OnPlayerAdded(string playfabId) {
        connectedPlayers.Add(new ConnectedPlayer(playfabId));
        PlayFabMultiplayerAgentAPI.UpdateConnectedPlayers(connectedPlayers);
    }

    private void OnAgentError(string error) {
        Debug.Log(error);
    }

    private void OnShutdown() {
        Debug.Log("Server shutting down");
        StartCoroutine(Shutdown());
    }

    IEnumerator Shutdown() {
        yield return new WaitForSeconds(.5f);
        Application.Quit();
    }
}
