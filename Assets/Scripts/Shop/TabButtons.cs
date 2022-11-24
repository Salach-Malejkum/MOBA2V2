using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Mirror;

[RequireComponent(typeof(Image))]
public class TabButtons : NetworkBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    [SerializeField]
    private TabGroup tabGroup;

    [SerializeField]
    private Image backGround;

    public Image BackGround
    {
        get { return backGround; }
    }

    [Client]
    public void OnPointerClick(PointerEventData eventData)//client
    {
        this.tabGroup.OnTabSelected(this);
    }

    [Client]
    public void OnPointerEnter(PointerEventData eventData)//client
    {
        this.tabGroup.OnTabEnter(this);
    }

    [Client]
    public void OnPointerExit(PointerEventData eventData)//client
    {
        this.tabGroup.OnTabExit(this);
    }

    [Client]
    void Start()//client
    {
        this.backGround = GetComponent<Image>();
        this.tabGroup.Subscribe(this);
    }
}
