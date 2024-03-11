using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[System.Serializable]
public class PasswordList
{
    public string password;
    public PlateType strength;

    public PasswordList(string password, PlateType strength)
    {
        this.password = password;
        this.strength = strength;
    }
}
public class PasswordScreen : MonoBehaviour
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
    [SerializeField] List<PasswordList> passwordLists;
    List<PasswordList> remainingPasswords;
    List<PasswordList> wrongPasswords;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] Material materialON;
    [SerializeField] Material materialOFF;
    [SerializeField] Material materialCorrect;
    [SerializeField] Material materialWrong;
    [SerializeField] MeshRenderer meshRenderer;

    const string HTML_ALPHA = "<color=#00000000>";
    public bool isTyping = false;
    const float MAX_TYPE_TIME = 0.1f;
    public static PasswordScreen instance;


    public string currentPassword;
    public PlateType currentStrength;



    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        TurnOff();
        remainingPasswords = passwordLists;
        wrongPasswords = new List<PasswordList>();
        text.text = "";
    }

    string RandomPassword()
    {
        if (remainingPasswords.Count == 0)
        {
            if (wrongPasswords.Count == 0)
            {
                Debug.Log("No passwords in list");
                return "";
            }
            
            foreach(PasswordList password in wrongPasswords)
            {
                remainingPasswords.Add(password);
            }
            wrongPasswords.RemoveAll(item => item.password == currentPassword);
        }

        int randomIndex = Random.Range(0, remainingPasswords.Count);

        currentPassword = remainingPasswords[randomIndex].password;
        currentStrength = remainingPasswords[randomIndex].strength;

        remainingPasswords.RemoveAt(randomIndex);
        return currentPassword;
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
        NextPassword();
    }

    public void TurnOff()
    {
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

    public void AddCurrentPasswordToWrongList()
    {
        PasswordList item = new PasswordList(currentPassword, currentStrength);
        wrongPasswords.Add(item);
    }
}
