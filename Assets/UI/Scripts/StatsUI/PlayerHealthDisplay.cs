using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthDisplay : MonoBehaviour
{
    private PlayerStats playerStats = null;
    [SerializeField] private Image HealthBarImage;
    private RectTransform transf;

    private void Awake()
    {
        foreach (NetworkIdentity player in NetworkManagerLobby.Instance.PlayersLoadedToScene)
        {
            if (player.isLocalPlayer)
            {
                playerStats = player.gameObject.GetComponent<PlayerStats>();
            }
        }
        this.playerStats.OnUnitHealthUpdate += LocalPlayerHealthUpdated;
        this.playerStats.OnUnitMaxHealthUpdate += LocalPlayerHealthUpdated;
        this.transf = GetComponent<RectTransform>();
    }

    private void OnDestroy()
    {
        this.playerStats.OnUnitMaxHealthUpdate -= LocalPlayerHealthUpdated;
        this.playerStats.OnUnitHealthUpdate -= LocalPlayerHealthUpdated;
    }

    private void Update()
    {
        this.transf.rotation = Quaternion.Euler(-45f, 0, 0);
    }

    private void LocalPlayerHealthUpdated(float newCurrHP, float newMaxHP)
    {
        this.HealthBarImage.fillAmount = newCurrHP / newMaxHP;
    }
}
