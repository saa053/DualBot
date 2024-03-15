using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Reward : MonoBehaviour
{
    [SerializeField] GameObject fx;
    [SerializeField] GameObject visualCue;
    AudioSource pickUpSound;

    Trigger trigger;

    void Start()
    {
        pickUpSound = GameObject.Find("RewardPickUp").GetComponent<AudioSource>();

        trigger = GetComponentInChildren<Trigger>();
    }

    void Update()
    {
        CheckForPickUp(trigger.Player1Trigger());
        CheckForPickUp(trigger.Player2Trigger());
    }

    void PlayFx()
    {
        GameObject particleObject = Instantiate(fx, transform.position, Quaternion.identity);
        ParticleSystem particleSystem = particleObject.GetComponent<ParticleSystem>();
        particleSystem.Play();
    }

    void CheckForPickUp(bool value)
    {
        if (value)
        {
            RewardManager.instance.IncrementRewardCount();
            
            pickUpSound.Play();
            PlayFx();

            Destroy(this.gameObject);
        }
    }
}
