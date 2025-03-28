using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animación_Baul : MonoBehaviour
{

    Animator ani;


    private void Start()
    {
        ani = GetComponent<Animator>();
    }
    private void OnTriggerEnter(Collider other)
    {
        ani.SetBool("Abrir", true);
    }

    private void OnTriggerExit(Collider other)
    {
        ani.SetBool("Abrir", false);
    }
}
