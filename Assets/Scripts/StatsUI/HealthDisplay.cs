using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    [SerializeField] private UnitStats unitStats;
    [SerializeField] private Image HealthBarImage;
    private RectTransform transf;

    private void Awake()
    {
        this.unitStats.OnUnitMaxHealthUpdate += LocalPlayerHealthUpdated;
        this.unitStats.OnUnitHealthUpdate += LocalPlayerHealthUpdated;
        this.transf = GetComponent<RectTransform>();
    }

    private void OnDestroy()
    {
        this.unitStats.OnUnitMaxHealthUpdate -= LocalPlayerHealthUpdated;
        this.unitStats.OnUnitHealthUpdate -= LocalPlayerHealthUpdated;
    }

    private void FixedUpdate()
    {
        this.transf.rotation = Quaternion.Euler(-45f, 0, 0);
    }

    private void LocalPlayerHealthUpdated(float newCurrHP, float newMaxHP)
    {
        this.HealthBarImage.fillAmount = newCurrHP / newMaxHP;
    }
}
