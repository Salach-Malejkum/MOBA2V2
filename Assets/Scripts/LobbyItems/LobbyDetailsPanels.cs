using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyDetailsPanels : MonoBehaviour
{
    [SerializeField]
    public List<GameObject> detailsPanels;

    public void DesactivateAllInfoPanels()
    {
        foreach (GameObject panel in this.detailsPanels)
        {
            panel.SetActive(false);
        }
    }
}
