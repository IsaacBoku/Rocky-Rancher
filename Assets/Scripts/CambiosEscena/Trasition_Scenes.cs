using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class Trasition_Scenes : MonoBehaviour
{
    public static Trasition_Scenes instance;
    public enum Locations
    {
        Farm, Town, Dungeons, Homes, Cinematica, Inicio_Menu,Taberna,Fase_Juego
    }
    public Locations currentLocation;
    public Transform playerPoint;
    
    bool screenFadeOut;
    public UnityEvent onLocationLoad;

    static readonly Locations[] indoor = { Locations.Homes ,Locations.Taberna };
    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(gameObject);
        else
            instance = this;

        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnLocationLoad;
        playerPoint = FindObjectOfType<Player_Move>().transform;
    }
    public bool CurrentlyIndoor()
    {
        return indoor.Contains(currentLocation);
    }
    public void SwitchLocation(Locations locationToSwitch)
    {
        UIManager.Instance.FadeOutScreen();
        screenFadeOut = false;
        StartCoroutine(ChangeScene(locationToSwitch));
    }
    private void OnFadeOutComplete()
    {
        screenFadeOut = true;
    }
    IEnumerator ChangeScene(Locations locationToSwitch)
    {
       /*while(!screenFadeOut)
        {
           
            yield return new WaitForSeconds(1);
        }*/
        screenFadeOut = false;
        UIManager.Instance.ResetDefault();
        SceneManager.LoadScene(locationToSwitch.ToString());
        
        yield return null;
           
    }
    public void OnLocationLoad(Scene scene, LoadSceneMode mode)
    {
        Locations oldLocation = currentLocation;

        Locations newLocation = (Locations)Enum.Parse(typeof(Locations), scene.name);

        if (currentLocation == newLocation)
            return;

        Transform starPoint = LocationManager.Instance.GetPlayerStartingPosition(oldLocation);
        if (playerPoint == null) return;
        CharacterController playerCharacter = playerPoint.GetComponent<CharacterController>();
        playerCharacter.enabled = false;

        playerPoint.position = starPoint.position;
        playerPoint.rotation = starPoint.rotation;
        playerCharacter.enabled = true;

        currentLocation = newLocation;
        onLocationLoad.Invoke();
    }
}
