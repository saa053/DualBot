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

    bool player1InRange;
    bool player2InRange;

    void Awake()
    {
        visualCue.SetActive(false);
    }

    void Start()
    {
        player1InRange = false;
        player2InRange = false;
        player1Input = GameObject.FindWithTag("Player1").GetComponent<PlayerInputManager>();
        player2Input = GameObject.FindWithTag("Player2").GetComponent<PlayerInputManager>();
    }

    void Update()
    {
        if ((player1InRange || player2InRange) && !DialogueManager.instance.dialogueIsPlaying)
        {
            visualCue.SetActive(true);

            if (player1Input.GetInteract() && player1InRange)
                DialogueManager.instance.EnterDialogueMode(inkJSON);
            else if (player2Input.GetInteract() && player2InRange)
                DialogueManager.instance.EnterDialogueMode(inkJSON);
        }
        else
        {
            visualCue.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player1")
            player1InRange = true;
        if (other.gameObject.tag == "Player2")
            player2InRange = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player1")
            player1InRange = false;
        if (other.gameObject.tag == "Player2")
            player2InRange = false;
    }
}
