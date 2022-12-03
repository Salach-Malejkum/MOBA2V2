using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject landingPangePanel = default;
    [SerializeField] private GameObject itemDisplay = default;

    //zmiana funkcjonalności na hostowanie custom lobby, dlatego zostawiam kod
    public void HostLobby() {
        NetworkManagerLobby.Instance.connType = "remoteServer";
        NetworkManagerLobby.Instance.StartServer();


        ToggleLandingPageNonActive();
        ToggleItemDisplayNonActive();
    }

    private void OnEnable()
    {
        NetworkManagerLobby.OnClientConnected += ToggleLandingPageNonActive;
        NetworkManagerLobby.OnClientConnected += ToggleItemDisplayNonActive;
        NetworkManagerLobby.OnClientDisconnected += ToggleLandingPageActive;
    }

    private void OnDisable()
    {
        NetworkManagerLobby.OnClientConnected -= ToggleLandingPageNonActive;
        NetworkManagerLobby.OnClientConnected -= ToggleItemDisplayNonActive;
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

    private void ToggleItemDisplayNonActive()
    {
        landingPangePanel.SetActive(false);
    }
}
