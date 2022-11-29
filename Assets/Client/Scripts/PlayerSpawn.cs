using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerSpawn : NetworkBehaviour
{
    [SerializeField] private GameObject playerPrefabRed = default;
    [SerializeField] private GameObject playerPrefabBlue = default;

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

        GameObject playerInstance = default;
        PlayerStats playerStats = default;
        

        switch (args.PlayerId % 2)
        {
            case 0:
                playerInstance = Instantiate(this.playerPrefabBlue, spawnPoints[args.PlayerId].transform.position, spawnPoints[args.PlayerId].transform.rotation);
                playerStats = playerInstance.GetComponent<PlayerStats>();
                playerStats.side = "Blue";
                break;
            case 1:
                playerInstance = Instantiate(this.playerPrefabRed, spawnPoints[args.PlayerId].transform.position, spawnPoints[args.PlayerId].transform.rotation);
                playerStats = playerInstance.GetComponent<PlayerStats>();
                playerStats.side = "Red";
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

        playerStats.playerName = args.PlayerName;

        NetworkServer.ReplacePlayerForConnection(args.conn, playerInstance);

        spawnPoints[args.PlayerId].AssignPlayerToThisSpawnPoint(playerInstance);

        NetworkManagerLobby.Instance.PlayersLoadedToScene.Add(playerInstance.GetComponent<NetworkIdentity>());

        playerInstance.GetComponent<UpgradeManager>().SetTurrets();
    }

}
