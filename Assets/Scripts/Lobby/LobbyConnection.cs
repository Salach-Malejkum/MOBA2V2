using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LobbyConnection : MonoBehaviour
{
    [SerializeField] TMP_InputField joinMatchInput;
    [SerializeField] Button joinButton;
    [SerializeField] Button hostButton;

    public void Host() {
        joinMatchInput.interactable = false;
        joinButton.interactable = false;
        hostButton.interactable = false;

        PlayerConnection.localPlayerConnection.HostGame();
    }

    public void Join() {
        joinMatchInput.interactable = false;
        joinButton.interactable = false;
        hostButton.interactable = false;
    }
}
