using System;
using UnityEngine;

[Serializable]
public class CountedItem
{
    public Item item;
    public int count;

    public CountedItem(Item item, int count)
    {
        this.item = item;
        this.count = count;
    }
}
