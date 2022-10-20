using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.MultiplayerAgent.Model;
using Mirror;

public class ServerStartUp : MonoBehaviour
{
    [SerializeField] private BuildType buildType;
    [SerializeField] private NetworkManagerLobby networkManager;

    private List<ConnectedPlayer> connectedPlayers;

    void Start()
    {
        if(buildType.chosenBuild == BuildType.Build.RemoteServer) {
            StartRemoteServer();
        }
    }

    private void StartRemoteServer() {
        PlayFabMultiplayerAgentAPI.Start();
        PlayFabMultiplayerAgentAPI.IsDebugging = buildType.debugBuild;
		PlayFabMultiplayerAgentAPI.OnShutDownCallback += OnShutdown;
		PlayFabMultiplayerAgentAPI.OnServerActiveCallback += OnServerActive;
		PlayFabMultiplayerAgentAPI.OnAgentErrorCallback += OnAgentError;

        networkManager.OnPlayerAdded.AddListener(OnPlayerAdded);
        networkManager.OnPlayerRemoved.AddListener(OnPlayerRemoved);

        networkManager.StartServer();

        StartCoroutine(ReadyForPlayers());
    }

    IEnumerator ReadyForPlayers() {
        yield return new WaitForSeconds(1f);
        PlayFabMultiplayerAgentAPI.ReadyForPlayers();
    }

    private void OnServerActive() {
        networkManager.StartServer();
        Debug.Log("Server started");
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
        yield return new WaitForSeconds(5f);
        Application.Quit();
    }
}
