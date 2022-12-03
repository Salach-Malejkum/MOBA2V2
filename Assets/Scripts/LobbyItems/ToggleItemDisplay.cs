using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleItemDisplay : MonoBehaviour
{
    [SerializeField]
    private GameObject itemDisplay;
    public void ToggleShop()
    {
        if (this.itemDisplay.activeSelf)
        {
            this.itemDisplay.SetActive(false);
        }
        else
        {
            this.itemDisplay.SetActive(true);
        }
    }
}
