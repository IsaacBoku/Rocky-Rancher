using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interacion_Objetos : MonoBehaviour
{
    public ItemData_Oficial item;
    public int count = 1;

    public UnityEvent onInteract = new UnityEvent();
    public void Set(ItemData_Oficial item, int count)
    {
        this.item = item;
        this.count = count;
    }
   

    public virtual void Pickup()
    {
        onInteract?.Invoke();

        Inventory.instance.AddItem(item);
        Destroy(gameObject);
    }
}
