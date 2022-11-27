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
    private TMP_Text priceVal;
    public TMP_Text PriceVal
    {
        get { return priceVal; }
    }
}
