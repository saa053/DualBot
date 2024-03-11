using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    bool p1InRange;
    bool p2InRange;

    PlayerInputManager player1Input;
    PlayerInputManager player2Input;
    void Start()
    {
        player1Input = GameObject.FindWithTag("Player1").GetComponent<PlayerInputManager>();
        player2Input = GameObject.FindWithTag("Player2").GetComponent<PlayerInputManager>();
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

    public bool Player1Trigger()
    {
        return p1InRange && player1Input.GetInteract();
    }

    public bool Player2Trigger()
    {
        return p2InRange && player2Input.GetInteract();
    }

    public bool Player1Close()
    {
        return p1InRange;
    }

    public bool Player2Close()
    {
        return p2InRange;
    }
}
