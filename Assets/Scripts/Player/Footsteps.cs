using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footsteps : MonoBehaviour
{
    public CheckIfGround checkIfground;
    public CheckTerrainTexture checkTerrainTexture;

    public AudioSource audioSource;

    public AudioClip[] stoneClip;
    public AudioClip[] dirtClips;
    public AudioClip[] woodClips;
    AudioClip previousClip;

    CharacterController character;
    float currentSpeed;
    bool walking;
    float distanceCovered;
    public float modifier = 0.5f;

    float airTime;

    private void Start()
    {
        character = gameObject.GetComponent<CharacterController>();
    }
    private void Update()
    {
        currentSpeed = GetPlayerSpeed();
        walking = CheckIfWalking();
        if(walking )
        {
            distanceCovered += (currentSpeed * Time.deltaTime) * modifier;
            if(distanceCovered > 1)
            {
                TriggerNextClip();
                distanceCovered = 0;
            }
        }
    }

    float GetPlayerSpeed()
    {
        float speed = character.velocity.magnitude;
        return speed;
    }
    bool CheckIfWalking()
    {
        if(currentSpeed > 0 && checkIfground.isGround)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    AudioClip GetClipFromArray(AudioClip[] clipArray)
    {
        int attempts = 3;
        AudioClip selectedClip = clipArray[Random.Range(0,clipArray.Length - 1)];

        while(selectedClip == previousClip && attempts > 0)
        {
            selectedClip = clipArray[Random.Range(0,clipArray.Length - 1)];
            attempts--;
        }
        previousClip = selectedClip;
        return selectedClip;
    }

    void TriggerNextClip()
    {
        audioSource.pitch = Random.Range(0.9f, 1.1f);
        audioSource.volume = Random.Range(0.8f, 1);

        if (checkIfground.isOnTerrain)
        {
            checkTerrainTexture.GetterrainTexture();
            if (checkTerrainTexture.textureValues[0] > 0)
            {
                audioSource.PlayOneShot(GetClipFromArray(stoneClip),checkTerrainTexture.textureValues[0]);
            }
            if (checkTerrainTexture.textureValues[1] > 0)
            {
                audioSource.PlayOneShot(GetClipFromArray(dirtClips), checkTerrainTexture.textureValues[1]);
            }
            if (checkTerrainTexture.textureValues[2] > 0)
            {
                audioSource.PlayOneShot(GetClipFromArray(dirtClips), checkTerrainTexture.textureValues[2]);
            }
            if (checkTerrainTexture.textureValues[3] > 0)
            {
                audioSource.PlayOneShot(GetClipFromArray(stoneClip), checkTerrainTexture.textureValues[3]);
            }
        }
    }

}
