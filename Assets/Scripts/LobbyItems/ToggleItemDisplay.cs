using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ToggleItemDisplay : MonoBehaviour
{
    [SerializeField]
    private GameObject itemDisplay;
    [SerializeField]
    private TMP_Text itemsBtnText;
    public void ToggleShop()
    {
        if (this.itemDisplay.activeSelf)
        {
            this.itemDisplay.SetActive(false);
            this.itemsBtnText.text = "Show Items";
        }
        else
        {
            this.itemDisplay.SetActive(true);
            this.itemsBtnText.text = "Hide Items";
        }
    }
}
