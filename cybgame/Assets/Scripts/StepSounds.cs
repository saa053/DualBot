using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepSounds : MonoBehaviour
{
    [SerializeField] float pitchLowest;
    [SerializeField] float pitchHighest;
    AudioSource audioSource;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void Step()
    {
        audioSource.pitch = Random.Range(pitchLowest, pitchHighest);
        audioSource.Play();
    }
}
