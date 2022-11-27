using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerSpawn : NetworkBehaviour
{
    [SerializeField] private GameObject playerPrefab = default;

    private static List<Transform> spawnPoints = new List<Transform>();
    public static void AddSpawnPoint(Transform transform) {
        spawnPoints.Add(transform);

        spawnPoints = spawnPoints.OrderBy(x => x.GetSiblingIndex()).ToList();
    }

    public static void RemoveSpawnPoint(Transform transform) => spawnPoints.Remove(transform);

    public override void OnStartServer()
    {
        NetworkManagerLobby.OnServerReadied += SpawnPlayer;
    }

    [ServerCallback]
    private void OnDestroy() => NetworkManagerLobby.OnServerReadied -= SpawnPlayer;

    [Server]
    public void SpawnPlayer(object sender, OnPlayerSpawnArgs args) {
        Debug.Log("Spawning player: " + args.conn.ToString() + " on point: " + args.PlayerId.ToString());
        Transform spawnPoint = spawnPoints.ElementAtOrDefault(args.PlayerId);

        if (spawnPoint == null) {
            Debug.LogError("Missing spawn");
            return;
        }

        Debug.Log(spawnPoints[args.PlayerId].position);
        GameObject playerInstance = (GameObject)Instantiate(this.playerPrefab, spawnPoints[args.PlayerId].position, spawnPoints[args.PlayerId].rotation);
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
        Debug.Log(playerInstance.layer);

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

        playerInstance.GetComponent<UpgradeManager>().SetTurrets();

        NetworkServer.ReplacePlayerForConnection(args.conn, playerInstance);

        playerInstance.GetComponent<UpgradeManager>();
    }
}
