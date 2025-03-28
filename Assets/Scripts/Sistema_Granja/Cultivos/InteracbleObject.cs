using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteracbleObject : MonoBehaviour
{
    public ItemData_Oficial item;
    public int count = 1;
    public void Set(ItemData_Oficial item, int count)
    {
        this.item = item;
        this.count = count;
    }
    public  virtual void Pickup()
    {
        Destroy(gameObject);
    }
}
