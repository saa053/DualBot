using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portable : MonoBehaviour
{
    [SerializeField] float distanceFromPlayer;
    [SerializeField] float height;
    
    Rigidbody body;
    Trigger trigger;
    BoxCollider boxCollider;

    [Header("Players")]
    bool player1IsCarry;
    bool player2IsCarry;
    Transform player1;
    Transform player2;
    PlayerInputManager player1Input;
    PlayerInputManager player2Input;
    Animator player1Animator;
    Animator player2Animator;
    
    void Start()
    {
        body = GetComponent<Rigidbody>();  
        trigger = transform.Find("Trigger").GetComponent<Trigger>();
        boxCollider = GetComponent<BoxCollider>();

        player1 = GameObject.FindWithTag("Player1").transform;
        player2 = GameObject.FindWithTag("Player2").transform;
        player1Input = GameObject.FindWithTag("Player1").GetComponent<PlayerInputManager>();
        player2Input = GameObject.FindWithTag("Player2").GetComponent<PlayerInputManager>();
        player1Animator = player1.GetComponentInChildren<Animator>();
        player2Animator = player2.GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player1IsCarry && player1Input.GetInteract() && transform.parent == player1)
        {
            player1IsCarry = false;
            Drop();
            player1Animator.SetBool("isCarry", false);
        }
        
        if (player2IsCarry && player2Input.GetInteract() && transform.parent == player2)
        {
            player2IsCarry = false;
            Drop();
            player2Animator.SetBool("isCarry", false);
        }
            
        if (trigger.Player1Trigger() && !player1IsCarry)
        {
            player1IsCarry = true;
            PickUp(player1);
            player1Animator.SetBool("isCarry", true);
        }
        
        if (trigger.Player2Trigger() && !player2IsCarry)
        {
            player2IsCarry = true;
            PickUp(player2);
            player2Animator.SetBool("isCarry", true);
        }
    }

    void PickUp(Transform parent)
    {
        transform.rotation = parent.rotation;
        transform.position = parent.transform.position;
        
        transform.position += parent.forward * distanceFromPlayer;
        transform.position += new Vector3(0, height, 0);

        body.useGravity = false;
        body.isKinematic = true;
        boxCollider.isTrigger = true;

        transform.parent = parent;
    }

    void Drop()
    {
        body.useGravity = true;
        body.isKinematic = false;
        boxCollider.isTrigger = false;

        transform.parent = RoomController.instance.currentRoom.transform;
    }
}
