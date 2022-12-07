using Mirror;
using System.Collections;
using System.Linq;
using UnityEngine;

public class SpawnManager : NetworkBehaviour
{
    private float timePassed = 0f;
    private MinionSpawnerScript blueTeamTopMinions;
    private MinionSpawnerScript blueTeamBotMinions;
    private MinionSpawnerScript redTeamTopMinions;
    private MinionSpawnerScript redTeamBotMinions;

    // Start is called before the first frame update
    public override void OnStartServer()
    {
        this.blueTeamTopMinions = new MinionSpawnerScript
            (
            Enums.TeamMinionSpawnerPosition.blueTeamSpawn,
            Enums.Layers.blueTeamLayer,
            Enums.MinionPaths.topPathPoints
            );

        this.blueTeamBotMinions = new MinionSpawnerScript
            (
            Enums.TeamMinionSpawnerPosition.blueTeamSpawn,
            Enums.Layers.blueTeamLayer,
            Enums.MinionPaths.botPathPoints
            );

        this.redTeamTopMinions = new MinionSpawnerScript
            (
            Enums.TeamMinionSpawnerPosition.redTeamSpawn,
            Enums.Layers.redTeamLayer,
            Enumerable.Reverse(Enums.MinionPaths.topPathPoints).ToArray()
            );

        this.redTeamBotMinions = new MinionSpawnerScript
            (
            Enums.TeamMinionSpawnerPosition.redTeamSpawn,
            Enums.Layers.redTeamLayer,
            Enumerable.Reverse(Enums.MinionPaths.botPathPoints).ToArray()
            );
    }

    [ServerCallback]
    // Update is called once per frame
    private void FixedUpdate()
    {
        this.timePassed += Time.deltaTime;
        this.SpawnMinions(this.blueTeamTopMinions);
        this.SpawnMinions(this.blueTeamBotMinions);
        this.SpawnMinions(this.redTeamTopMinions);
        this.SpawnMinions(this.redTeamBotMinions);
    }

    [Server]
    private void SpawnMinions(MinionSpawnerScript team)
    {
        if (team.CheckIfCanSpawn(this.timePassed))
        {
            StartCoroutine(SpawnMinionWave(team));
        }
    }

    [ServerCallback]
    private IEnumerator SpawnMinionWave(MinionSpawnerScript team)
    {
        for (int i = 0; i < 3; i++)
        {
            SpawnMinion(team.GetMeleeMinionPrefab(), team.GetSpawnPosition(), team.GetLayer(), team.GetMinionPath());
            yield return new WaitForSeconds(0.5f);
        }

        if (team.CheckIfCanSpawnCannon())
        {
            SpawnMinion(team.GetCannonMinionPrefab(), team.GetSpawnPosition(), team.GetLayer(), team.GetMinionPath());
            yield return new WaitForSeconds(0.5f);
        }

        for (int i = 0; i < 3; i++)
        {
            SpawnMinion(team.GetRangedMinionPrefab(), team.GetSpawnPosition(), team.GetLayer(), team.GetMinionPath());
            yield return new WaitForSeconds(0.5f);
        }
    }

    private void SpawnMinion(GameObject prefab, Vector3 position, LayerMask layer, Vector3[] path)
    {
        GameObject go = Instantiate(prefab, position, Quaternion.identity);
        go.layer = layer;
        go.GetComponent<MinionScript>().SetMinionPath(path);

        NetworkServer.Spawn(go);
        go.GetComponent<MobStats>().OnMobSpawned();
    }
}
