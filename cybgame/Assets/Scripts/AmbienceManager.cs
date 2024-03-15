using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AmbienceManager : MonoBehaviour
{
    [SerializeField] AudioSource mainSong;
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

    // Possible bug?
    public void TurnOffAllAudioSourcesMaybe(AudioSource dontFade, AudioSource dontFade2)
    {
        AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>();

        foreach (AudioSource audioSource in allAudioSources)
        {
            if (audioSource == dontFade || audioSource == dontFade2)
                return;
            
            StartCoroutine(FadeOut(audioSource, fadeOutDuration));
        }
    }

    public static IEnumerator FadeOut(AudioSource audioSource, float duration)
    {
        float startVolume = audioSource.volume;

        if (audioSource.volume == 0f)
        {
            audioSource.Stop();
            yield break;
        }

        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            float normalizedTime = t / duration;

            audioSource.volume = Mathf.Lerp(startVolume, 0f, normalizedTime);

            yield return null;
        }

        audioSource.volume = 0f;
        audioSource.Stop();
    }

    public void FadeThisSourceAway(AudioSource fadeSource)
    {
            StartCoroutine(FadeOutOneAudio(fadeSource, 2f));
    }

    IEnumerator FadeOutOneAudio(AudioSource audioSource, float duration)
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / duration;
            yield return null;
        }

        audioSource.Stop(); 
    }
}
