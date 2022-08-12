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
            itemPrice.text = $"${item.Buy_Price}";
        else if (myShop.mode == Shop.ShopMode.Sell)
            itemPrice.text = $"${item.Sell_Price}";

        itemIcon.sprite = item.Item_Icon;
    }

    public void SelectItem()
    {
        myShop.SelectItem(item);
        Refresh();
    }

    public void Refresh()
    {
        itemCount.text = count.ToString();

        if (myShop.mode == Shop.ShopMode.Buy)
            itemPrice.text = $"${item.Buy_Price}";
        else if (myShop.mode == Shop.ShopMode.Sell)
            itemPrice.text = $"${item.Sell_Price}";

        if (count == 0)
        {
            Destroy(gameObject);
        }
    }
}
