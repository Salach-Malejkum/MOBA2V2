using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.MultiplayerAgent.Model;

public class ServerStartUp : MonoBehaviour
{
    [SerializeField] private BuildType buildType;

    private List<ConnectedPlayer> connectedPlayers;

    void Start()
    {
        if(buildType.chosenBuild == BuildType.Build.RemoteServer) {
            StartRemoteServer();
        }
    }

    private void StartRemoteServer() {
        Debug.Log("This is a test log. Please ignore.");
        PlayFabMultiplayerAgentAPI.Start();
        PlayFabMultiplayerAgentAPI.IsDebugging = buildType.debugBuild;
		PlayFabMultiplayerAgentAPI.OnShutDownCallback += OnShutdown;
		PlayFabMultiplayerAgentAPI.OnServerActiveCallback += OnServerActive;
		PlayFabMultiplayerAgentAPI.OnAgentErrorCallback += OnAgentError;
        NetworkManagerLobby.Instance.StartServer();
        Debug.Log("Server started");
        NetworkManagerLobby.Instance.OnPlayerAdded.AddListener(OnPlayerAdded);
        NetworkManagerLobby.Instance.OnPlayerRemoved.AddListener(OnPlayerRemoved);

        StartCoroutine(ReadyForPlayers());
    }

    IEnumerator ReadyForPlayers() {
        yield return new WaitForSeconds(1f);
        PlayFabMultiplayerAgentAPI.ReadyForPlayers();
    }

    private void OnServerActive() {
        //TODO: cos do zrobienia po przejsciu serwera na active (jak otrzymuje graczy, startserver musi byc called wczesniej)
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
