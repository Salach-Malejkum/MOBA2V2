using TMPro;
using UnityEngine.UI;
using UnityEngine;
using System.Net.Mail;
using System;
using System.Linq;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Text;
using UnityEngine.SceneManagement;

public class LoginController : MonoBehaviour
{
    public GameObject inputFieldEmail;
    public GameObject inputFieldPass;

    public Button loginButt;
    public Button registerButt;
    
    public GameObject loginBox;
    public GameObject registerBox;
    public GameObject messageBox;

    void Start()
    {
        loginButt.onClick.AddListener(LogIn);

        registerButt.onClick.AddListener(ChangeBox);
    }

    void LogIn()
    {
        ResetComponents();

        string mail = inputFieldEmail.GetComponent<TMP_InputField>().text;
        string pass = inputFieldPass.GetComponent<TMP_InputField>().text;
        bool ans = CheckInputsWithServer(mail, pass);

        if (ans)
        {
            //za³aduj scene klienta
            //zapisaæ id klienta w preferencjach
            SceneManager.LoadScene("LoggedIn");
        }
        else
        {
            messageBox.SetActive(true);
            messageBox.GetComponent<TextMeshProUGUI>().text = "podane dane logowania nie pasuj¹ do ¿adnych zawartych w systemie";

            ColorBlock cb = inputFieldEmail.GetComponent<TMP_InputField>().colors;
            cb.normalColor = Color.red;
            inputFieldEmail.GetComponent<TMP_InputField>().colors = cb;
            inputFieldPass.GetComponent<TMP_InputField>().colors = cb;
        }
    }

    void ChangeBox()
    {
        ResetComponents();

        inputFieldEmail.GetComponent<TMP_InputField>().text = "";
        inputFieldPass.GetComponent<TMP_InputField>().text = "";

        messageBox.SetActive(false);
        loginBox.SetActive(false);
        registerBox.SetActive(true);
    }

    bool CheckInputsWithServer(string mail, string pass)
    {
        var mailAns = CheckMailField(mail);
        var passAns = CheckPassField(pass);
        if (!(mailAns && passAns)) { 
            return false;
        }

        //wysy³am mail i pass do servera
        return ServerConnections.Login(mail, pass);
    }

    bool CheckMailField(string mail)
    {
        if (mail == "")
        {
            return false;
        }

        try
        {
            MailAddress m = new MailAddress(mail);
        }
        catch (FormatException)
        {
            return false;
        }

        return true;
    }

    bool CheckPassField(string pass)
    {
        if (pass == "" || pass.Length >= 25)
        {
            return false;
        }

        if (pass.Any(x => !char.IsLetterOrDigit(x)))
        {
            return false;
        }

        if (!pass.Any(char.IsUpper) || !pass.Any(char.IsDigit) || !pass.Any(char.IsLower))
        {
            return false;
        }

        return true;
    }

    void ResetComponents()
    {
        messageBox.GetComponent<TextMeshProUGUI>().text = "";
        messageBox.SetActive(false);

        ColorBlock cb = inputFieldEmail.GetComponent<TMP_InputField>().colors;
        cb.normalColor = Color.white;
        inputFieldEmail.GetComponent<TMP_InputField>().colors = cb;
        inputFieldPass.GetComponent<TMP_InputField>().colors = cb;
    }
}
