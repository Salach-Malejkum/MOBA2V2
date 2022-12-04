using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName ="ShopMenu", menuName = "Scriptable Objects/New Base Shop Item", order = 1)]
public class ShopItemSo : ScriptableObject
{
    [SerializeField]
    private string title;
    public string Title
    {
        get { return title; }
    }

    [SerializeField]
    private float attack;
    public float Attack
    {
        get { return attack; }
    }

    [SerializeField]
    private float abilityPower;
    public float AbilityPower
    {
        get { return abilityPower; }
    }

    [SerializeField]
    private float armor;
    public float Armor
    {
        get { return armor; }
    }

    [SerializeField]
    private float magicResist;
    public float MagicResist
    {
        get { return magicResist; }
    }

    [SerializeField]
    private float cooldownReduction;
    public float CooldownReduction
    {
        get { return cooldownReduction; }
    }

    [SerializeField]
    private float health;
    public float Health
    {
        get { return health; }
    }

    [SerializeField]
    private float unitPrice;

    [SerializeField]
    private List<ShopItemSo> components;
    public List<ShopItemSo> Components
    {
        get { return components; }
    }

    public float TotalPrice
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