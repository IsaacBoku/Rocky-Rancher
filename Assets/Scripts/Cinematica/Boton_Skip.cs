using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Boton_Skip : MonoBehaviour
{
    public void Skip_Scene()
    {
        SceneManager.LoadScene("Nombre_Jugador");
    }
}
