using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Ink.Runtime;
using System;
using UnityEngine.InputSystem;

public class DialogueManager : MonoBehaviour
{
    [Header("Dialogue UI")]
    [SerializeField] GameObject dialoguePanel;
    [SerializeField] TextMeshProUGUI dialogueText;

    Story currentStory;
    public bool dialogueIsPlaying { get; private set; }
    public static DialogueManager instance;

    PlayerInputManager player1Input;
    PlayerInputManager player2Input;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        player1Input = GameObject.FindWithTag("Player1").GetComponent<PlayerInputManager>();
        player2Input = GameObject.FindWithTag("Player2").GetComponent<PlayerInputManager>();
    }

    void Update()
    {
        if (!dialogueIsPlaying)
            return;

        if (player1Input.GetInteract() || player2Input.GetInteract())
        {
            ContinueStory();
        }
    }

    public void EnterDialogueMode(TextAsset inkJSON)
    {
        currentStory = new Story(inkJSON.text);
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);

        ContinueStory();
    }

    void ExitDialogueMode()
    {
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
    }

    void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            dialogueText.text = currentStory.Continue();
        }
        else
        {
            ExitDialogueMode();
        }
    }
}
