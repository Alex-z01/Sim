using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    public Item attachedItem;
    public Image itemIcon;
    public Text itemCount;

    public void SetCountText(int count)
    {
        itemCount.text = $"{count}";
    }

    public void SetIcon(Sprite icon)
    {
        itemIcon.sprite = icon;
    }

    public void SetAttachedItem(Item item)
    {
        attachedItem = item;
    }

    public void UseItem()
    {
        if(attachedItem != null)
        {
            print(attachedItem.Item_Name);
            if (attachedItem.GetType() == typeof(Equippable))
            {
                var equippable = (Equippable)attachedItem;

                GameManager.instance.Player.GetComponent<PlayerInventory>().EquipGear(equippable);

                EmptySlot();
            }
        }
    }

    public void EmptySlot()
    {
        attachedItem = null;
        itemIcon.sprite = null;
        itemCount.text = "";
    }
}
