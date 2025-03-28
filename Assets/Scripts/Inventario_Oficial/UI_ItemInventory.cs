using UnityEngine.EventSystems;

public class UI_ItemInventory : UI_ItemSlot
{
    public static UI_ItemInventory Instance;

    public MaterialType materialSlot;

    ShippingBin money;
    private void OnValidate()
    {
        gameObject.name = "Equipment slot - " + materialSlot.ToString();
    }
    public override void OnPointerDown(PointerEventData eventData)
    {
        if (item == null || item.data == null)
            return;

        Inventory.instance.UnequipMaterial(item.data as ItemData_Material);
        Inventory.instance.AddItem(item.data as ItemData_Material);

        ui.itemToolTip.HideToolTip();
        CleanUpSlot();
    }
    public virtual void borrar()
    {
        CleanUpSlot();
    }
}
