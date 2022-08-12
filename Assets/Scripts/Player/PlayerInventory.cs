using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{
    public Equippable headGear;
    public Equippable bodyGear;

    [SerializeField]
    public List<EquippableSocket> sockets;
    public EquippableSocket bodySocket;
    public EquippableSocket headSocket;

    private PlayerMovement PlayerMovement;

    public List<CountedItem> inventory;
    public List<Equippable> equippedInventory;

    public GameObject inventoryObj;
    public GameObject inventorySlotContainer;
    public GameObject inventorySlotObj;

    public Text coinsText;

    public int itemSlotCount;
    public int usedSlots;

    public int coins;

    private void Start()
    {
        PlayerMovement = GetComponent<PlayerMovement>();
        coins = 100;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            print("Open inv");

            PlayerMovement.Busy = !PlayerMovement.Busy ? true : false;
            PlayerMovement.CanMove = PlayerMovement.CanMove ? false : true;

            bool invActive = inventoryObj.activeSelf ? false : true;
            inventoryObj.SetActive(invActive);

            if (invActive)
            {
                coinsText.text = $"${coins}";
                SpawnItemSlots();
            }
            else
            {
                DeleteItemSlots();
            }     
        }
    }

    public void AddItem(Item item)
    {
        var itemExists = inventory.Find(x => x.item == item);

        if(itemExists != null)
        {
            print("Item already exists");
            itemExists.count += 1;
            return;
        }
        CountedItem cItem = new CountedItem(item, 1);
        inventory.Add(cItem);
    }

    void SpawnItemSlots()
    {
        usedSlots = 0;
        for(int i = 0; i < itemSlotCount; i++)
        {
            GameObject itemSlot = Instantiate(inventorySlotObj, inventorySlotContainer.transform);
            itemSlot.name = $"Slot {i}";

            if(inventory.Count > 0 && usedSlots < inventory.Count)
            {
                PopulateSlot(itemSlot, i);
            }
        }
    }

    void DeleteItemSlots()
    {
        usedSlots = 0;
        foreach(Transform slot in inventorySlotContainer.transform)
        {
            Destroy(slot.gameObject);
        }
    }

    void PopulateSlot(GameObject slot, int idx)
    {
        try
        {
            var cItem = inventory[idx];

            if (cItem != null)
            {
                usedSlots++;

                slot.GetComponent<ItemSlot>().SetAttachedItem(cItem.item);
                slot.GetComponent<ItemSlot>().SetIcon(cItem.item.Item_Icon);
                slot.GetComponent<ItemSlot>().SetCountText(cItem.count);
            }
        }
        catch(IndexOutOfRangeException e)
        {
            print(e);
            return;
        }
    }

    public void EquipGear(Equippable equippable)
    {
        // Designate gear as equipped
        equippable.equipped = true;

        // Check gear type
        if (equippable.type == Equippable.EquipType.Head)
        {
            // Find item in inventory
            var equippedItem = inventory.Find(x => x.item == equippable);

            // Add to equipped inv
            equippedInventory.Add((Equippable)equippedItem.item);

            inventory.Remove(equippedItem);

            // Check if corressponding socket exists and this item has animations
            if (headSocket != null && equippable.AnimationClips != null)
            {
                // Visually equip to socket
                headSocket.Equip(equippable.AnimationClips);
            }
        }
    }

    public void UnEquipGear(Equippable equippable)
    {
        equippable.equipped = false;
        if (equippable.type == Equippable.EquipType.Head)
        {
            if (headSocket != null && equippable.AnimationClips != null)
            {
                headSocket.UnEquip();
            }
        }
    }
}
