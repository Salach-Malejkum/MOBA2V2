using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class AfterGameMenu : MonoBehaviour
{
    [SerializeField] private TMP_Text afterGameText = default;
    [SerializeField] private GameResultHold resultHold = default;

    void Awake() {
        afterGameText.text = "";
    }

    void Update() {
        Debug.Log(resultHold.gameResult);
        Debug.Log(NetworkManagerLobby.Instance.PlayerSide);
        if(this.resultHold.gameResult != "" && afterGameText.text == "") {
            if (NetworkManagerLobby.Instance.PlayerSide == this.resultHold.gameResult) {
                afterGameText.text = "Defeat";
            } else {
                afterGameText.text = "Victory";
            }
        }
    }

    public void ChangeSceneToLobby() {
        DisconnectLogic();
    }

    public void QuitApplication() {
        DisconnectLogic();
        Application.Quit();
    }

    private void DisconnectLogic() {
        if (NetworkManagerLobby.Instance.connType == "host") {
            NetworkManagerLobby.Instance.StopHost();
        } else if (NetworkManagerLobby.Instance.connType == "local") {
            NetworkManagerLobby.Instance.StopClient();
        } else {
            NetworkManagerLobby.Instance.OnApplicationQuit();
        }
    }
}
