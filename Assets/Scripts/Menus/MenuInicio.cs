using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuInicio : MonoBehaviour
{
    public Button loadGameButton;
    public void NewGame()
    {
        StartCoroutine(LoadGameAsync(Trasition_Scenes.Locations.Fase_Juego,null));
        
    }
    public void ContinueGame()
    {
        StartCoroutine(LoadGameAsync(Trasition_Scenes.Locations.Homes, LoadGame));
    }

    void LoadGame()
    {
        if(GameStateManager.Instance == null)
        {
            Debug.LogError("Cannot find Game State Manager!");
            return;
        }
        //GameStateManager.Instance.LoadSave();
    }
    public void OnApplicationQuit()
    {
        Application.Quit();
    }

    IEnumerator LoadGameAsync(Trasition_Scenes.Locations scene,Action onFirstFrameLoad)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene.ToString());
        //DontDestroyOnLoad(gameObject);
        while(!asyncLoad.isDone)
        {
            yield return null;
            Debug.Log("Loading");
        }
        Debug.Log("Loading!");

        yield return new WaitForEndOfFrame();
        Debug.Log("First frame is loaded");
        onFirstFrameLoad?.Invoke();

        //Destroy(gameObject);
    }
    private void Start()
    {
        loadGameButton.interactable = SaveManager.HasSave();
    }

    public void VolverInicio()
    {
        SceneManager.LoadScene("Inicio_Menu");
    }
}
