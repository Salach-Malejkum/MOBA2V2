using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerSpawnPoint : NetworkBehaviour
{
    [SyncVar] public GameObject playerAssigned = null;
    private bool hasPlayerAssigned = false;
    [SerializeField] private float respawnTimeInterval = 5.1f;
    [SerializeField] private float respawnTimer;

    private void Awake() {
        PlayerSpawn.AddSpawnPoint(this);
        this.respawnTimer = this.respawnTimeInterval;
    } 

    public void AssignPlayerToThisSpawnPoint(GameObject playerInstance) {
        this.playerAssigned = playerInstance;
    }

    [ServerCallback]
    void Update() {
        if(this.playerAssigned != null) {
            if(!playerAssigned.gameObject.activeSelf) {
                if(respawnTimer > 0.1f) {
                    Debug.Log("current timer: " + respawnTimer.ToString());
                    this.respawnTimer -= Time.deltaTime;
                } else {
                    Debug.Log("current timer: " + respawnTimer.ToString());
                    RpcHandlePlayerRespawn();
                    this.respawnTimer = this.respawnTimeInterval;
                }
            }
        }
    }

    private void OnDestroy() => PlayerSpawn.RemoveSpawnPoint(this);

    private void OnDrawGizmos() {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position, 1f);
    }

    [ClientRpc]
    private void RpcHandlePlayerRespawn() {
        this.playerAssigned.transform.position = this.transform.position;
        this.playerAssigned.SetActive(true);
    }
}
