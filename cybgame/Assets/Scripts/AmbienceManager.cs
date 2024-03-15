using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AmbienceManager : MonoBehaviour
{
    [SerializeField] AudioSource mainSong;
    [SerializeField] AudioSource dontFade;
    [SerializeField] float fadeOutDuration;
    Room currentRoom;

    static public AmbienceManager instance;

    void Awake()
    {
        instance = this;
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

    public void StopMainMusic()
    {
        mainSong.Stop();
    }

    public void TurnOffAllAudioSources()
    {
        AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>();

        foreach (AudioSource audioSource in allAudioSources)
        {
            if (audioSource == dontFade)
                return;
            StartCoroutine(FadeOut(audioSource, fadeOutDuration));
        }
    }

    public static IEnumerator FadeOut(AudioSource audioSource, float duration)
    {
        float startVolume = audioSource.volume;

        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            float normalizedTime = t / duration;

            audioSource.volume = Mathf.Lerp(startVolume, 0f, normalizedTime);

            yield return null;
        }

        audioSource.volume = 0f;
        audioSource.Stop();
    }
}
