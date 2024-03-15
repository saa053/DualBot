using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{

    [SerializeField] public string NPCName;

    [Header("Visual Cure")]
    [SerializeField] GameObject visualCue;

    [Header("Ink JSON")]
    [SerializeField] TextAsset inkJSON;

    Trigger trigger;

    void Awake()
    {
        visualCue.SetActive(false);
    }

    void Start()
    {
        trigger = GetComponent<Trigger>();
    }

    void Update()
    {
        if ((trigger.Player1Close() || trigger.Player2Close()) && !DialogueManager.instance.dialogueIsPlaying)
        {
            visualCue.SetActive(true);

            if (trigger.Player1Trigger())
                DialogueManager.instance.EnterDialogueMode(inkJSON, NPCName);
            else if (trigger.Player2Trigger())
                DialogueManager.instance.EnterDialogueMode(inkJSON, NPCName);
        }
        else
        {
            visualCue.SetActive(false);
        }
    }
}
