using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UpperPanel : MonoBehaviour
{
    public Button logoutButt;

    void Start()
    {
        logoutButt.onClick.AddListener(LogOut);
    }

    void LogOut()
    {
        // usun¹æ id klienta z preferencjii i po³¹cznie z serverem
        SceneManager.LoadScene("LoginScreen");
    }
}
