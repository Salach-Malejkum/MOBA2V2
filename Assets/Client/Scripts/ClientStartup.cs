using System.Collections;
using System.Collections.Generic;
using System;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.MultiplayerModels;
using UnityEngine;
using Mirror;

public class ClientStartup : MonoBehaviour
{
    [SerializeField] private BuildType buildType;

    public void RequestServerData() {
        if(buildType.chosenBuild == BuildType.Build.RemoteClient) {
            RequestMultiplayerServerRequest requestData = new RequestMultiplayerServerRequest();
            requestData.BuildId = buildType.buildId;
            requestData.SessionId = System.Guid.NewGuid().ToString();
            requestData.PreferredRegions = new List<string>() { "NorthEurope" };
            PlayFabMultiplayerAPI.RequestMultiplayerServer(requestData, OnRequestMultiplayerServer, OnRequestMultiplayerServerError);
        }
    }

    private void OnRequestMultiplayerServer(RequestMultiplayerServerResponse response) {
        Debug.Log(response.ToString());
        ShowConnInfo(response);
    }

    private void ShowConnInfo(RequestMultiplayerServerResponse response = null) {
        if(response == null) {
            Debug.Log("No response");
        } else {
            Debug.Log("IP: " + response.IPV4Address + " Port " + (ushort)response.Ports[0].Num);
        }
    }

    private void OnRequestMultiplayerServerError(PlayFabError error) {
        Debug.Log(error.ToString());
    }
}
