using System;
using System.Collections.Generic;
using System.Linq;
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
    private int abilityPower;
    public int AbilityPower
    {
        get { return abilityPower; }
    }

    [SerializeField]
    private int armor;
    public int Armor
    {
        get { return armor; }
    }

    [SerializeField]
    private int magicResist;
    public int MagicResist
    {
        get { return magicResist; }
    }

    [SerializeField]
    private int cooldownReduction;
    public int CooldownReduction
    {
        get { return cooldownReduction; }
    }

    [SerializeField]
    private int health;
    public int Health
    {
        get { return health; }
    }

    [SerializeField]
    private int unitPrice;

    [SerializeField]
    private List<ShopItemSo> components;
    public List<ShopItemSo> Components
    {
        get { return components; }
    }

    public int TotalPrice
    {
        get { return unitPrice + components.Sum(component => component.unitPrice); }
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