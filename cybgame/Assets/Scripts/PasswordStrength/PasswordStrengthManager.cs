using UnityEngine;
using Ink.Runtime;

public class PasswordStrengthManager : MonoBehaviour
{
    [Header("Mini-game settings")]
    [SerializeField] int numPasswords;

    [Header("Doors")]

    [SerializeField] DoorController[] doors;

    [SerializeField] NPCMove NPC;
    [SerializeField] TextAsset NPCText;
    [SerializeField] TextAsset winText;

    bool screenOn = false;
    bool gameComplete = false;

    int numAnswered = 0;

    public static PasswordStrengthManager instance;
    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if (gameComplete)
        {
            if (NPC.ready)
            {
                DialogueManager.instance.EnterDialogueMode(winText);
                NPC.ready = false;
            }

            SpawnReward();
            return;
        }

        if (screenOn)
            return;

        if (!ShouldStart())
            return;

        CloseDoors();
        
        if (NPC.ready)
        {
            PasswordScreen.instance.TurnOn();
            screenOn = true;
        }
        else
        {
            NPC.trigger = true;
        }
    }

    void SpawnReward()
    {
        if (DialogueSaveManager.instance.GetBool(winText, "reward"))
        {
            Debug.Log("Reward spawned?");
            OpenDoors();
        }
    }

    bool ShouldStart()
    {
        bool result = DialogueSaveManager.instance.GetBool(NPCText, "GO");
        return result;
    }

    void GameComplete()
    {
        gameComplete = true;

        PasswordScreen.instance.TurnOff();
        screenOn = false;

        NPC.SetNewTarget(transform.position, 4f);
        NPC.trigger = true;
    }

    void CloseDoors()
    {
        foreach (DoorController door in doors)
        {
            door.locked = true;
        }
    }

    void OpenDoors()
    {
        foreach (DoorController door in doors)
        {
            door.locked = false;
        }
    }

    public void SubmitAnswer(PlateType plateType)
    {
        switch (plateType)
        {
            case PlateType.Svakt:
                break;
            case PlateType.Middels:
                break;
            case PlateType.Sterkt:
                break;
            default:
                break;
        }

        numAnswered++;
        if (numAnswered == numPasswords)
            GameComplete();
        else
            PasswordScreen.instance.NextPassword();

    }
}
