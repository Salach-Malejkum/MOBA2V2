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

    public void Subscribe(TabButtons button)
    {
        if(tabButtons == null)
        {
            tabButtons = new List<TabButtons>();
        }
        tabButtons.Add(button);
    }

    public void OnTabEnter(TabButtons button)
    {
        ResetTabs();
        if(selectedTab == null || button != selectedTab)
        {
            button.backGround.sprite = tabHover;
        }
        
    }

    public void OnTabExit(TabButtons button)
    {
        ResetTabs();
    }

    public void OnTabSelected(TabButtons button)
    {
        selectedTab = button;
        ResetTabs();
        button.backGround.sprite = tabActive;
        int index = button.transform.GetSiblingIndex();
        for(int i = 0; i<objectToSwap.Count; i++)
        {
            if(i == index)
            {
                objectToSwap[i].SetActive(true);
            }
            else
            {
                objectToSwap[i].SetActive(false);
            }
        }

    }

    public void ResetTabs()
    {
        foreach(TabButtons button in tabButtons)
        {
            if (selectedTab != null && button == selectedTab)
            {
                continue;
            }
            button.backGround.sprite = tabIdle;
        }
    }

}
