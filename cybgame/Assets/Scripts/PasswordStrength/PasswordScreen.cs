using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;

public class PasswordScreen : MonoBehaviour
{

    [SerializeField] float typeSpeed;
    [SerializeField] string[] passwordList;
    List<string> remainingPasswords;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] Material materialON;
    [SerializeField] Material materialOFF;
    [SerializeField] MeshRenderer meshRenderer;

    const string HTML_ALPHA = "<color=#00000000>";
    public bool isTyping = false;
    const float MAX_TYPE_TIME = 0.1f;

    public static PasswordScreen instance;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        TurnOff();
        remainingPasswords = passwordList.ToList();
        text.text = "";
    }

    void Update()
    {
        
    }

    string RandomPassword()
    {
        if (remainingPasswords.Count == 0)
        {
            Debug.Log("No passwords in list");
            return "";
        }

        int randomIndex = Random.Range(0, remainingPasswords.Count);
        string password = remainingPasswords[randomIndex];
        remainingPasswords.RemoveAt(randomIndex);
        return password;
    }

    public void NextPassword()
    {
        if (!isTyping)
        {
            string p = RandomPassword();
            StartCoroutine(TypeDialogueText(p));
        }
    }

    IEnumerator TypeDialogueText(string p)
    {
        isTyping = true;

        text.text = "";

        string originalText = p;
        string displayedText = "";
        int alphaIndex = 0;

        foreach (char c in p.ToCharArray())
        {
            alphaIndex++;
            text.text = originalText;

            displayedText = text.text.Insert(alphaIndex, HTML_ALPHA);
            text.text = displayedText;

            yield return new WaitForSeconds(MAX_TYPE_TIME / typeSpeed);
        }

        isTyping = false;
    }

    public void TurnOn()
    {
        meshRenderer.materials[2] = materialON;
        NextPassword();
    }

    public void TurnOff()
    {
        meshRenderer.materials[2] = materialOFF;
    }
}
