using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour
{
    [SerializeField] RoomController roomController;
    Room currentLightRoom;

    void Start()
    {

    }

    void Update()
    {
        if (!currentLightRoom)
            currentLightRoom = roomController.currentRoom;

        if (currentLightRoom != roomController.currentRoom)
        {
            currentLightRoom.TurnOffLights();
            currentLightRoom = roomController.currentRoom;
            currentLightRoom.TurnOnLights();
        }
    }
}
