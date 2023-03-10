using UnityEngine;
using UnityEngine.UI;
using TMPro;

//dołączenie do custom lobby, zostawiam funkcjonalność
public class JoinLobbyMenu : MonoBehaviour
{

    [SerializeField] private GameObject landingPagePanel = null;
    [SerializeField] private TMP_InputField ipAddressInputField = null;
    [SerializeField] private Button joinButton = null;

    private void OnEnable() {
        NetworkManagerLobby.OnClientConnected += HandleClientConnected;
        NetworkManagerLobby.OnClientDisconnected += HandleClientDisconnected;
    }

    private void OnDisable() {
        NetworkManagerLobby.OnClientConnected -= HandleClientConnected;
        NetworkManagerLobby.OnClientDisconnected -= HandleClientDisconnected;
    }

    public void JoinLobby() {
        string ipAddress = ipAddressInputField.text;
        NetworkManagerLobby.Instance.connType = "remoteClient";
        NetworkManagerLobby.Instance.networkAddress = ipAddress;
        NetworkManagerLobby.Instance.StartClient();
    
        joinButton.interactable = false;
    }

    private void HandleClientConnected() {
        joinButton.interactable = true;

        gameObject.SetActive(false);
        landingPagePanel.SetActive(false);
    }

    private void HandleClientDisconnected() {
        joinButton.interactable = true;
    }
}
