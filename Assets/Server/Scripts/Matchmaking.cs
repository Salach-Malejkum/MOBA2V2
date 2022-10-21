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

    private const string queueName = "DebugQueue";
    private string matchTicketId;

    private Coroutine pollTicketCoroutine;

    public void StartMatchmaking() {
        playButton.SetActive(false);
        queueStatusText.text = "Submitting ticker";
        queueStatusText.gameObject.SetActive(true);

        PlayFabMultiplayerAPI.CreateMatchmakingTicket(
            new CreateMatchmakingTicketRequest {
                Creator = new MatchmakingPlayer {
                    Entity = new EntityKey
                    {
                        Id = UserAuthorization.EntityId,
                        Type = "title_player_account"
                    },
                    Attributes = new MatchmakingPlayerAttributes
                    {
                        DataObject = new 
                        {
                            Latencies = new {
                                NorthEurope = 400
                            }
                         }
                    },
                },
                GiveUpAfterSeconds = 120,
                QueueName = queueName
            },
            OnMatchTicketCreated,
            OnMatchmakingError
        );
    }

    private void OnMatchTicketCreated(CreateMatchmakingTicketResult result) {
        matchTicketId = result.TicketId;
        pollTicketCoroutine = StartCoroutine(PollTicket(result.TicketId));
        leaveQueueButton.SetActive(true);
        queueStatusText.text = "Created ticket";
    }

    private IEnumerator PollTicket(string ticketId) {
        while(true) {
            PlayFabMultiplayerAPI.GetMatchmakingTicket(
                new GetMatchmakingTicketRequest
                {
                    TicketId = ticketId,
                    QueueName = queueName
                },
                OnGetMatchTicket,
                OnMatchmakingError
            );
            yield return new WaitForSeconds(6f);
        }
    }

    private void OnGetMatchTicket(GetMatchmakingTicketResult result) {
        queueStatusText.text = $"Status: {result.Status}";

        switch(result.Status)
        {
            case "Matched":
                StopCoroutine(pollTicketCoroutine);
                StartMatch(result.MatchId);
                break;
            case "Canceled":
                break;
        }
    }

    private void StartMatch(string matchId) {
        queueStatusText.text = $"Starting match";
        PlayFabMultiplayerAPI.GetMatch(
            new GetMatchRequest
            {
                MatchId = matchId,
                QueueName = queueName
            },
            OnGetMatch,
            OnMatchmakingError
        );
    }

    private void OnGetMatch(GetMatchResult result) {
        NetworkManagerLobby.Instance.StartClient();
        NetworkManagerLobby.Instance.networkAddress = result.ServerDetails.IPV4Address;
        NetworkManagerLobby.Instance.GetComponent<kcp2k.KcpTransport>().Port = (ushort) result.ServerDetails.Ports[0].Num; 
    }

    public void CancelMatchmaking() {
        PlayFabMultiplayerAPI.CancelMatchmakingTicket(
            new CancelMatchmakingTicketRequest
            {
                QueueName = queueName,
                TicketId = matchTicketId
            },
            OnCancelMatchTicket,
            OnMatchmakingError
        ); 
    }

    private void OnCancelMatchTicket(CancelMatchmakingTicketResult result) {
        playButton.SetActive(true);
        leaveQueueButton.SetActive(false);
        queueStatusText.text = "";
    }

    private void OnMatchmakingError(PlayFabError error) {
        playButton.SetActive(true);
        leaveQueueButton.SetActive(false);
        queueStatusText.text = "";
        Debug.Log(error.GenerateErrorReport());
    }
}
