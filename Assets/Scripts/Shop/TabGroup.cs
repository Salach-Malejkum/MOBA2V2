using Mirror;
using System.Collections.Generic;
using UnityEngine;

public class TabGroup : MonoBehaviour
{
    [SerializeField]
    private List<TabButtons> tabButtons;

    [SerializeField]
    private Sprite tabIdle;

    [SerializeField]
    private Sprite tabHover;

    [SerializeField]
    private Sprite tabActive;

    [SerializeField]
    private TabButtons selectedTab;

    [SerializeField]
    private List<GameObject> objectToSwap;

    //[Client]
    public void Subscribe(TabButtons button)
    {
        if (this.tabButtons == null)
        {
            this.tabButtons = new List<TabButtons>();
        }
        this.tabButtons.Add(button);
    }

    //[Client]
    public void OnTabEnter(TabButtons button)
    {
        this.ResetTabs();
        if (this.selectedTab == null || button != this.selectedTab)
        {
            button.BackGround.sprite = this.tabHover;
        }
        
    }

    //[Client]
    public void OnTabExit(TabButtons _) => this.ResetTabs(); 

    //[Client]
    public void OnTabSelected(TabButtons button)
    {
        this.selectedTab = button;
        this.ResetTabs();
        button.BackGround.sprite = this.tabActive;
        int index = button.transform.GetSiblingIndex();
        for (int i = 0; i < this.objectToSwap.Count; i++)
        {
            if (i == index)
            {
                this.objectToSwap[i].SetActive(true);
            }
            else
            {
                this.objectToSwap[i].SetActive(false);
            }
        }

    }

    //[Client]
    private void ResetTabs()
    {
        foreach (TabButtons button in this.tabButtons)
        {
            if (this.selectedTab != null && button == this.selectedTab)
            {
                continue;
            }
            button.BackGround.sprite = tabIdle;
        }
    }

}
