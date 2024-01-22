using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{

    [Header("Visual Cure")]
    [SerializeField] GameObject visualCue;

    [Header("Ink JSON")]
    [SerializeField] TextAsset inkJSON;

    PlayerInputManager player1Input;
    PlayerInputManager player2Input;

    bool playerInRange;
    void Awake()
    {
        playerInRange = false;
        visualCue.SetActive(false);
    }

    void Start()
    {
        player1Input = GameObject.FindWithTag("Player1").GetComponent<PlayerInputManager>();
        player2Input = GameObject.FindWithTag("Player2").GetComponent<PlayerInputManager>();
    }

    void Update()
    {
        if (playerInRange && !DialogueManager.instance.dialogueIsPlaying)
        {
            visualCue.SetActive(true);

            if (player1Input.GetInteract() || player2Input.GetInteract())
            {
                DialogueManager.instance.EnterDialogueMode(inkJSON);
            }
        }
        else
        {
            visualCue.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player1" || other.gameObject.tag == "Player2")
        {
            playerInRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player1" || other.gameObject.tag == "Player2")
        {
            playerInRange = false;
        }
    }
}
