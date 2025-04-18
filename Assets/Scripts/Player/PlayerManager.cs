using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    public Player_Move player;
    private void Awake()
    {
        if (instance != null)
            Destroy(this);
        else
            instance = this;
    }
}
