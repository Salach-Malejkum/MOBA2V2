using Mirror;

public class NetworkInGamePlayer : NetworkBehaviour
{
    [SyncVar]
    public string DisplayName = "Loading...";

    public override void OnStartClient()
    {
        DontDestroyOnLoad(gameObject);
        NetworkManagerLobby.Instance.InGamePlayers.Add(this);
    }

    public override void OnStopClient()
    {
        NetworkManagerLobby.Instance.InGamePlayers.Remove(this);
    }

    [Server]
    public void SetDisplayName(string displayName) {
        this.DisplayName = displayName;
    }
}
