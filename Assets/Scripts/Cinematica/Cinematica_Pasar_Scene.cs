using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class Cinematica_Pasar_Scene : MonoBehaviour
{
    public VideoPlayer video;

    private void Awake()
    {
        video = GetComponent<VideoPlayer>();
        video.Play();
        video.loopPointReached += CheckOver;
    }

    void CheckOver(VideoPlayer vp)
    {
        SceneManager.LoadScene("Nombre_Jugador");
        
    }

}
