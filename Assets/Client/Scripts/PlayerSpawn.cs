using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerSpawn : NetworkBehaviour
{
    [SerializeField] private GameObject playerPrefab = default;

    private static List<Transform> spawnPoints = new List<Transform>();

    private int nextIdx = 0;

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
    public void SpawnPlayer(NetworkConnection conn) {
        Transform spawnPoint = spawnPoints.ElementAtOrDefault(nextIdx);

        if(spawnPoint == null) {
            Debug.LogError("Missing spawn");
            return;
        }

        GameObject playerInstance = Instantiate(playerPrefab, spawnPoints[nextIdx].position, spawnPoints[nextIdx].rotation);
        NetworkServer.Spawn(playerInstance, conn);

        nextIdx++;
    }
}
