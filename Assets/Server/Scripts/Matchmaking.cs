using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

[System.Serializable]
public class Match {
    public string matchID;
    readonly SyncListObjects players = new SyncListObjects();

    public Match(string matchID, GameObject hostPlayer) {
        this.matchID = matchID;
        players.Add(hostPlayer);
    }

    public Match() { }
}

[System.Serializable]
public class SyncListObjects : SyncList<GameObject> { }

[System.Serializable]
public class SyncListMatches : SyncList<Match> { }

public class Matchmaking : NetworkBehaviour
{
    public static Matchmaking instance;

    public readonly SyncListMatches matches = new SyncListMatches();
    public readonly SyncList<string> matchIDs = new SyncList<string>();

    private void Start() {
        instance = this;
    }

    public bool HostGame(string matchID, GameObject hostPlayer) {
        if(!matchIDs.Contains(matchID)) {
            return false;
        } else {
            matchIDs.Add(matchID);
            matches.Add(new Match(matchID, hostPlayer));
            return true;
        }
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
