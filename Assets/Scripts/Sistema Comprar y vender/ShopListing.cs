using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class ShopListing : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image itemThumbnail;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI costText;
    ItemData_Oficial itemData;
    public void Display(ItemData_Oficial itemData)
    {
        this.itemData = itemData;
        itemThumbnail.sprite = itemData.icon;
        nameText.text = itemData.name;
        costText.text = itemData.cost + Player_Stats_Buy.CURRENCY;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        UIManager.Instance.shopListingManager.OpenConfirmationScreen(itemData);
    }
}
