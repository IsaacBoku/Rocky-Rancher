using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopListing_Manager : Listingmanager<ItemData_Oficial>
{

    ItemData_Oficial itemToBuy;
    int quantity;

    [Header("Confirmation Screen")]
    [SerializeField] private Image itemThumbnail;
    public GameObject confirmationScreen;
    public TextMeshProUGUI confirmationPrompt;
    public TextMeshProUGUI quantityText;
    public TextMeshProUGUI costCalculationText;
    public Button purchaseItem;

    public GameObject closedWindow;

    protected override void DisplayListing(ItemData_Oficial listingItem, GameObject listingGameObject)
    {
        listingGameObject.GetComponent<ShopListing>().Display(listingItem);
        Cursor.visible = true;
    }
    public void OpenConfirmationScreen(ItemData_Oficial item)
    {
        itemToBuy = item;
        quantity = 1;
        RenderConfirmationScreen();
    }
    public void RenderConfirmationScreen()
    {
        Cursor.visible = true;
        confirmationScreen.SetActive(true);
        confirmationPrompt.text = $"Buy{itemToBuy.name}?";

        //quantityText.text = "x" + quantity;

        int cost = itemToBuy.cost * quantity;

        int playerMoneyLeft = Player_Stats_Buy.Money - cost;

        if(playerMoneyLeft < 0)
        {
            costCalculationText.text = "Insufficient funds.";
            purchaseItem.interactable = false;
            return;
        }
        purchaseItem.interactable = true;
        costCalculationText.text = $"{Player_Stats_Buy.Money} > {playerMoneyLeft}";
    }
    public void AddQuantity()
    {
        quantity++;
        RenderConfirmationScreen();
    }
    public void RemoveQuantity()
    {
        if(quantity > 1)
        {
            quantity--;
        }
        RenderConfirmationScreen() ;
    }
    public void ConfirmPucharse()
    {
        Shop.Purchase(itemToBuy, quantity);
   
        confirmationScreen.SetActive(false);
    }
    public void CancelPurchase()
    {
       
        confirmationScreen.SetActive(false);
    }
    public void CerrarVentana()
    {
        Cursor.visible=false;
        gameObject.SetActive(false);
    }
}