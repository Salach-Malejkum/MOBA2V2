using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using System.Net.Mail;
using System;
using System.Linq;
using System.Text.RegularExpressions;

public class RegisterController : MonoBehaviour
{
    public GameObject inputFieldEmail;
    public GameObject inputFieldPass;
    public GameObject inputFieldUserName;
    public GameObject inputFieldPassConf;

    public Button backButt;
    public Button registerButt;

    public GameObject loginBox;
    public GameObject registerBox;
    public GameObject messageBox;

    void Start()
    {
        registerButt.onClick.AddListener(Register);

        backButt.onClick.AddListener(ChangeBox);
    }

    void Register()
    {
        ResetComponents();

        string mail = inputFieldEmail.GetComponent<TMP_InputField>().text;
        string pass = inputFieldPass.GetComponent<TMP_InputField>().text;
        string username = inputFieldUserName.GetComponent<TMP_InputField>().text;
        string passCnof = inputFieldPassConf.GetComponent<TMP_InputField>().text;

        bool mailAns = CheckMailField(mail);
        bool userNameAns = CheckUserNameField(username);
        bool passAns = CheckPassField(pass);
        bool passConfAns = CheckPassConfField(pass, passCnof);

        if (mailAns && passAns && passConfAns && userNameAns)
        {
            bool serverAns = CheckInputsWithServer(mail, username, pass);
            if (serverAns)
            {
                messageBox.SetActive(true);
                messageBox.GetComponentInChildren<TextMeshProUGUI>().text = "Rejestracja przebieg³a pomyœlnie";
            }
            else
            {
                messageBox.SetActive(true);
                messageBox.GetComponentInChildren<TextMeshProUGUI>().text += "nazwa u¿ytkownika jest ju¿ zajêta";
                ColorBlock cb = inputFieldUserName.GetComponent<TMP_InputField>().colors;
                cb.normalColor = Color.red;
                inputFieldUserName.GetComponent<TMP_InputField>().colors = cb;
            }
        }
        else
        {
            messageBox.SetActive(true);
        }
    }

    void ChangeBox()
    {
        ResetComponents();

        inputFieldEmail.GetComponent<TMP_InputField>().text = "";
        inputFieldPass.GetComponent<TMP_InputField>().text = "";
        inputFieldUserName.GetComponent<TMP_InputField>().text = "";
        inputFieldPassConf.GetComponent<TMP_InputField>().text = "";

        messageBox.SetActive(false);
        loginBox.SetActive(true);
        registerBox.SetActive(false);
    }

    bool CheckMailField(string mail)
    {
        if (mail == "")
        {
            CheckMailFieldHelper();
            return false;
        }
            

        try
        {
            MailAddress m = new MailAddress(mail);
        }
        catch (FormatException)
        {
            CheckMailFieldHelper();
            return false;
        }

        return true;
    }

    void CheckMailFieldHelper()
    {
        messageBox.GetComponentInChildren<TextMeshProUGUI>().text += "Podano niepoprawny adres email.\n";
        ColorBlock cb = inputFieldEmail.GetComponent<TMP_InputField>().colors;
        cb.normalColor = Color.red;
        inputFieldEmail.GetComponent<TMP_InputField>().colors = cb;
    }

    bool CheckUserNameField(string userName)
    {
        if (userName == "" || userName.Length > 15)
        {
            CheckUserNameFieldHelper();
            return false;
        }

        if (userName.Any(x => !char.IsLetterOrDigit(x)))
        {
            CheckUserNameFieldHelper();
            return false;
        }

        return true;
    }

    void CheckUserNameFieldHelper()
    {
        messageBox.GetComponentInChildren<TextMeshProUGUI>().text += "Nazwa u¿ytkownika nie mo¿e zawieraæ znaków specjalnych i byæ d³u¿sza ni¿ 15 znaków.\n";
        ColorBlock cb = inputFieldPassConf.GetComponent<TMP_InputField>().colors;
        cb.normalColor = Color.red;
        inputFieldUserName.GetComponent<TMP_InputField>().colors = cb;
    }

    bool CheckPassField(string pass)
    {
        if (pass == "" || pass.Length >= 25)
        {
            CheckPassFieldHelper();
            return false;
        }

        if (pass.Any(x => !char.IsLetterOrDigit(x)))
        {
            CheckPassFieldHelper();
            return false;
        }

        if (!pass.Any(char.IsUpper) || !pass.Any(char.IsDigit) || !pass.Any(char.IsLower))
        {
            CheckPassFieldHelper();
            return false;
        }

        return true;
    }

    void CheckPassFieldHelper()
    {
        messageBox.GetComponentInChildren<TextMeshProUGUI>().text += "Podano has³o nie spe³niaj¹ce nastêpuj¹cych wymagañ:\nHas³o zawiera przynajmniej jedn¹ ma³¹ litere, \nHas³o zawiera przynajmniej jedn¹ wielk¹ litere, \nHas³o zawiera przynajmniej jedn¹ cyfrê.\n";
        ColorBlock cb = inputFieldPass.GetComponent<TMP_InputField>().colors;
        cb.normalColor = Color.red;
        inputFieldPass.GetComponent<TMP_InputField>().colors = cb;
    }

    bool CheckPassConfField(string pass, string passConf)
    {
        if (!pass.Equals(passConf))
        {
            messageBox.GetComponentInChildren<TextMeshProUGUI>().text += "Podane potwierdzenie has³a ró¿ni siê od has³a.\n";
            ColorBlock cb = inputFieldPassConf.GetComponent<TMP_InputField>().colors;
            cb.normalColor = Color.red;
            inputFieldPassConf.GetComponent<TMP_InputField>().colors = cb;
            return false;
        }

        return true;
    }

    bool CheckInputsWithServer(string mail, string username, string pass)
    {
        //wysy³am mail, user name i pass do servera
       
        return false;//return true;
    }

    void ResetComponents()
    {
        messageBox.SetActive(false);
        messageBox.GetComponentInChildren<TextMeshProUGUI>().text = "";

        ColorBlock cb = inputFieldUserName.GetComponent<TMP_InputField>().colors;
        cb.normalColor = Color.white;
        inputFieldUserName.GetComponent<TMP_InputField>().colors = cb;
        inputFieldEmail.GetComponent<TMP_InputField>().colors = cb;
        inputFieldPass.GetComponent<TMP_InputField>().colors = cb;
        inputFieldPassConf.GetComponent<TMP_InputField>().colors = cb;
    }
}
