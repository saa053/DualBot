using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Ink.Runtime;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Unity.VisualScripting;
using Unity.Mathematics;
using System.Linq;

public class DialogueManager : MonoBehaviour
{
    [Header("Choice settings")]
    [SerializeField] float waitTimeWhenChoosingSameAnswer;

    [Header("Dialogue UI")]
    [SerializeField] GameObject dialoguePanel;
    [SerializeField] TextMeshProUGUI dialogueText;
    [SerializeField] GameObject NPCName;
    [SerializeField] GameObject NPCImage;
    [SerializeField] GameObject transparentBackground;
    [SerializeField] GameObject countdown;
    [SerializeField] GameObject checkmarks;
    TextMeshProUGUI countdownsText;

    [Header("Choices UI")]
    [SerializeField] GameObject choicePanel;
    [SerializeField] GameObject[] choices;
    [SerializeField] Color p2Color;
    [SerializeField] Color p1Color;

    [SerializeField] Color defaultColor;
    [SerializeField] Color agreeColor;
    TextMeshProUGUI[] choicesText;

    int p1SelectedChoice;
    int p2SelectedChoice;
    int p1CurrentChoice;
    int p2CurrentChoice;

    int numChoices;

    bool p1AcceptInput = true;
    bool p2AcceptInput = true;

    bool displayingChoices;
    Story currentStory;
    public bool dialogueIsPlaying { get; private set; }

    bool waitingToSubmit;
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
        displayingChoices = false;
        waitingToSubmit = false;

        dialoguePanel.SetActive(false);
        choicePanel.SetActive(false);

        transparentBackground.SetActive(false);
        NPCName.SetActive(false);
        NPCImage.SetActive(false);
        countdown.SetActive(false);

        checkmarks.SetActive(false);
        checkmarks.transform.GetChild(0).gameObject.SetActive(false);
        checkmarks.transform.GetChild(1).gameObject.SetActive(false);

        player1Input = GameObject.FindWithTag("Player1").GetComponent<PlayerInputManager>();
        player2Input = GameObject.FindWithTag("Player2").GetComponent<PlayerInputManager>();

        choicesText = new TextMeshProUGUI[choices.Length];
        countdownsText = countdown.GetComponentInChildren<TextMeshProUGUI>();
        int index = 0;
        foreach (GameObject choice in choices)
        {
            // Set visibility of player choice icons
            choice.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
            choice.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);

