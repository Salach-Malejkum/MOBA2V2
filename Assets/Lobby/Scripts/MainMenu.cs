using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject landingPangePanel = default;

    //useless
    public void HostLobby() {
        NetworkManagerLobby.Instance.StartHost();

        landingPangePanel.SetActive(false);
    }
}
