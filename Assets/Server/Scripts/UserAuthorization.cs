using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
public class UserAuthorization : MonoBehaviour
{
    [SerializeField] private GameObject signInDisplay = default;
    [SerializeField] private GameObject afterLoginScreen = default;
    [SerializeField] private TMP_InputField usernameInputField = default;
    [SerializeField] private TMP_InputField emailInputField = default;
    [SerializeField] private TMP_InputField passwordInputField = default;
    [SerializeField] private ClientStartup client = default;

    public static string EntityId = null;
    public static string SessionTicket = null;

    public void CreateAccount() {
        PlayFabClientAPI.RegisterPlayFabUser(new RegisterPlayFabUserRequest {
            TitleId = PlayFabSettings.TitleId,
            Username = usernameInputField.text,
            Email = emailInputField.text,
            Password = passwordInputField.text
        }, resultCallback => {
            SessionTicket = resultCallback.SessionTicket;
            EntityId = resultCallback.EntityToken.Entity.Id;
            client.RequestServerData();
            signInDisplay.SetActive(false);
            afterLoginScreen.SetActive(true);
            PlayerDataManager.StrSave("Username", usernameInputField.text);
        }, errorCallback => {
            Debug.Log(errorCallback.GenerateErrorReport());
        });
    }

    public void LoginWithCredentials() {
        PlayFabClientAPI.LoginWithPlayFab(new LoginWithPlayFabRequest {
            TitleId = PlayFabSettings.TitleId,
            Username = usernameInputField.text,
            Password = passwordInputField.text
        }, resultCallback => {
            SessionTicket = resultCallback.SessionTicket;
            EntityId = resultCallback.EntityToken.Entity.Id;
            client.RequestServerData();
            signInDisplay.SetActive(false);
            afterLoginScreen.SetActive(true);
            PlayerDataManager.StrSave("Username", usernameInputField.text);
        }, errorCallback => {
            Debug.Log(errorCallback.GenerateErrorReport());
        });
    }
}
