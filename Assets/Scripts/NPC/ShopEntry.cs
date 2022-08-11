using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopEntry : MonoBehaviour
{
    public Item item;
    public int count;

    public Text itemName, itemDesc, itemCount, itemPrice;
    public Image itemIcon;

    private Shop myShop;

    private void Start()
    {
        myShop = GetComponentInParent<Shop>();

        itemName.text = item.Item_Name + ":";
        itemDesc.text = item.Item_Description;
        itemCount.text = count.ToString();

        if (myShop.mode == Shop.ShopMode.Buy)
            itemPrice.text = item.Buy_Price.ToString();
        else if (myShop.mode == Shop.ShopMode.Sell)
            itemPrice.text = item.Sell_Price.ToString();

        itemIcon.sprite = item.Item_Icon;
    }
}
