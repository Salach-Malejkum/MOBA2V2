using UnityEngine;
using TMPro;

public class DisplayPlayerName : MonoBehaviour
{
    [SerializeField] private TMP_Text playerName;
    [SerializeField] private PlayerStats playerStats;

    private void Start()
    {
        this.playerName.text = this.playerStats.playerName.ToString();
    }
}
