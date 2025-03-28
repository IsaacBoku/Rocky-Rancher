using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Item Index")]
public class ItemIndex : ScriptableObject
{
   public  List<ItemData_Oficial> items;
    public ItemData_Oficial GetItemFromString(string name)
    {
        return items.Find(i => i.name == name);
    }
}
