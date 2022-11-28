using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuUI : NetworkBehaviour
{
    [SerializeField] private GameObject menuCanva;

    [ClientCallback]
    public void OpenMenu(InputAction.CallbackContext context)
    {
        if (!hasAuthority)
        {
            return;
        }

        if (context.control.displayName == "Esc")
        {
            this.ToggleMenu();
        }
    }

    [Client]
    public void ToggleMenu()
    {
        if (this.menuCanva.activeSelf)
        {
            this.menuCanva.SetActive(false);
        }
        else
        {
            this.menuCanva.SetActive(true);
        }
    }

    [Client]
    public void QuitGame()
    {
        Application.Quit();
    }
}
