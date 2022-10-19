using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private float timePassed = 0f;
    private MinionSpawnerScript leftTeamMinions;
    private MinionSpawnerScript rightTeamMinions;

    // Start is called before the first frame update
    void Start()
    {
        this.leftTeamMinions = new MinionSpawnerScript(Enums.TeamSpawnerPosition.blueTeamSpawn, "Blue Team");
        this.rightTeamMinions = new MinionSpawnerScript(Enums.TeamSpawnerPosition.redTeamSpawn, "Red Team");
        
    }

    // Update is called once per frame
    void Update()
    {
        this.timePassed += Time.deltaTime;
        this.SpawnMinions(this.leftTeamMinions);
        this.SpawnMinions(this.rightTeamMinions);
    }

    private void SpawnMinions(MinionSpawnerScript team)
    {
        if (team.CheckIfCanSpawm(this.timePassed))
        {
            this.SpawnMinionWave(team);
        }
    }

    private void SpawnMinionWave(MinionSpawnerScript team)
    {
        if (!team.CheckIfSpawnCannon())
        {
            return;
        }

        for (int i = 0; i < 3; i++)
        {
            Instantiate(team.GetMeleeMinionPrefab(), team.GetSpawnPosition(), Quaternion.identity);
        }

        if (team.CheckIfSpawnCannon())
        {
            Instantiate(team.GetCannonMinionPrefab(), team.GetSpawnPosition(), Quaternion.identity);
        }

        for (int i = 0; i < 3; i++)
        {
            Instantiate(team.GetRangedMinionPrefab(), team.GetSpawnPosition(), Quaternion.identity);
        }
    }


}
