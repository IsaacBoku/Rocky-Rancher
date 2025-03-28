using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    public List<ItemData_Oficial> startingItems;

    public List<InventoryItem> equipment;
    public Dictionary<ItemData_Equipment, InventoryItem> equipmentDictionary;

    public List<InventoryItem> materialEquip;
    public Dictionary<ItemData_Material, InventoryItem> materialequipDictionary;

    public List<InventoryItem> inventory;
    public Dictionary<ItemData_Oficial, InventoryItem> inventoryDictianory;

    public List<InventoryItem> stash;
    public Dictionary<ItemData_Oficial, InventoryItem> stashDictianory;

    [Header("Inventory UI")]
    [SerializeField] private Transform inventorySlotParent;
    [SerializeField] protected Transform stashSlotParent;
    [SerializeField] private Transform materialSlotParent;
    [SerializeField] private Transform equipmentSlotParent;
    [SerializeField] private Transform statSlotParent;

    private UI_ItemSlot[] inventoryItemSlot;
    private UI_ItemSlot[] stashItemSlot;
    private UI_ItemInventory[] inventorySlot;
    private UI_EquipmentSlot[] equipmentSlot;
    private UI_StatSlot[] statSlot;

    [Header("Items cooldown")]
    private float lastTimeUsedFlask;

    public ItemIndex itemIndex;
    public float flaskCooldown { get; private set; }
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
    private void Start()
    {
        inventory = new List<InventoryItem>();
        inventoryDictianory = new Dictionary<ItemData_Oficial, InventoryItem>();

        stash = new List<InventoryItem>();
        stashDictianory = new Dictionary<ItemData_Oficial, InventoryItem>();

        equipment = new List<InventoryItem>();
        equipmentDictionary = new Dictionary<ItemData_Equipment, InventoryItem>();

        materialEquip = new List<InventoryItem>();
        materialequipDictionary = new Dictionary<ItemData_Material, InventoryItem>();

        inventoryItemSlot = inventorySlotParent.GetComponentsInChildren<UI_ItemSlot>();
        stashItemSlot = stashSlotParent.GetComponentsInChildren<UI_ItemSlot>(); 
        inventorySlot = materialSlotParent.GetComponentsInChildren<UI_ItemInventory>();
        equipmentSlot = equipmentSlotParent.GetComponentsInChildren<UI_EquipmentSlot>();
        statSlot = statSlotParent.GetComponentsInChildren<UI_StatSlot>();
        AddStartingitems();
    }
    private void AddStartingitems()
    {
        for (int i = 0; i < startingItems.Count; i++)
        {
            if (startingItems[i] != null)
                AddItem(startingItems[i]);
        }
    }

    public void UpdateSlotsUI()
    {

        for (int i = 0; i < equipmentSlot.Length; i++)
        {
            foreach (KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDictionary)
            {
                if (item.Key.equipmentType == equipmentSlot[i].slotType)
                    equipmentSlot[i].UpdateSlot(item.Value);

            }

        }

       

        for (int i = 0; i < inventoryItemSlot.Length; i++)
        {
            inventoryItemSlot[i].CleanUpSlot();
        }
        for (int i = 0; i < stashItemSlot.Length; i++)
        {
            stashItemSlot[i].CleanUpSlot();
        }
        for (int i = 0; i < inventory.Count; i++)
        {
            inventoryItemSlot[i].UpdateSlot(inventory[i]);
        }
        for (int i = 0; i < stash.Count; i++)
        {
            stashItemSlot[i].UpdateSlot(stash[i]);
        }
        for (int i = 0; i < statSlot.Length; i++)
        {
            statSlot[i].UpdateStatValueUI();
        }
        for(int i = 0;i < inventorySlot.Length; i++)
        {
            inventorySlot[i].CleanUpSlot();
        }
    }

    public void UpdateSlotMaterial()
    {
        for (int i = 0; i < inventorySlot.Length; i++)
        {
            foreach (KeyValuePair<ItemData_Material, InventoryItem> item in materialequipDictionary)
            {
                if (item.Key.MaterialType == inventorySlot[i].materialSlot)
                    inventorySlot[i].UpdateSlot(item.Value);
            }
        }
    }
    public void UpdateSlotEquip()
    {
        for (int i = 0; i < equipmentSlot.Length; i++)
        {
            foreach (KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDictionary)
            {
                if (item.Key.equipmentType == equipmentSlot[i].slotType)
                    equipmentSlot[i].CleanUpSlot();
          
            }

        }
    }

    public void AddItem(ItemData_Oficial _item)
    {
        if (_item.itemType == ItemType.Equipment && CanAddItem())
            AddToInventory(_item);
        else if (_item.itemType == ItemType.Material)
            AddToStash(_item);

        UpdateSlotsUI();
    }
    private void AddToStash(ItemData_Oficial _item)
    {
        if (stashDictianory.TryGetValue(_item, out InventoryItem value))
        {
            value.AddStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(_item);
            stash.Add(newItem);
            stashDictianory.Add(_item, newItem);
        }
    }
    private void AddToInventory(ItemData_Oficial _item)
    {
        if (inventoryDictianory.TryGetValue(_item, out InventoryItem value))
        {
            value.AddStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(_item);
            inventory.Add(newItem);
            inventoryDictianory.Add(_item, newItem);
        }
    }
    public virtual void RemoveItem(ItemData_Oficial _item)
    {
        if (inventoryDictianory.TryGetValue(_item, out InventoryItem value))
        {
            if (value.stackSize <= 1)
            {
                inventory.Remove(value);
                inventoryDictianory.Remove(_item);
            }
            else
                value.RemoveStack();
        }
        if (stashDictianory.TryGetValue(_item, out InventoryItem stashValue))
        {
            if (stashValue.stackSize <= 1)
            {
                stash.Remove(stashValue);
                stashDictianory.Remove(_item);
            }
            else
                stashValue.RemoveStack();
        }
        UpdateSlotsUI();
    }

    public virtual void RemoveMaterial(ItemData_Material _material)
    {

        if (materialequipDictionary.TryGetValue(_material, out InventoryItem materialvalue))
        {
            if (materialvalue.stackSize <= 1)
            {
                materialEquip.Remove(materialvalue);
                materialequipDictionary.Remove(_material);
            }
            else
                materialvalue.RemoveStack();
        }
        UpdateSlotsUI();
    }
    public void RemoveEquip(ItemData_Equipment _item)
    {
       

        if (equipmentDictionary.TryGetValue(_item, out InventoryItem equipvalue))
           {
            if (equipvalue.stackSize <= 1)
            {
                equipment.Remove(equipvalue);
                equipmentDictionary.Remove(_item);
               
            }
            else
                equipvalue.RemoveStack();
            }
            UpdateSlotEquip();
        
        
        
    }
    public void EquipItem(ItemData_Oficial _item)
    {
        ItemData_Equipment newEquipment = _item as ItemData_Equipment;
        InventoryItem newItem = new InventoryItem(newEquipment);
        ItemData_Equipment oldEquipment = null;

        foreach (KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDictionary)
        {
            if (item.Key.equipmentType == newEquipment.equipmentType)
                oldEquipment = item.Key;
        }
        if (oldEquipment != null)
        {
            UnequipItem(oldEquipment);
            AddItem(oldEquipment);
        }
        equipment.Add(newItem);
        equipmentDictionary.Add(newEquipment, newItem);
        newEquipment.AddModifiers();

        RemoveItem(_item);

        UpdateSlotsUI();
    }
    public void EquipItemMaterial(ItemData_Oficial _item)
    {
        ItemData_Material newItemMaterial = _item as ItemData_Material;
        InventoryItem newMaterial = new InventoryItem(newItemMaterial);
        ItemData_Material oldMaterial = null;

        foreach (KeyValuePair<ItemData_Material, InventoryItem> item in materialequipDictionary)
        {
            if (item.Key.MaterialType == newItemMaterial.MaterialType)
                oldMaterial = item.Key;
        }
        if (oldMaterial != null)
        {
            UnequipMaterial(oldMaterial);
            AddItem(oldMaterial);
        }
        materialEquip.Add(newMaterial);
        materialequipDictionary.Add(newItemMaterial,newMaterial);

        RemoveItem(_item);

        UpdateSlotMaterial();
    }
    public void UnequipMaterial(ItemData_Material materialToRemove)
    {
        if(materialequipDictionary.TryGetValue(materialToRemove, out InventoryItem material))
        {
            materialEquip.Remove(material);
            materialequipDictionary.Remove(materialToRemove);
        }
    }
    public void UnequipItem(ItemData_Equipment itemToRemove)
    {
        if (equipmentDictionary.TryGetValue(itemToRemove, out InventoryItem value))
        {
            equipment.Remove(value);
            equipmentDictionary.Remove(itemToRemove);
            itemToRemove.RemoveModifiers();
        }
    }
    public bool CanAddItem()
    {
        if (inventory.Count >= inventoryItemSlot.Length)
        {
            Debug.Log("No more space");
            return false;
        }
        return true;
    }
    public bool CanCraft(ItemData_Equipment _itemToCraft, List<InventoryItem> _requiredMaterials)
    {
        List<InventoryItem> materialsToRemove = new List<InventoryItem>();

        for (int i = 0; i < _requiredMaterials.Count; i++)
        {
            if (stashDictianory.TryGetValue(_requiredMaterials[i].data, out InventoryItem stashValue))
            {
                if (stashValue.stackSize < _requiredMaterials[i].stackSize)
                {
                    Debug.Log("not enough materials");
                    return false;
                }
                else
                {
                    materialsToRemove.Add(stashValue);
                }
            }
            else
            {
                Debug.Log("not enough materials");
                return false;
            }
        }
        for (int i = 0; i < materialsToRemove.Count; i++)
        {
            RemoveItem(materialsToRemove[i].data);
        }
        AddItem(_itemToCraft);
        Debug.Log("Here is yout item " + _itemToCraft.name);
        return true;
    }
    public List<InventoryItem> GetEquipmentList() => equipment;
    public List<InventoryItem> Getstashlist() => stash;
    public virtual ItemData_Equipment GetEquipment(EquipmentType _type)
    {
        ItemData_Equipment equipedItem = null;

        foreach (KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDictionary)
        {
            if (item.Key.equipmentType == _type)
                equipedItem = item.Key;
        }
        return equipedItem;
    }
    public virtual ItemData_Material GetMaterial(MaterialType _type)
    { 
        ItemData_Material equipedMaterial = null;

        foreach(KeyValuePair<ItemData_Material,InventoryItem> item in materialequipDictionary)
        {
            if(item.Key.MaterialType == _type)
                equipedMaterial = item.Key;
        }
        return equipedMaterial;
    }
    public void UseFlask()
    {
        ItemData_Equipment currentFlask = GetEquipment(EquipmentType.Food);

        if (currentFlask == null)
            return;
        //RemoveEquip(currentFlask);

        bool canUseFlask = Time.time > lastTimeUsedFlask + flaskCooldown;

        if (canUseFlask )
        {
            flaskCooldown = currentFlask.itemCooldown;
            currentFlask.Effect(null);
            lastTimeUsedFlask = Time.time;
        }
        else
            Debug.Log("Flask on cooldown;");
    }

    public void UseHandMaterial()
    {
        ItemData_Material currentMaterial = GetMaterial(MaterialType.Material);
        if(currentMaterial == null) return;
    }
}
