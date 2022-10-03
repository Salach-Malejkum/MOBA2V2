using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Matchmaking : NetworkBehaviour
{
    public static Matchmaking instance;

    private void Start() {
        instance = this;
    }

    public bool HostGame() {

    }

    public static string GenerateMatchID() {
        string id = string.Empty;

        for(int i = 0; i < 5; i++)
        {
            int random = Random.Range(0, 36);

            if(random < 26) {
                id += (char)(random + 65);
            } else {
                id += (random - 26).ToString();
            }
        }
        Debug.Log(id);

        return id;
    }
}
