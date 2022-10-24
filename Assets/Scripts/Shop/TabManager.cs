using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabManager : MonoBehaviour
{
    [SerializeField]
    private List<ShopItemSo> shopItemSo;
    [SerializeField]
    private List<GameObject> shopPanelsGo;
    [SerializeField]
    private List<ShopTemplate> shopPanels;
    [SerializeField]
    private List<Button> myBuyButtons;

    [SerializeField]
    private ShopManager shopManager;

    private readonly int delayAmount = 1;
    private float timer;

    void Start()
    {
        for (int i = 0; i < this.shopItemSo.Count; i++)
            this.shopPanelsGo[i].SetActive(true);
        this.LoadPanels();
        this.CheckParchasable();
    }

    void Update()
    {
        this.timer += Time.deltaTime;

        if (this.timer >= this.delayAmount)
        {
            this.timer = 0f;
            this.CheckParchasable();

        }
    }

    private void CheckParchasable()
    {
        
        if (this.shopManager.IsInBorder() && !Inventory.instance.IsEqFull())
        {
            for (int i = 0; i < this.shopItemSo.Count; i++)
            {
                if (this.shopManager.GoldValue >= this.shopItemSo[i].Price)
                    this.myBuyButtons[i].interactable = true;
                else
                    this.myBuyButtons[i].interactable = false;
            }
        }
        else
        {
            for (int i = 0; i < this.shopItemSo.Count; i++)
            {
                this.myBuyButtons[i].interactable = false;
            }
        }
    }

    public void BuyItem(int btnNo)
    {
        if (this.shopManager.GoldValue >= this.shopItemSo[btnNo].Price)
        {
            this.shopManager.Subtract(this.shopItemSo[btnNo].Price);
            Inventory.instance.AddToEquipment(this.shopItemSo[btnNo]);
            this.CheckParchasable();
        }
    }

    private void LoadPanels()
    {
        for (int i = 0; i < this.shopItemSo.Count; i++)
        {
            this.shopPanels[i].TitleTxt.text = this.shopItemSo[i].Title;
            this.shopPanels[i].AttackVal.text = "Attack: " + this.shopItemSo[i].Attack.ToString();
            this.shopPanels[i].MagicVal.text = "Magic: " + this.shopItemSo[i].Magic.ToString();
            this.shopPanels[i].DefenceVal.text = "Defence: " + this.shopItemSo[i].Defence.ToString();
            this.shopPanels[i].PriceVal.text = "Price: " + this.shopItemSo[i].Price.ToString();
            this.shopPanels[i].ItemIm.sprite = this.shopItemSo[i].Image;
        }
    }
}
