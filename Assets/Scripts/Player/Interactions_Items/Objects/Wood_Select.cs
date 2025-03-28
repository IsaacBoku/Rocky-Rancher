using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wood_Select : MonoBehaviour
{
    [SerializeField] GameObject PickItemDrop;
    [SerializeField] ItemData_Oficial item;
    public GameObject selectPlace;
    private void Start()
    {
        Select(false);
    }
    public void Select(bool toggle)
    {
        selectPlace.SetActive(toggle);
    }
    public void InteractWood()
    {
        
    }
}
