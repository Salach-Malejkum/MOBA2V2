using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName ="ShopMenu", menuName = "scriptable Objects/New Base Shop Item", order = 1)]
public class ShopItemSo : ScriptableObject
{
    [SerializeField]
    private string title;
    public string Title
    {
        get { return title; }
    }

    [SerializeField]
    private int attack;
    public int Attack
    {
        get { return attack; }
    }

    [SerializeField]
    private int magic;
    public int Magic
    {
        get { return magic; }
    }

    [SerializeField]
    private int defence;
    public int Defence
    {
        get { return defence; }
    }

    [SerializeField]
    private int price;
    public int Price
    {
        get { return price; }
    }

    [SerializeField]
    private Sprite image;
    public Sprite Image
    {
        get { return image; }
    }

    public virtual void OnItemUse()
    {
    }

}

[Serializable]
[CreateAssetMenu(fileName = "ShopMenu", menuName = "scriptable Objects/New Shop Item custom message", order = 2)]
public class ItemTypeOne : ShopItemSo
{
    [SerializeField]
    private string myMassage;
    public string MyMassage
    {
        get { return myMassage; }
    }

    public override void OnItemUse()
    {
        //TODO Umiejêtnoœci przedmiotów u¿ywanych
        Debug.Log(this.myMassage);
    }
}