using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BuildingMenuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject buildingMenuCanva;
    public void ToggleBuildingMenu(InputAction.CallbackContext _)
    {
        if (this.buildingMenuCanva.activeSelf)
        {
            this.buildingMenuCanva.SetActive(false);
        }
        else
        {
            this.buildingMenuCanva.SetActive(true);
        }
    }
}
