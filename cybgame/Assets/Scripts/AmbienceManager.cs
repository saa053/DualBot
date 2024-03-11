using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbienceManager : MonoBehaviour
{
    Room currentRoom;

    void Start()
    {

    }

    void Update()
    {
        if (!currentRoom)
            currentRoom = RoomController.instance.currentRoom;

        if (currentRoom != RoomController.instance.currentRoom)
        {
            currentRoom.StopAmbience();
            currentRoom = RoomController.instance.currentRoom;
            currentRoom.PlayAmbience();
        }
    }
}
