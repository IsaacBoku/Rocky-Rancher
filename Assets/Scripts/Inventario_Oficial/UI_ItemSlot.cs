using UnityEngine.UI;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class UI_ItemSlot : MonoBehaviour,  IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    public static UI_ItemSlot instance;
    [SerializeField] protected Image itemImage;
    [SerializeField] protected TextMeshProUGUI itemText;

    public InventoryItem item;
    protected UI ui;
    protected virtual void Start()
    {
        ui = GetComponentInParent<UI>();
        
    }
    private void Update()
    {

    }
    public void UpdateSlot(InventoryItem _newItem)
    {
        item = _newItem;
        itemImage.color = Color.white;
        if (item != null)
        {
            itemImage.sprite = item.data.icon;
            if (item.stackSize > 1)
                itemText.text = item.stackSize.ToString();
            else
                itemText.text = "";
        }
    }
    public virtual void CleanUpSlot()
    {
        item = null;
        itemImage.sprite = null;
        itemImage.color = Color.clear;
        itemText.text = "";
    }
    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (item == null)
            return;

        if (item.data.itemType == ItemType.Equipment)
            Inventory.instance.EquipItem(item.data);

       else if(item.data.itemType == ItemType.Material)
          Inventory.instance.EquipItemMaterial(item.data);

        ui.itemToolTip.HideToolTip();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Shot Item Info");
        if (item == null)
            return;

        Vector2 mousePosition = Input.mousePosition;

        float xOffset = 0;
        float yOffset = 0;

        if (mousePosition.x > 600)
            xOffset = -200;
        else
            xOffset = 200;

        if (mousePosition.y > 320)
            yOffset = -200;
        else
            yOffset = 200;

        ui.itemToolTip.ShowToolTip(item.data as ItemData_Equipment);
        ui.itemToolTip.transform.position = new Vector2(mousePosition.x + xOffset, mousePosition.y + yOffset);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Hide item info");
        if (item == null) return;
        ui.itemToolTip.HideToolTip();
    }
}
