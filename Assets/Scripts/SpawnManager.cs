using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private float timePassed = 0f;
    private MinionSpawnerScript leftTeamMinions;
    private MinionSpawnerScript rightTeamMinions;

    // Start is called before the first frame update
    private void Start()
    {
        this.leftTeamMinions = new MinionSpawnerScript
            (
            Enums.TeamMinionSpawnerPosition.blueTeamSpawn,
            Enums.TeamMinionSpawnerTags.blueTeamTag
            );
        this.rightTeamMinions = new MinionSpawnerScript
            (
            Enums.TeamMinionSpawnerPosition.redTeamSpawn,
            Enums.TeamMinionSpawnerTags.redTeamTag
            );
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        this.timePassed += Time.deltaTime;
        this.SpawnMinions(this.leftTeamMinions);
        this.SpawnMinions(this.rightTeamMinions);
    }

    private void SpawnMinions(MinionSpawnerScript team)
    {
        if (team.CheckIfCanSpawn(this.timePassed))
        {
            this.SpawnMinionWave(team);
        }
    }

    private void SpawnMinionWave(MinionSpawnerScript team)
    {
        for (int i = 0; i < 3; i++)
        {
            GameObject go = Instantiate(team.GetMeleeMinionPrefab(), team.GetSpawnPosition(), Quaternion.identity);
            go.tag = team.GetTagName();
        }

        if (team.CheckIfCanSpawnCannon())
        {
            GameObject go = Instantiate(team.GetCannonMinionPrefab(), team.GetSpawnPosition(), Quaternion.identity);
            go.tag = team.GetTagName();
        }

        for (int i = 0; i < 3; i++)
        {
            GameObject go = Instantiate(team.GetRangedMinionPrefab(), team.GetSpawnPosition(), Quaternion.identity);
            go.tag = team.GetTagName();
        }
    }
}
