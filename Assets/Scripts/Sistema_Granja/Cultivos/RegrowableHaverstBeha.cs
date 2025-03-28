using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegrowableHaverstBeha : Interacion_Objetos
{
     CropsBehaviour parentCrop;
    public void SetParent(CropsBehaviour parentCrop)
    {
        this.parentCrop = parentCrop;
    }

    public override void Pickup()
    {
        Inventory.instance.AddItem(item);
        parentCrop.ReGrow();

    }
}
