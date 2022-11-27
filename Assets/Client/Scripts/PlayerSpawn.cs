using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerSpawn : NetworkBehaviour
{
    [SerializeField] private GameObject playerPrefab = default;

    private static List<PlayerSpawnPoint> spawnPoints = new List<PlayerSpawnPoint>();
    public static void AddSpawnPoint(PlayerSpawnPoint spawnPoint) {
        spawnPoints.Add(spawnPoint);

        spawnPoints = spawnPoints.OrderBy(x => x.transform.GetSiblingIndex()).ToList();
    }

    public static void RemoveSpawnPoint(PlayerSpawnPoint spawnPoint) => spawnPoints.Remove(spawnPoint);

    public override void OnStartServer()
    {
        NetworkManagerLobby.OnServerReadied += SpawnPlayer;
    }

    [ServerCallback]
    private void OnDestroy() => NetworkManagerLobby.OnServerReadied -= SpawnPlayer;

    [Server]
    public void SpawnPlayer(object sender, OnPlayerSpawnArgs args) {
        
        Debug.Log("Spawning player: " + args.conn.ToString() + " on point: " + args.PlayerId.ToString());
        PlayerSpawnPoint spawnPoint = spawnPoints.ElementAtOrDefault(args.PlayerId);

        if (spawnPoint == null) {
            Debug.LogError("Missing spawn");
            return;
        }

        Debug.Log(spawnPoints[args.PlayerId].transform.position);
        GameObject playerInstance = (GameObject)Instantiate(this.playerPrefab, spawnPoints[args.PlayerId].transform.position, spawnPoints[args.PlayerId].transform.rotation);
        Debug.Log(playerInstance.transform.position);

        PlayerStats playerStats = playerInstance.GetComponent<PlayerStats>();

        switch (args.PlayerId % 2)
        {
            case 0:
                playerStats.side = "Blue";
                playerInstance.gameObject.layer = LayerMask.NameToLayer("Blue");
                break;
            case 1:
                playerStats.side = "Red";
                playerInstance.gameObject.layer = LayerMask.NameToLayer("Red");
                break;
        }

        switch (args.PlayerId)
        {
            case 0:
            case 1:
                playerStats.lane = "Mid";
                break;
            case 2:
            case 3:
                playerStats.lane = "Bot";
                break;
        }

        NetworkServer.ReplacePlayerForConnection(args.conn, playerInstance);

        spawnPoints[args.PlayerId].AssignPlayerToThisSpawnPoint(playerInstance);

        playerInstance.GetComponent<UpgradeManager>().SetTurrets();
        NetworkManagerLobby.Instance.PlayersLoadedToGame.Add(playerInstance);

        foreach (GameObject inGamePlayer in NetworkManagerLobby.Instance.PlayersLoadedToGame)
        {
            if (inGamePlayer.GetComponent<NetworkIdentity>().isLocalPlayer)
            {

            }
        }
    }

}
