using Mirror;
using UnityEngine;

public class SetActiveStatsCanvas : NetworkBehaviour
{
    [SerializeField]
    private GameObject statsCanvas;
    [SerializeField]
    private GameObject healthBarCanvas;
    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        this.statsCanvas.SetActive(true);
        this.healthBarCanvas.SetActive(true);
    }
}
