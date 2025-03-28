using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerButton : MonoBehaviour
{
   public  GameObject canvasUI;
    Animator ani;

    private void Start()
    {
        ani = GetComponent<Animator>();
    }
    private void OnTriggerEnter(Collider other)=> canvasUI.SetActive(true);
    private void OnTriggerExit(Collider other)
    {
        
        canvasUI.SetActive(false);
    }
}
