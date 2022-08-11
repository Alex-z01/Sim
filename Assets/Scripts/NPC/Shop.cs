using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public List<ShopItem> inventory;

    [Serializable]
    public class ShopItem
    {
        public Item item;
        public int count;
    }

    public GameObject itemContainer;
    public GameObject itemSlotPrefab;

    public enum ShopMode { Buy, Sell, BuyBack };
    public ShopMode mode;

    // Start is called before the first frame update
    void Start()
    {
        CreateShopContent();
    }

    public void CreateShopContent()
    {
        foreach(ShopItem shopItem in inventory)
        {
            GameObject entry = Instantiate(itemSlotPrefab, itemContainer.transform);
            entry.GetComponent<ShopEntry>().item = shopItem.item;
            entry.GetComponent<ShopEntry>().count = shopItem.count;
        }
    }

    public void SelectItem()
    {
        if (mode == ShopMode.Buy)
        {
            // Check if player has enough money

            // Add item to player inventory, deduct money

            // Decrement item count

            // Check item count, if 1 remove item

        }
        else if (mode == ShopMode.Sell)
        {
            // Check if player has item

            // Check count, if 1 remove item, otherwise decrement count

            // Add sell value to player money
        }
    }

    public void ExitShop()
    {
        GameManager.instance.UIManager.currentNPC.GetComponent<NPC>().OnFinshInteraction();
        Destroy(this.gameObject);
    }
}


