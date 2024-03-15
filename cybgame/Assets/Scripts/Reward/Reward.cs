using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Reward : MonoBehaviour
{
    [SerializeField] GameObject fx;
    [SerializeField] GameObject visualCue;
    AudioSource pickUpSound;
    bool p1InRange;
    bool p2InRange;

    PlayerInputManager player1Input;
    PlayerInputManager player2Input;

    void Start()
    {
        p1InRange = false;
        p2InRange = false;

        player1Input = GameObject.FindWithTag("Player1").GetComponent<PlayerInputManager>();
        player2Input = GameObject.FindWithTag("Player2").GetComponent<PlayerInputManager>();

        pickUpSound = GameObject.Find("RewardPickUp").GetComponent<AudioSource>();
    }

    void Update()
    {
        CheckForPickUp(p1InRange, player1Input.GetInteract());
        CheckForPickUp(p2InRange, player2Input.GetInteract());
    }

    void PlayFx()
    {
        GameObject particleObject = Instantiate(fx, transform.position, Quaternion.identity);
        ParticleSystem particleSystem = particleObject.GetComponent<ParticleSystem>();
        particleSystem.Play();
    }

    void CheckForPickUp(bool inRange, bool interact)
    {
        if (inRange && interact)
        {
            RewardManager.instance.IncrementRewardCount();
            
            pickUpSound.Play();
            PlayFx();

            Destroy(this.gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player1")
            p1InRange = true;
        if (other.gameObject.tag == "Player2")
            p2InRange = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player1")
            p1InRange = false;
        if (other.gameObject.tag == "Player2")
            p2InRange = false;
    }
}
