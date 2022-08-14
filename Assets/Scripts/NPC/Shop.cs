using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public string ShopName;

    private PlayerInventory playerInventory;

    public List<ShopItem> inventory;
    public List<ShopEntry> shopEntries;

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

    public bool shopExists;

    // Start is called before the first frame update
    void Start()
    {
        playerInventory = GameManager.instance.Player.GetComponent<PlayerInventory>();

        CreateShopContent();
    }

    public void CreateShopContent()
    {
        foreach(ShopItem shopItem in inventory)
        {
            GameObject entry = Instantiate(itemSlotPrefab, itemContainer.transform);
            entry.GetComponent<ShopEntry>().item = shopItem.item;
            entry.GetComponent<ShopEntry>().count = shopItem.count;
            shopEntries.Add(entry.GetComponent<ShopEntry>());
        }
    }

    public void SelectItem(Item item)
    {
        var itemInInv = inventory.Find(x => x.item == item);
        var itemInEntries = shopEntries.Find(x => x.item == item);
        int invIdx = inventory.IndexOf(itemInInv);
        int entryIdx = shopEntries.IndexOf(itemInEntries);

        if (mode == ShopMode.Buy)
        {
            // Check if player has enough money and space
            if(playerInventory.coins >= itemInInv.item.Buy_Price && playerInventory.usedSlots < playerInventory.itemSlotCount)
            {
                // Check if player has item
                var i = playerInventory.inventory.Find(x => x.item == item);
                if (i != null)
                {
                    i.count++;
                }
                else
                {
                    // Add item to player inventory 
                    playerInventory.inventory.Add(new CountedItem(item, 1));
                    
                }
                // Deduct coins
                playerInventory.coins -= item.Buy_Price;

                // Decrement item count
                inventory[invIdx].count--;
                shopEntries[entryIdx].count--;

                // Check shop item count, if 0 remove item
                if(inventory[invIdx].count <= 0)
                {
                    inventory.RemoveAt(invIdx);
                    shopEntries.RemoveAt(entryIdx);
                }
            }
            // Otherwise don't buy
            return;
        }
        else if (mode == ShopMode.Sell)
        {
            // Check if player has item
            var checkItem = playerInventory.inventory.Find(x => x.item == item);
            if(checkItem != null)
            {
                // Decrement item count
                checkItem.count--;
                
                if(checkItem.count == 0)
                {
                    playerInventory.inventory.Remove(checkItem);
                }

                // Add sell value to player money
                playerInventory.coins += checkItem.item.Sell_Price;
            }
        }
    }

    public void ExitShop()
    {
        // Disable the shop 
        gameObject.SetActive(false);

        // End the NPC interaction
        GameManager.instance.UIManager.currentNPC.GetComponent<NPC>().OnFinshInteraction();
    }
}
