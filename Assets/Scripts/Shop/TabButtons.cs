using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Mirror;

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

    //[Client]
    public void OnPointerClick(PointerEventData eventData)
    {
        this.tabGroup.OnTabSelected(this);
    }

    //[Client]
    public void OnPointerEnter(PointerEventData eventData)
    {
        this.tabGroup.OnTabEnter(this);
    }

    //[Client]
    public void OnPointerExit(PointerEventData eventData)
    {
        this.tabGroup.OnTabExit(this);
    }

    //[ClientCallback]
    void Start()
    {
        this.backGround = GetComponent<Image>();
        this.tabGroup.Subscribe(this);
    }
}
