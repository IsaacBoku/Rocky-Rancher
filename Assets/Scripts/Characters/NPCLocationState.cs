using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct NPCLocationState
{

    public CharacterData character;
    public Trasition_Scenes.Locations location;
    public Vector3 coord;
    public Vector3 facing;

    public NPCLocationState(CharacterData character) : this()
    {
        this.character = character;
    }
    public NPCLocationState(CharacterData character,Trasition_Scenes.Locations location,Vector3 coord,Vector3 facing)
    {
        this.character = character;
        this.location = location;
        this.coord = coord;
        this.facing = facing;
    }

}
