using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationEntryPoint : MonoBehaviour
{
    [SerializeField]
    Trasition_Scenes.Locations locationToSwitch;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("toco");
            Trasition_Scenes.instance.SwitchLocation(locationToSwitch);
        }

        if(other.tag =="Item")
            Destroy(other.gameObject);
    }
}
