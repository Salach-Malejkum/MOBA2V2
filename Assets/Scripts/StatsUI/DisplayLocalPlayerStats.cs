using Mirror;

public class DisplayLocalPlayerStats : NetworkBehaviour
{
    private PlayerStats localPlayerStats;
    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
    }

    [Client]
    private void LoadLocalPlayerStats()
    {

    }
}
