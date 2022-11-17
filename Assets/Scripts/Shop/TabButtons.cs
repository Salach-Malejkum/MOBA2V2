using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Image))]
public class TabButtons : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    [SerializeField]
    private TabGroup tabGroup;

    [SerializeField]
    private Image backGround;

    public Image BackGround
    {
        get { return backGround; }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        this.tabGroup.OnTabSelected(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        this.tabGroup.OnTabEnter(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        this.tabGroup.OnTabExit(this);
    }

    void Start()
    {
        this.backGround = GetComponent<Image>();
        this.tabGroup.Subscribe(this);
    }
}
