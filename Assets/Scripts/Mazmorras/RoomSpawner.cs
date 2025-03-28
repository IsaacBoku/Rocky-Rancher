using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    public int openSide;
    [SerializeField]private RoomTemplates roomTemplates;
    private int rand;
    [SerializeField]private bool spawned = false;
    private void Start()
    {
        roomTemplates =  GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        Invoke("Spawn", .1f);
    }
    private void Spawn()
    {
        if(spawned == false)
        {
            if (openSide == 1)
            {
                rand = Random.Range(0, roomTemplates.bottonRooms.Length);
                Instantiate(roomTemplates.bottonRooms[rand], transform.position, roomTemplates.bottonRooms[rand].transform.rotation);
            }
            else if (openSide == 2)
            {
                rand = Random.Range(0, roomTemplates.topRooms.Length);
                Instantiate(roomTemplates.topRooms[rand], transform.position, roomTemplates.topRooms[rand].transform.rotation);
            }
            else if (openSide == 3)
            {
                rand = Random.Range(0, roomTemplates.leftRooms.Length);
                Instantiate(roomTemplates.leftRooms[rand], transform.position, roomTemplates.leftRooms[rand].transform.rotation);
            }
            else if (openSide == 4)
            {
                rand = Random.Range(0, roomTemplates.rightRooms.Length);
                Instantiate(roomTemplates.rightRooms[rand], transform.position, roomTemplates.rightRooms[rand].transform.rotation);
            }
            spawned = true;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SpawnPoint"))
        {
            spawned = true;
        }
    }
}
