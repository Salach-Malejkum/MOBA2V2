using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayPlayerName : MonoBehaviour
{
    [SerializeField] private TMP_Text playerName;
    [SerializeField] private PlayerStats playerStats;

    private void Start()
    {
        Debug.Log(this.playerStats.PlayerName);
        this.playerName.text = this.playerStats.PlayerName.ToString();
    }
}
