using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName ="ShopMenu", menuName = "scriptable Objects/New Base Shop Item", order = 1)]
public class ShopItemSo : ScriptableObject
{
    public string title;
    public int attack;
    public int magic;
    public int defence;
    public int price;
    public Sprite image;

    public virtual void OnUse()
    {
        Debug.Log("base");
    }
}

[Serializable]
[CreateAssetMenu(fileName = "ShopMenu", menuName = "scriptable Objects/New Shop Item custom message", order = 2)]
public class ItemTypeOne : ShopItemSo
{
    public string myMassage;
    public override void OnUse()
    {
        Debug.Log(myMassage);
    }
}