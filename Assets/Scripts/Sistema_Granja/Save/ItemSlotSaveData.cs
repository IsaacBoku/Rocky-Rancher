using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemSlotSaveData 
{
    public string itemID;
    public int quantity;

    public ItemSlotSaveData(InventoryItem data)
    {
        if (data.stackSize == 0)
        {
            itemID = null;
            quantity = 0;
            return;
        }
        itemID = data.data.name;
        quantity = data.stackSize;
    }



}
