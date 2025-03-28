using System;

[Serializable]
public class InventoryItem 
{
    public ItemData_Oficial data;

    public int stackSize;
    public InventoryItem(ItemData_Oficial _newItemData)
    {
        data = _newItemData;
        //add to stack
        AddStack();
    }
    public void AddStack() => stackSize++;
    public void RemoveStack() => stackSize--;
}
