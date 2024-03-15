using UnityEngine;
using Ink.Runtime;
using System.Collections;
using Unity.VisualScripting;

public class StatementManager : MonoBehaviour
{
    [Header("Mini-game settings")]
    [SerializeField] int numStatements;
    [SerializeField] float displayResultTime;
    [SerializeField] Vector3 rewardPos;

    [Header("Doors")]
    [SerializeField] DoorController[] doors;
    bool doorsClosed;

    [Header("NPC")]
    [SerializeField] NPCMove NPC;
    [SerializeField] TextAsset NPCText;
    [SerializeField] TextAsset winText;

    [Header("StatementPlates")]
    [SerializeField] StatementPlates[] plates;
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

    public static StatementManager instance;
    void Awake()
    {
        instance = this;
    }
    void Start()
    {

        player1 = GameObject.FindWithTag("Player1").transform;
        player2 = GameObject.FindWithTag("Player2").transform;

        doorsClosed = false;
    }

    void Update()
    {
        
        if (movePlayers)
        {
            foreach (StatementPlates plate in plates)
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
                DialogueManager.instance.EnterDialogueMode(winText, "");
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
        if (!doorsClosed)
            CloseDoors();

        if (NPC.ready)
        {
            StatementScreen.instance.TurnOn();
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
            RewardManager.instance.CreateReward(transform.TransformPoint(rewardPos));
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

        StatementScreen.instance.TurnOff(true);

        NPC.SetNewTarget(transform.position, 4f);
        NPC.trigger = true;
    }

    void CloseDoors()
    {
        foreach (DoorController door in doors)
        {
            door.CloseDoor();
        }

        doorsClosed = true;
    }

    void OpenDoors()
    {
        foreach (DoorController door in doors)
        {
            door.OpenDoor();
        }
    }

    public void SubmitAnswer(StatementPlateType plateType)
    {
        bool result = false;
        switch (plateType)
        {
            case StatementPlateType.Sant:
                if (StatementScreen.instance.currentValue == StatementPlateType.Sant)
                    result = true;
                break;
            case StatementPlateType.Usant:
                if (StatementScreen.instance.currentValue == StatementPlateType.Usant)
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
            StatementScreen.instance.Correct();
        }
        else
        {
            StatementScreen.instance.Wrong();
        }
        
        yield return new WaitForSeconds(displayResultTime);

        StatementScreen.instance.Default();
        
        movePlayers = true;
        displayingResult = false;

        if (result)
        {
            numCorrect++;
        }
        else
        {
            numWrong++;
            StatementScreen.instance.AddCurrentStatementToWrongList();
        }

        numAnswered++;
        if (numCorrect == numStatements)
            GameComplete();
        else
            StatementScreen.instance.NextStatement();
        
    }
}
