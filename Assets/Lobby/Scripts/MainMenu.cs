using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject landingPangePanel = default;

    //zmiana funkcjonalności na hostowanie custom lobby, dlatego zostawiam kod
    public void HostLobby() {
        NetworkManagerLobby.Instance.connType = "local";
        NetworkManagerLobby.Instance.StartHost();
        

        landingPangePanel.SetActive(false);
    }
}
