using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UI;
using Kino;
using TMPro;

public class Outro : MonoBehaviour
{
    [SerializeField] float waitForNSeconds;

    [Header("Audio Sources")]
    [SerializeField] AudioSource glitchSound;
    [SerializeField] AudioSource glitch2Sound;
    [SerializeField] AudioSource dialogueSound;
    [SerializeField] AudioSource fadeThisSourceAway;

    [Header("Glitch")]
    AnalogGlitch analogGlitch;
    DigitalGlitch digitalGlitch;

    [Header("Text settings")]
    [SerializeField] float typeSpeed;
    [SerializeField] float paragraphsWait;
    [SerializeField] GameObject introScreen;
    [SerializeField] TextMeshProUGUI introText;
    [SerializeField] GameObject outroText;

    [SerializeField] string p;
    const string HTML_ALPHA = "<color=#00000000>";
    const float MAX_TYPE_TIME = 0.1f;
    bool isTyping = false;
    bool introShown = false;

    bool playOutro = false;

    public static Outro instance;
    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        p = introText.text;
        introText.text = "";

        Camera camera = Camera.main.transform.GetChild(0).GetComponent<Camera>();
        introScreen.GetComponent<Canvas>().worldCamera = camera;

        analogGlitch = camera.GetComponent<AnalogGlitch>();
        digitalGlitch = camera.GetComponent<DigitalGlitch>();

        analogGlitch.enabled = false;
        digitalGlitch.enabled = false;

        outroText.SetActive(false);
    }

    public void PlayOutro()
    {
        playOutro = true;
        AmbienceManager.instance.FadeThisSourceAway(fadeThisSourceAway);
    }

    void Update()
    {       
        if (!introShown && playOutro)
        {
            introShown = true;
            StartCoroutine(WaitBeforeIntro(waitForNSeconds));
        }
    }

    IEnumerator WaitBeforeIntro(float time)
    {
        yield return new WaitForSeconds(time);
        PlayIntro();
    }


    void PlayIntro()
    {
        digitalGlitch.enabled = true;
        analogGlitch.enabled = true;
        introScreen.SetActive(true);
        GlitchVeryLow();
        StartCoroutine(TypeDialogueText(p));
    }

    void GlitchHigh(bool alternativeSound)
    {
        digitalGlitch.intensity = 0.15f;
        analogGlitch.colorDrift = 0.2f;
        analogGlitch.scanLineJitter = 0.4f;

        if (alternativeSound)
            glitch2Sound.Play();
        else
            glitchSound.Play();
    }

    void GlitchLow()
    {
        analogGlitch.scanLineJitter = 0.18f;
        glitchSound.Stop();
    }

    void GlitchVeryLow()
    {
        analogGlitch.scanLineJitter = 0.11f;
        glitchSound.Stop();
    }

    void GlitchOff(bool playSound)
    {
        digitalGlitch.intensity = 0f;

        analogGlitch.scanLineJitter = 0f;
        analogGlitch.colorDrift = 0f;
    }

    IEnumerator TypeDialogueText(string p)
    {
        outroText.SetActive(true);
        isTyping = true;
        introShown = true;

        introText.text = "";

        string originalText = p;
        string displayedText = "";
        int alphaIndex = 0;

        foreach (char c in p.ToCharArray())
        {
            if (!dialogueSound.isPlaying && char.IsLetter(c))
                dialogueSound.Play();

            alphaIndex++;
            introText.text = originalText;

            displayedText = introText.text.Insert(alphaIndex, HTML_ALPHA);
            introText.text = displayedText;

            yield return new WaitForSeconds(MAX_TYPE_TIME / typeSpeed);

            if (c == '\n')
            {
                dialogueSound.Stop();
                yield return new WaitForSeconds(paragraphsWait);
            }
        }

        isTyping = false;
        dialogueSound.Stop();
    }
}
