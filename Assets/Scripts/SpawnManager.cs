using Mirror;
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
            this.SpawnMinionWave(team);
        }
    }

    [ServerCallback]
    private void SpawnMinionWave(MinionSpawnerScript team)
    {
        for (int i = 0; i < 3; i++)
        {
            GameObject go = Instantiate(team.GetMeleeMinionPrefab(), team.GetSpawnPosition(), Quaternion.identity);
            go.layer = team.GetLayer();
            go.GetComponent<MinionScript>().SetMinionPath(team.GetMinionPath());

            NetworkServer.Spawn(go);
            go.GetComponent<MobStats>().OnMobSpawned();
        }

        if (team.CheckIfCanSpawnCannon())
        {
            GameObject go = Instantiate(team.GetCannonMinionPrefab(), team.GetSpawnPosition(), Quaternion.identity);
            go.layer = team.GetLayer();
            go.GetComponent<MinionScript>().SetMinionPath(team.GetMinionPath());

            NetworkServer.Spawn(go);
            go.GetComponent<MobStats>().OnMobSpawned();
        }

        for (int i = 0; i < 3; i++)
        {
            GameObject go = Instantiate(team.GetRangedMinionPrefab(), team.GetSpawnPosition(), Quaternion.identity);
            go.layer = team.GetLayer();
            go.GetComponent<MinionScript>().SetMinionPath(team.GetMinionPath());

            NetworkServer.Spawn(go);
            go.GetComponent<MobStats>().OnMobSpawned();
        }
    }
}
