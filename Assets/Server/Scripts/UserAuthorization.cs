using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
public class UserAuthorization : MonoBehaviour
{
    [SerializeField] private GameObject signInDisplay = default;
    [SerializeField] private TMP_InputField usernameInputField = default;
    [SerializeField] private TMP_InputField emailInputField = default;
    [SerializeField] private TMP_InputField passwordInputField = default;

    public static string EntityId;
    public static string SessionTicket;

    public void CreateAccount() {
        PlayFabClientAPI.RegisterPlayFabUser(new RegisterPlayFabUserRequest {
            Username = usernameInputField.text,
            Email = emailInputField.text,
            Password = passwordInputField.text
        }, resultCallback => {
            SessionTicket = resultCallback.SessionTicket;
            EntityId = resultCallback.EntityToken.Entity.Id;
            signInDisplay.SetActive(false);
        }, errorCallback => {
            Debug.Log(errorCallback.GenerateErrorReport());
        });
    }

    public void LoginWithCredentials() {
        PlayFabClientAPI.LoginWithPlayFab(new LoginWithPlayFabRequest {
            Username = usernameInputField.text,
            Password = passwordInputField.text
        }, resultCallback => {
            SessionTicket = resultCallback.SessionTicket;
            EntityId = resultCallback.EntityToken.Entity.Id;
            signInDisplay.SetActive(false);
        }, errorCallback => {
            Debug.Log(errorCallback.GenerateErrorReport());
        });
    }
}
