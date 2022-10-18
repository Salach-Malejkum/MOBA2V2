using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private MinionSpawnerScript leftTeam;
    //private MinionSpawnerScript rightTeam;

    // Start is called before the first frame update
    void Start()
    {
        this.leftTeam = new MinionSpawnerScript(new Vector3(-5, 0, 0));
        //this.rightTeam = new MinionSpawnerScript(new Vector3(5, 0, 0));
    }

    // Update is called once per frame
    void Update()
    {
        this.leftTeam.FixedUpdate();
    }
}
