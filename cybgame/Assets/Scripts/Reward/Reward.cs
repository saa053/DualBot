using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Reward : MonoBehaviour
{
    [SerializeField] GameObject fx;
    AudioSource pickUpSound;

    Trigger trigger;

    void Start()
    {
        pickUpSound = GameObject.Find("RewardPickUp").GetComponent<AudioSource>();

        trigger = GetComponentInChildren<Trigger>();
    }

    void Update()
    {
        if (trigger.Player1Close())
            CheckForPickUp(trigger.Player1Trigger());
        if (trigger.Player2Close())
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
