using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject landingPangePanel = default;

    //zmiana funkcjonalności na hostowanie custom lobby, dlatego zostawiam kod
    public void HostLobby() {
        NetworkManagerLobby.Instance.connType = "remoteServer";
        NetworkManagerLobby.Instance.StartServer();


        ToggleLandingPageNonActive();
    }

    private void OnEnable()
    {
        NetworkManagerLobby.OnClientConnected += ToggleLandingPageNonActive;
        NetworkManagerLobby.OnClientDisconnected += ToggleLandingPageActive;
    }

    private void OnDisable()
    {
        NetworkManagerLobby.OnClientConnected -= ToggleLandingPageNonActive;
        NetworkManagerLobby.OnClientDisconnected -= ToggleLandingPageActive;
    }

    private void ToggleLandingPageActive()
    {
        landingPangePanel.SetActive(true);
    }

    private void ToggleLandingPageNonActive()
    {
        landingPangePanel.SetActive(false);
    }
}
