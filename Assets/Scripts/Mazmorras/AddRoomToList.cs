using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddRoomToList : MonoBehaviour
{
    private RoomTemplates roomTemplates;
    void Start()
    {
        roomTemplates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();

        roomTemplates.rooms.Add(this.gameObject);
    }
}
