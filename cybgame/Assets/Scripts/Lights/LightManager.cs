using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour
{
    [SerializeField] RoomController roomController;
    [SerializeField] GameObject directionalLights;

    [SerializeField] float turnOffDirectionalSpeed;
    [SerializeField] float turnOnDirectionalSpeed;
    [SerializeField] float turnOffRoomSpeed;
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
            StartCoroutine(currentLightRoom.TurnOffLights(turnOffRoomSpeed));
            currentLightRoom = roomController.currentRoom;
            currentLightRoom.TurnOnLights();

            if (currentLightRoom.turnOffDirectional)
                StartCoroutine(TurnOffDirectionals());
            else
                StartCoroutine(TurnOnDirectionals());

        }
    }

    IEnumerator TurnOffDirectionals()
    {
        yield return new WaitForSeconds(turnOffDirectionalSpeed);
        directionalLights.SetActive(false);
    }

    IEnumerator TurnOnDirectionals()
    {
        yield return new WaitForSeconds(turnOnDirectionalSpeed);
        directionalLights.SetActive(true);
    }
}
