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
        Debug.Log(context.control.displayName);
        if (context.control.displayName == "Esc")
        {
            Debug.Log("im in");
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
