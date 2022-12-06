using Mirror;
using UnityEngine;

public class SetActiveStatsCanvas : NetworkBehaviour
{
    [SerializeField]
    private GameObject statsCanvas;
    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        this.statsCanvas.SetActive(true);
    }
}
