using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Itemobject_Trigger : MonoBehaviour
{
    private ItemObject myItemObject => GetComponentInParent<ItemObject>();
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.GetComponent<Player_Move>() != null)
        {
            if (collision.GetComponent<CharacterStats>().isDead)
                return;
            myItemObject.PickupItem();
        }
    }
}