            choicesText[index] = choice.transform.GetComponentInChildren<TextMeshProUGUI>();
            index++;
        }

        p1CurrentChoice = 0;
        p2CurrentChoice = 0;
        p1SelectedChoice = -1;
        p2SelectedChoice = -1;

        movePlayerIcon();
    }

    void Update()
    {
        if (!dialogueIsPlaying)
            return;

        if (displayingChoices)
        {
            MoveChoice(player1Input, ref p1CurrentChoice, ref p1AcceptInput);
            MoveChoice(player2Input, ref p2CurrentChoice, ref p2AcceptInput);

            SelectChoice(player1Input, p1CurrentChoice, ref p1SelectedChoice, p1Color);
            SelectChoice(player2Input, p2CurrentChoice, ref p2SelectedChoice, p2Color);

            if (p1SelectedChoice == p2SelectedChoice && p1SelectedChoice != -1 && !waitingToSubmit)
            {
                StartCoroutine(WaitToSubmit(waitTimeWhenChoosingSameAnswer));
            }
        }
        else
        {
            if (player1Input.GetInteract() || player2Input.GetInteract())
            {
                ContinueStory();
            }
        }
    }

    void CancelWaitToSubmit(int p1PreCancelChoice, int p2PreCancelChoice)
    {
        StopCoroutine("WaitToSubmit");
        waitingToSubmit = false;
        countdown.SetActive(false);

        if (p1PreCancelChoice != p1SelectedChoice)
        {
            ChangeColorOfButton(choices[p2SelectedChoice].GetComponent<Button>(), p2Color);
        }
        else if (p2PreCancelChoice != p2SelectedChoice)
        {
            ChangeColorOfButton(choices[p1SelectedChoice].GetComponent<Button>(), p1Color);
        }
    }

    IEnumerator WaitToSubmit(float waitTime)
    {
        waitingToSubmit = true;
        ChangeColorOfButton(choices[p1SelectedChoice].GetComponent<Button>(), agreeColor);

        float remainingTime = waitTime;

        int p1PreCancelChoice = p1CurrentChoice;
        int p2PreCancelChoice = p2CurrentChoice;

        while (remainingTime > 0)
        {
            countdown.SetActive(true);
            countdownsText.text = "" + Mathf.CeilToInt(remainingTime).ToString();
            yield return new WaitForSeconds(0.1f);

            if (p1SelectedChoice != p2SelectedChoice)
            {
                CancelWaitToSubmit(p1PreCancelChoice, p2PreCancelChoice);
                yield break;
            }


            remainingTime -= 0.1f;
        }
        
        if (p1SelectedChoice == p2SelectedChoice && p1SelectedChoice != -1)
        {
            currentStory.ChooseChoiceIndex(p1SelectedChoice);
            ContinueStory();
            displayingChoices = false;
            waitingToSubmit = false;
            countdown.SetActive(false);
        }
        else
        {
            CancelWaitToSubmit(p1PreCancelChoice, p2PreCancelChoice);
        }

    }

    void movePlayerIcon()
    {
        foreach (GameObject choice in choices)
        {
            choice.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
            choice.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
        }

        choices[p1CurrentChoice].transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
        choices[p2CurrentChoice].transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
    }

    void MoveChoice(PlayerInputManager inputManager, ref int currentChoice, ref bool acceptInput)
    {
        if (!inputManager.GetMove())
            return;
    
        float input = inputManager.GetMoveInput().z;

        float deadZone = 0.75f;
        if (Mathf.Abs(input) < deadZone)
        {
            acceptInput = true;
            return;
        }
        else if(input == 1 || input == -1)
            acceptInput = true;
        

        if (input < 0 && acceptInput)
        {
            currentChoice = (currentChoice + 1) % (numChoices + 1);
            acceptInput = false;
        }
        else if (input > 0 && acceptInput)
        {
            currentChoice = (currentChoice - 1 + numChoices + 1) % (numChoices + 1);
            acceptInput = false;
        }

        movePlayerIcon();
    }

    void SelectChoice(PlayerInputManager inputManager, int currentChoice, ref int selectedChoice, Color selectColor)
    {
        if (!inputManager.GetInteract())
            return;

        if (selectedChoice == currentChoice)
            return;

        if (selectedChoice != -1)
        {
            ChangeColorOfButton(choices[selectedChoice].GetComponent<Button>(), defaultColor);
        }

        selectedChoice = currentChoice;

        ChangeColorOfButton(choices[selectedChoice].GetComponent<Button>(), selectColor);
    }

    void ChangeColorOfButton(Button button, Color color)
    {
        ColorBlock colorBlock = button.colors;
        colorBlock.normalColor = color;
        button.colors = colorBlock;
    }

    public void EnterDialogueMode(TextAsset inkJSON)
    {
        currentStory = new Story(inkJSON.text);

        if (currentStory.variablesState == null || !currentStory.variablesState.Contains("saveString"))
        {
            Debug.Log("Ink story doesn't have a save string!");
            return;
        }

        bool shouldSave = (bool)currentStory.variablesState["shouldSave"];
        string saveString = (string)currentStory.variablesState["saveString"];

        if (shouldSave)
        {
            Story storyState = DialogueSaveManager.instance.LoadStoryState(currentStory, saveString);
            if (storyState != null)
            {
                Debug.Log("Loading story...");
                currentStory = storyState;
                currentStory.ChoosePathString("resetLabel");
            }
        }

        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);

        NPCName.SetActive(true);
        NPCImage.SetActive(true);
        transparentBackground.SetActive(true);

        ContinueStory();
    }

    void ExitDialogueMode()
    {
        bool shouldSave = (bool)currentStory.variablesState["shouldSave"];
        string saveString = (string)currentStory.variablesState["saveString"];

        if (shouldSave)
        {
            bool result = DialogueSaveManager.instance.SaveStoryState(currentStory, saveString);
            if (!result)
            {
                Debug.Log("Failed saving story state!");
                return;
            }
        }

        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";

        NPCName.SetActive(false);
        NPCImage.SetActive(false);
        transparentBackground.SetActive(false);
    }

    void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            dialogueText.text = currentStory.Continue();
            DisplayChoices();
        }
        else
        {
            ExitDialogueMode();
        }
    }

    void DisplayChoices()
    {
        List<Choice> currentChoices = currentStory.currentChoices;

        p1CurrentChoice = 0;
        p2CurrentChoice = 0;
        p1SelectedChoice = -1;
        p2SelectedChoice = -1;

        numChoices = currentChoices.Count - 1;

        if (currentChoices.Count > choices.Length)
            Debug.LogError("More choices were given than the UI can support. Number of choices given: " + currentChoices.Count);
        
        int index = 0;
        foreach(Choice choice in currentChoices)
        {
            choices[index].gameObject.SetActive(true);
            choicesText[index].text = choice.text;

            ChangeColorOfButton(choices[index].GetComponent<Button>(), defaultColor);
            index++;
        }

        for (int i = index; i < choices.Length; i++)
        {
            choices[i].gameObject.SetActive(false);
        }

        if (currentChoices.Count != 0)
        {
            choicePanel.SetActive(true);
            displayingChoices = true;
        }
        else
            choicePanel.SetActive(false);
    }
}
