using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[System.Serializable]
public class StatementList
{
    public string statement;
    public StatementPlateType value;

    public StatementList(string statement, StatementPlateType value)
    {
        this.statement = statement;
        this.value = value;
    }
}
public class StatementScreen : MonoBehaviour
{
    [Header("Audio")]

    [SerializeField] AudioSource turnOnSound;
    [SerializeField] AudioSource turnOffSound;
    [SerializeField] AudioSource correctSound;
    [SerializeField] AudioSource wrongSound;

    [Header("PasswordScreen")]    
    [SerializeField] AudioSource textSound;
    [SerializeField] Light screenLight;
    [SerializeField] float typeSpeed;
    [SerializeField] List<StatementList> statementLists;
    List<StatementList> remainingStatements;
    List<StatementList> wrongStatements;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] Material materialON;
    [SerializeField] Material materialOFF;
    [SerializeField] Material materialCorrect;
    [SerializeField] Material materialWrong;
    [SerializeField] MeshRenderer meshRenderer;

    const string HTML_ALPHA = "<color=#00000000>";
    public bool isTyping = false;
    const float MAX_TYPE_TIME = 0.1f;
    public static StatementScreen instance;


    public string currentStatement;
    public StatementPlateType currentValue;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        TurnOff(false);
        remainingStatements = statementLists;
        wrongStatements = new List<StatementList>();
        text.text = "";
    }

    string RandomStatement()
    {
        if (remainingStatements.Count == 0)
        {
            if (wrongStatements.Count == 0)
            {
                Debug.Log("No passwords in list");
                return "";
            }
            
            foreach(StatementList statement in wrongStatements)
            {
                remainingStatements.Add(statement);
            }
            wrongStatements.RemoveAll(item => item.statement == currentStatement);
        }

        int randomIndex = Random.Range(0, remainingStatements.Count);

        currentStatement = remainingStatements[randomIndex].statement;
        currentValue = remainingStatements[randomIndex].value;

        remainingStatements.RemoveAt(randomIndex);
        return currentStatement;
    }

    public void NextStatement()
    {
        if (!isTyping)
        {
            string s = RandomStatement();
            StartCoroutine(TypeDialogueText(s));
        }
    }

    IEnumerator TypeDialogueText(string s)
    {
        isTyping = true;

        text.text = "";

        string originalText = s;
        string displayedText = "";
        int alphaIndex = 0;

        foreach (char c in s.ToCharArray())
        {
            if (!textSound.isPlaying && char.IsLetter(c))
                textSound.Play();

            alphaIndex++;
            text.text = originalText;

            displayedText = text.text.Insert(alphaIndex, HTML_ALPHA);
            text.text = displayedText;

            yield return new WaitForSeconds(MAX_TYPE_TIME / typeSpeed);
        }

        textSound.Stop();
        isTyping = false;
    }

    public void TurnOn()
    {
        turnOnSound.Play();
        Color color = materialON.color;
        screenLight.color = color;
        screenLight.gameObject.SetActive(true);


        Material[] newMaterials = meshRenderer.materials;
        newMaterials[2] = materialON;
        meshRenderer.materials = newMaterials;
        NextStatement();
    }

    public void TurnOff(bool sound)
    {
        if (sound)
            turnOffSound.Play();

        screenLight.gameObject.SetActive(false);
        Material[] newMaterials = meshRenderer.materials;
        newMaterials[2] = materialOFF;
        meshRenderer.materials = newMaterials;
        text.text = "";
    }

    public void Correct()
    {
        correctSound.Play();
        Color color = materialCorrect.color;
        screenLight.color = color;

        Material[] newMaterials = meshRenderer.materials;
        newMaterials[2] = materialCorrect;
        meshRenderer.materials = newMaterials;
    }

    public void Wrong()
    {
        wrongSound.Play();
        Color color = materialWrong.color;
        screenLight.color = color;

        Material[] newMaterials = meshRenderer.materials;
        newMaterials[2] = materialWrong;
        meshRenderer.materials = newMaterials;
    }

    public void Default()
    {
        Color color = materialON.color;
        screenLight.color = color;

        Material[] newMaterials = meshRenderer.materials;
        newMaterials[2] = materialON;
        meshRenderer.materials = newMaterials;
    }

    public void AddCurrentStatementToWrongList()
    {
        StatementList item = new StatementList(currentStatement, currentValue);
        wrongStatements.Add(item);
    }
}
