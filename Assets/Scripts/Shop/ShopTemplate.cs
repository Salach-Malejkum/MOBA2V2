using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopTemplate : MonoBehaviour
{
    [SerializeField]
    private TMP_Text titleTxt;
    public TMP_Text TitleTxt
    {
        get { return titleTxt; }
    }

    [SerializeField]
    private Image itemIm;
    public Image ItemIm
    {
        get { return itemIm; }
    }

    [SerializeField]
    private TMP_Text attackVal;
    public TMP_Text AttackVal
    {
        get { return attackVal; }
    }

    [SerializeField]
    private TMP_Text magicVal;
    public TMP_Text MagicVal
    {
        get { return magicVal; }
    }

    [SerializeField]
    private TMP_Text defenceVal;
    public TMP_Text DefenceVal
    {
        get { return defenceVal; }
    }

    [SerializeField]
    private TMP_Text priceVal;
    public TMP_Text PriceVal
    {
        get { return priceVal; }
    }

}
