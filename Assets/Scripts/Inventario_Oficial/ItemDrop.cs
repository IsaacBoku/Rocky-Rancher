using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    [SerializeField] private int possibleItemDrop;
    [SerializeField] private ItemData_Oficial[] possibleDrop;
    private List<ItemData_Oficial> dropList = new List<ItemData_Oficial>();
    [SerializeField] private GameObject dropPrefab;
    public virtual void GenerateDrop()
    {
        for (int i = 0; i < possibleDrop.Length; i++)
        {
            if ((Random.Range(0, 100) <= possibleDrop[i].dropChance))
                dropList.Add(possibleDrop[i]);
        }
        for (int i = 0; i < possibleItemDrop; i++)
        {
            ItemData_Oficial randomItem = dropList[Random.Range(0, dropList.Count - 1)];

            dropList.Remove(randomItem);
            DropItem(randomItem);
        }
    }
    protected void DropItem(ItemData_Oficial _itemData)
    {
        GameObject newDrop = Instantiate(dropPrefab, transform.position, Quaternion.identity);

        Vector3 randomVelocity = new Vector3(Random.Range(-5, 5), Random.Range(15, 20));

        newDrop.GetComponent<ItemObject>().SetupItem(_itemData, randomVelocity);
    }
}
