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
    [SerializeField] float mainIntensity;
    [SerializeField] float subIntensity;
    Room currentLightRoom;

    public static LightManager instance;

    void Awake()
    {
        instance = this;
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

    public IEnumerator TurnOnDirectionalBlink(int blinkTimes, float blinkPause, float lightIntensity)
    {
        SetDirectionalIntesity(lightIntensity, lightIntensity);

        for (int i = 0; i < blinkTimes; i++)
        {
            directionalLights.SetActive(true);

            yield return new WaitForSeconds(blinkPause);

            directionalLights.SetActive(false);

            if (i == blinkTimes/2)
                yield return new WaitForSeconds(blinkPause*4);
            else
                yield return new WaitForSeconds(blinkPause);
        }

        SetDirectionalIntesity(mainIntensity, subIntensity);
        directionalLights.SetActive(true);
    }

    void SetDirectionalIntesity(float main, float sub)
    {
        foreach (Transform light in directionalLights.transform)
        {
            if (light.CompareTag("MainLight"))
                light.GetComponent<Light>().intensity = main;
            else
                light.GetComponent<Light>().intensity = sub;
        }
    }
}
