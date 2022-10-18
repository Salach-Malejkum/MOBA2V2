using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.MultiplayerModels;
using TMPro;

public class Matchmaking : MonoBehaviour
{
    [SerializeField] private GameObject playButton;
    [SerializeField] private GameObject leaveQueueButton;
    [SerializeField] private TMP_Text queueStatusText;

    private const string QueueName = "DefaultQueue";

    public void StartMatchmaking() {
        playButton.SetActive(false);
        queueStatusText.text = "Submitting ticker";
        queueStatusText.gameObject.SetActive(true);

        // PlayFabMultiplayerAPI.CreateMatchmakingTicket(
        //     new CreateMatchmakingTicketRequest {
        //         Creator = new MatchmakingPlayer {
        //             Entity = new EntityKey
        //             {
        //                 Id = UserAuthorization.EntityId,
        //                 Type = "title_player_account"
        //             },
        //             Attributes = new MatchmakingPlayerAttributes
        //             {
        //                 DataObject = new { }
        //             },
        //         },
        //         GiveUpAfterSeconds = 120,
        //         QueueName = QueueName
        //     }
        // )
    }
}
