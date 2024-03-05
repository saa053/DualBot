using UnityEngine;
using Ink.Runtime;
using System.Collections;
using Unity.VisualScripting;

public class PasswordStrengthManager : MonoBehaviour
{
    [Header("Mini-game settings")]
    [SerializeField] int numPasswords;
    [SerializeField] float displayResultTime;

    [Header("Doors")]
    [SerializeField] DoorController[] doors;

    [Header("NPC")]
    [SerializeField] NPCMove NPC;
    [SerializeField] TextAsset NPCText;
    [SerializeField] TextAsset winText;

    [Header("Plates")]
    [SerializeField] Plates[] plates;
    public bool gameActive = false;
    bool rewardSpawned = false;
    public bool movePlayers = false;
    public bool gameComplete = false;
    public int numAnswered = 0;
    public int numCorrect = 0;
    public int numWrong = 0;
    public bool displayingResult = false;

    Transform player1;
    Transform player2;

    [SerializeField] Room room;

    public static PasswordStrengthManager instance;
    void Awake()
    {
        instance = this;
    }
    void Start()
    {

        player1 = GameObject.FindWithTag("Player1").transform;
        player2 = GameObject.FindWithTag("Player2").transform;
    }

    void Update()
    {
        
        if (movePlayers)
        {
            foreach (Plates plate in plates)
            {
                if (plate.player1OnPlate)
                    player1.position = room.GetRoomCenter() + new Vector3(1, 0, 0);
                if (plate.player2OnPlate)
                    player2.position = room.GetRoomCenter() + new Vector3(-1, 0, 0);
            }

            movePlayers = false;
        }

        if (gameComplete)
        {
            if (NPC.ready)
            {
                DialogueManager.instance.EnterDialogueMode(winText);
                NPC.ready = false;
            }

            if (!rewardSpawned)
                SpawnReward();

            return;
        }

        if (ShouldStart() && !gameActive)
            StartGame();
    }

    void StartGame()
    {
        CloseDoors();

        if (NPC.ready)
        {
            PasswordScreen.instance.TurnOn();
            gameActive = true;
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
            // SPAWN REWARD HERE
            rewardSpawned = true;
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
        gameActive = false;

        PasswordScreen.instance.TurnOff();

        NPC.SetNewTarget(transform.localPosition, 4f);
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
        bool result = false;
        switch (plateType)
        {
            case PlateType.Svakt:
                if (PasswordScreen.instance.currentStrength == PlateType.Svakt)
                    result = true;
                break;
            case PlateType.Middels:
                if (PasswordScreen.instance.currentStrength == PlateType.Middels)
                    result = true;
                break;
            case PlateType.Sterkt:
                if (PasswordScreen.instance.currentStrength == PlateType.Sterkt)
                    result = true;            
                break;
            default:
                result = false;
                break;
        }

        StartCoroutine(DisplayResult(result));
    }

    IEnumerator DisplayResult(bool result)
    {
        displayingResult = true;
        if (result)
        {
            PasswordScreen.instance.Correct();
        }
        else
        {
            PasswordScreen.instance.Wrong();
        }
        
        yield return new WaitForSeconds(displayResultTime);

        PasswordScreen.instance.Default();
        
        movePlayers = true;
        displayingResult = false;

        if (result)
        {
            numCorrect++;
        }
        else
        {
            numWrong++;
            PasswordScreen.instance.AddCurrentPasswordToWrongList();
        }

        numAnswered++;
        if (numCorrect == numPasswords)
            GameComplete();
        else
            PasswordScreen.instance.NextPassword();
        
    }
}
