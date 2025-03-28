using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShippingBin : Interacion_Objetos
{
    public static int hourToShip = 18;
    public static List<InventoryItem> itemsToShip = new List<InventoryItem>();


    public override void Pickup()
    {
        ItemData_Oficial handSlot = Inventory.instance.GetMaterial(MaterialType.Material);
        if (handSlot == null)
        {
            DialogueManager.instance.StartDialogue(DialogueManager.CreateSimpleMessage(" You don't have anything in your hand"));
            return;
        }
            
        
        UIManager.Instance.TriggerYesNoPrompt($"Do you want to sell {handSlot.name} ? ", PlaceItemsInShippingBin);


    }
    public virtual void PlaceItemsInShippingBin()
    {
        ItemData_Material currentItem = Inventory.instance.GetMaterial(MaterialType.Material);
        itemsToShip.Add( new InventoryItem(currentItem));
        Inventory.instance.RemoveMaterial(currentItem);
        foreach ( InventoryItem item in itemsToShip )
            Debug.Log($"{item.data.name} x {item.stackSize}");
    }
    public static void ShipItems()
    {
        int moneyToReceive = TallyItems(itemsToShip);

        Player_Stats_Buy.Earn(moneyToReceive);

        itemsToShip.Clear();
    }
    static int TallyItems(List<InventoryItem> items)
    {
        int netTotal = 0;
        foreach ( InventoryItem item in items )
        {
            netTotal += item.stackSize * item.data.cost;
        }
        return netTotal;
    }
}
