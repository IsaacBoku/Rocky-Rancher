using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectName : MonoBehaviour
{
    public InputField inputText;

    public Text textName;
    public Image luz;
    public GameObject botonAceptar;

    private void Awake()
    {
        luz.color = Color.red;
    }
    private void Update()
    {
        if(textName.text.Length < 3)
        {
            luz.color = Color.red;
            botonAceptar.SetActive(false);
        }
        if (textName.text.Length >= 3)
        {
            luz.color = Color.green;
            botonAceptar.SetActive(true);
        }
    }

    public void Accept()
    {
        PlayerPrefs.SetString("name1", inputText.text);
        SceneManager.LoadScene("Homes");
    }

}
