using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Logica_Options : MonoBehaviour
{
    [Header("Resolution")]
    [SerializeField] private TMP_Dropdown resolucionDropDown;
    Resolution[] resolucion;
    [SerializeField] Toggle _fullScreen;

    [Header("Volumen")]
    [SerializeField] private Slider _VolumenSlider;
    [SerializeField] private float _Volumen;
    [SerializeField] private TextMeshProUGUI _valorVolumen;

    [Header("Brillo")]
    [SerializeField] private Slider _BrilloSlider;
    [SerializeField] private float _Brillo;
    [SerializeField] private Image _PanelBrillo;
    void Start()
    {
        if(Screen.fullScreen)
        {
            _fullScreen.isOn = true;
        }
        else
        {
            _fullScreen.isOn = false;
        }
        RevisarResolucion();
        VolumenCotrol();
        BrilloControl();
    }
    #region Volumen
    void VolumenCotrol()
    {
        _VolumenSlider.value = PlayerPrefs.GetFloat("volumenAudio", 0.5f);
        AudioListener.volume = _VolumenSlider.value;
    }
    public void ChangeSliderVolumen(float _value)
    {
        _Volumen = _value;
        PlayerPrefs.SetFloat("volumenAudio", _Volumen);
        AudioListener.volume = _VolumenSlider.value;
    }
    #endregion
    #region Brillo
    void BrilloControl()
    {
        _BrilloSlider.value = PlayerPrefs.GetFloat("brillo", 0.5f);
        _PanelBrillo.color = new Color(_PanelBrillo.color.r, _PanelBrillo.color.g,_PanelBrillo.color.b, _BrilloSlider.value);
    }
    public void ChangeSliderBrillo(float _value)
    {
        _Brillo = _value;
        PlayerPrefs.SetFloat("brillo", _Brillo);
        _PanelBrillo.color = new Color(_PanelBrillo.color.r, _PanelBrillo.color.g, _PanelBrillo.color.b, _BrilloSlider.value);
    }
    #endregion

    public void ActivarPantallaCompleta(bool pantallaCompleta)
    {
        Screen.fullScreen = pantallaCompleta;
    }


    public void RevisarResolucion()
    {
        resolucion = Screen.resolutions;
        resolucionDropDown.ClearOptions();
        List<string> opciones = new List<string>();
        int resolucionActual = 0;
        for (int i = 0; i < resolucion.Length; i++)
        {
            string opcion = resolucion[i].width + "x" + resolucion[i].height;
            opciones.Add(opcion);
            if (Screen.fullScreen && resolucion[i].width == Screen.currentResolution.width && resolucion[i].height == Screen.currentResolution.height)
            {
                resolucionActual = i;
            }
        }
        resolucionDropDown.AddOptions(opciones);
        resolucionDropDown.value = resolucionActual;
        resolucionDropDown.RefreshShownValue();
        resolucionDropDown.value = PlayerPrefs.GetInt("numeroResolucion", 0);
    }
    public void CambiarResolucion(int indiceResolucion)
    {
        PlayerPrefs.SetInt("numeroResolucion", resolucionDropDown.value);
        Resolution resoluciones = resolucion[indiceResolucion];
        Screen.SetResolution(resoluciones.width,resoluciones.height,Screen.fullScreen);
    }


}
