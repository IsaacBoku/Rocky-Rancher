using System.Text;
using UnityEngine;

public enum ItemType
{
    Material, Equipment
}
[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Item")]
public class ItemData_Oficial : ScriptableObject
{
    public ItemType itemType;
    public string itemName;
    public Sprite icon;
    public GameObject gameModel;


    [Range(0, 100)]
    public float dropChance;

    protected StringBuilder sb = new StringBuilder();
    public int cost;
    public virtual string GetDescription()
    {
        return "";
    }
}

