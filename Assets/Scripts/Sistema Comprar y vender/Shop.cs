using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : Interacion_Objetos
{
    [SerializeField] CharacterData owner;

    public List<ItemData_Oficial> shopItems;

    [Header("Dialogue")]
    public List<DialogueLine> dialogueOnShopOpen;

    public static void Purchase(ItemData_Oficial itemData, int quantity)
    {
        int totalcost = itemData.cost * quantity;
        if(Player_Stats_Buy.Money >= totalcost)
        {
            Player_Stats_Buy.Spend(totalcost);
            Inventory.instance.AddItem(itemData);      
        }
    }
    public override void Pickup()
    {
        if (!IsStoreManned())
        {
            DialogueManager.instance.StartDialogue(DialogueManager.CreateSimpleMessage(" The store is closed right now. It opens from 9AM to 6PM"));

            return;
        }

        UIManager.Instance.OpenShop(shopItems);
    }
    bool IsStoreManned()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 4);
        foreach(Collider collider in colliders)
        {
            if (collider.tag != "Item") continue;

            InteractableCharacter characterInteractable = collider.gameObject.GetComponent<InteractableCharacter>();
            if (characterInteractable == null) continue;
            if(characterInteractable.characterData.name == owner.name) return true;
        }
        return false;
    }

    public void CerrarTienda()
    {
        gameObject.SetActive(false);
    }
}
