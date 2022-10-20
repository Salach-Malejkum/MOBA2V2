using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private NetworkManagerLobby networkManager = default;

    [SerializeField] private GameObject landingPangePanel = default;

    //useless
    public void HostLobby() {
        networkManager.StartHost();

        landingPangePanel.SetActive(false);
    }
}
