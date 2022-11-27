using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetActiveStatsCanvas : NetworkBehaviour
{
    [SerializeField]
    private GameObject statsCanvas;
    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        statsCanvas.SetActive(true);
    }
}
