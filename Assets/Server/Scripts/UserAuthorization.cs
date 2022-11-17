using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
public class UserAuthorization : MonoBehaviour
{
    [SerializeField] private GameObject signUpDisplay = default;
    [SerializeField] private GameObject signInDisplay = default;
    [SerializeField] private GameObject loginScreenBackground = default;
    [SerializeField] private GameObject afterLoginScreen = default;
    [SerializeField] private TMP_InputField usernameInputFieldRegister = default;
    [SerializeField] private TMP_InputField emailInputFieldRegister = default;
    [SerializeField] private TMP_InputField passwordInputFieldRegister = default;
    [SerializeField] private TMP_InputField usernameInputFieldLogin = default;
    [SerializeField] private TMP_InputField passwordInputFieldLogin = default;
    [SerializeField] private ClientStartup client = default;

    public static string EntityId = null;
    public static string SessionTicket = null;

    public void CreateAccount() {
        this.transform.GetComponent<Feedback>().ChangeInputFieldsWhite(this.usernameInputFieldRegister, this.emailInputFieldRegister, this.passwordInputFieldRegister);
        this.transform.GetComponent<Feedback>().DesactivateMessageBox();

        PlayFabClientAPI.RegisterPlayFabUser(new RegisterPlayFabUserRequest {
            TitleId = PlayFabSettings.TitleId,
            Username = this.usernameInputFieldRegister.text,
            Email = this.emailInputFieldRegister.text,
            Password = this.passwordInputFieldRegister.text
        }, resultCallback => {
            SessionTicket = resultCallback.SessionTicket;
            EntityId = resultCallback.EntityToken.Entity.Id;
            this.client.RequestServerData();
            this.signInDisplay.SetActive(false);
            this.signUpDisplay.SetActive(false);
            this.loginScreenBackground.SetActive(false);
            this.afterLoginScreen.SetActive(true);
            PlayerDataManager.StrSave("Username", this.usernameInputFieldRegister.text);
        }, errorCallback => {
            Debug.Log(errorCallback.GenerateErrorReport());
            this.transform.GetComponent<Feedback>().ChangeInputFieldsRed(this.usernameInputFieldRegister, this.emailInputFieldRegister, this.passwordInputFieldRegister);
            this.transform.GetComponent<Feedback>().ActivateAndSetMessageText(errorCallback.ToString());
        });
    }

    public void LoginWithCredentials() {
        this.transform.GetComponent<Feedback>().ChangeInputFieldsWhite(this.usernameInputFieldLogin, this.passwordInputFieldLogin);
        this.transform.GetComponent<Feedback>().DesactivateMessageBox();

        PlayFabClientAPI.LoginWithPlayFab(new LoginWithPlayFabRequest {
            TitleId = PlayFabSettings.TitleId,
            Username = this.usernameInputFieldLogin.text,
            Password = this.passwordInputFieldLogin.text
        }, resultCallback => {
            SessionTicket = resultCallback.SessionTicket;
            EntityId = resultCallback.EntityToken.Entity.Id;
            this.client.RequestServerData();
            this.signInDisplay.SetActive(false);
            this.signUpDisplay.SetActive(false);
            this.loginScreenBackground.SetActive(false);
            this.afterLoginScreen.SetActive(true);
            PlayerDataManager.StrSave("Username", this.usernameInputFieldLogin.text);
        }, errorCallback => {
            Debug.Log(errorCallback.GenerateErrorReport());
            this.transform.GetComponent<Feedback>().ChangeInputFieldsRed(this.usernameInputFieldLogin, this.passwordInputFieldLogin);
            this.transform.GetComponent<Feedback>().ActivateAndSetMessageText(errorCallback.ToString());
        });
    }

    public void Logout()
    {
        EntityId = null;
        SessionTicket = null;
        this.loginScreenBackground.SetActive(true);
        this.signInDisplay.SetActive(true);
        this.afterLoginScreen.SetActive(false);
    }
}
