using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] GameObject lights;
    [SerializeField] GameObject ambience;
    [SerializeField] float fadeTime;
    [SerializeField] bool lightsOnEnter;
    [SerializeField] public bool turnOffDirectional;
    public int width;
    public int height;
    public int x;
    public int y;

    void Start()
    {
        if(RoomController.instance == null)
        {
            //Debug.Log("You pressed play in the wrong scene!");
            return;
        }

        RoomController.instance.RegisterRoom(this);

        foreach (Transform light in lights.transform)
        {
            light.gameObject.SetActive(false);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(width, 0, height));
    }

    public Vector3 GetRoomCenter()
    {
        float paddingX = RoomController.instance.paddingX;
        float paddingY = RoomController.instance.paddingY;
        return new Vector3(x * width * paddingX, 0, y * height * paddingY);
    }

    public (int, int) GetGridPos()
    {
        return (x, y);
    }

    public void TurnOnLights()
    {
        foreach (Transform light in lights.transform)
        {
            if (lightsOnEnter)
                light.gameObject.SetActive(true);
        }
    }

    public IEnumerator TurnOffLights(float speed)
    {
        yield return new WaitForSeconds(speed);

        foreach (Transform light in lights.transform)
        {
            light.gameObject.SetActive(false);
        }
    }

    public void PlayAmbience()
    {
        foreach (Transform audio in ambience.transform)
        {   
            AudioSource source = audio.GetComponent<AudioSource>();
            source.Play();
            StartCoroutine(FadeAmbience(source));
        }
    }

    public void StopAmbience()
    {
        foreach (Transform audio in ambience.transform)
        {   
            AudioSource source = audio.GetComponent<AudioSource>();
            source.Stop();
        }
    }

    IEnumerator FadeAmbience(AudioSource audioSource)
    {
        audioSource.volume = 0f;
        float elapsedTime = 0.0f;
        float startValue = 0.0f;
        float endValue = 1.0f;

        while (elapsedTime < fadeTime)
        {
            float t = elapsedTime / fadeTime;
            float lerpedValue = Mathf.Lerp(startValue, endValue, t);

            audioSource.volume = lerpedValue;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        audioSource.volume = 1f;
    }

    public void OpenDoors()
    {
        DoorController[] doors = GetComponentsInChildren<DoorController>();
        foreach (DoorController door in doors)
        {
            door.OpenDoor();
        }
    }

    public void CloseDoors()
    {
        DoorController[] doors = GetComponentsInChildren<DoorController>();
        foreach (DoorController door in doors)
        {
            door.CloseDoor();
        }
    }
}
