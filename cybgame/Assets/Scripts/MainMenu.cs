using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UI;
using Kino;
using TMPro;

public class MainMenu : MonoBehaviour
{
    [SerializeField] string sceneToPlay;

    [Header("Audio Sources")]
    [SerializeField] AudioSource glitchSound;
    [SerializeField] AudioSource glitch2Sound;
    [SerializeField] AudioSource blackscreenSound;
    [SerializeField] AudioSource dialogueSound;
    private bool isLoading = false;

    AnalogGlitch analogGlitch;
    DigitalGlitch digitalGlitch;


    [Header("Controls")]
    [SerializeField] GameObject controlsImage;
    bool controlsShown = false;
    [SerializeField] PlayerInputManager player1;
    [SerializeField] PlayerInputManager player2;

    [SerializeField] GameObject checkmark1;
    [SerializeField] GameObject checkmark2;
    [SerializeField] GameObject fade;

    [SerializeField] float fadeInSpeed;
    [SerializeField] float decrementAlpha;

    bool player1Ready;
    bool player2Ready;


    [Header("Text settings")]
    [SerializeField] float typeSpeed;
    [SerializeField] float paragraphsWait;
    [SerializeField] GameObject startText;
    [SerializeField] GameObject introScreen;
    [SerializeField] TextMeshProUGUI introText;
    [SerializeField] GameObject continueText;

    [SerializeField] string p;
    const string HTML_ALPHA = "<color=#00000000>";
    const float MAX_TYPE_TIME = 0.1f;
    bool isTyping = false;
    bool introShown = false;
    bool glitching = false;

    void Start()
    {
        p = introText.text;
        introText.text = "";

        analogGlitch = GetComponent<AnalogGlitch>();
        digitalGlitch = GetComponent<DigitalGlitch>();

        GlitchLow();

        introScreen.SetActive(false);
        controlsImage.SetActive(false);
        continueText.SetActive(false);
        fade.SetActive(false);

        RemoveCheckmark(checkmark1);
        RemoveCheckmark(checkmark2);
    }

    void Update()
    {   
        if (!Input.anyKeyDown || isTyping || glitching)
            return;
            
        if (!introShown)
            StartCoroutine(PlayIntro());
        else if (!controlsShown)
        {
            fade.SetActive(true);
            introScreen.SetActive(false);
            StartCoroutine(ShowControls());
        }
    }

    void LoadScene()
    {
        if (!isLoading)
        {
            StartCoroutine(LoadSceneAsync(sceneToPlay));
        }
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        isLoading = true;

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        isLoading = false;
    }


    IEnumerator PlayIntro()
    {
        glitching = true;
        GlitchHigh(false);

        yield return new WaitForSeconds(1f);

        GlitchOff(true);
        startText.SetActive(false);
        introScreen.SetActive(true);

        yield return new WaitForSeconds(1f);

        glitching = false;
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
        
        if (playSound)
            blackscreenSound.Play();
    }

    IEnumerator TypeDialogueText(string p)
    {
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
        continueText.SetActive(true);
    }

    IEnumerator ShowControls()
    {
        GlitchHigh(true);
        yield return new WaitForSeconds(1f);
        GlitchOff(false);
        Destroy(digitalGlitch);
        Destroy(analogGlitch);
        StartCoroutine(FadeIn());
        controlsImage.SetActive(true);
        controlsShown = true;


        while (!player1Ready || !player2Ready)
        {
            if (player1.GetInteract())
            {
                ShowCheckmark(checkmark1);
                player1Ready = true;

                if (!player2Ready)
                    ShowBlankChechmark(checkmark2);
            }
            else if (player2.GetInteract())
            {
                ShowCheckmark(checkmark2);
                player2Ready = true;

                if (!player1Ready)
                    ShowBlankChechmark(checkmark1);
            }

            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(0.5f);
        LoadScene();

    }

    void RemoveCheckmark(GameObject checkmark)
    {
        checkmark.transform.GetChild(0).gameObject.SetActive(false);
        checkmark.transform.GetChild(1).gameObject.SetActive(false);
        checkmark.SetActive(false);
    }

    void ShowCheckmark(GameObject checkmark)
    {
        checkmark.transform.GetChild(0).gameObject.SetActive(true);
        checkmark.transform.GetChild(1).gameObject.SetActive(true);
        checkmark.SetActive(true);
    }

    void ShowBlankChechmark(GameObject checkmark)
    {
        checkmark.transform.GetChild(0).gameObject.SetActive(true);
        checkmark.transform.GetChild(1).gameObject.SetActive(false);
        checkmark.SetActive(true);
    }

    IEnumerator FadeIn()
    {
        float newAlpha = 1f;

        while (fade.GetComponent<Image>().color.a > 0)
        {
            Color newColor = fade.GetComponent<Image>().color;
            newColor.a = newAlpha;

            fade.GetComponent<Image>().color = newColor;

            newAlpha -= decrementAlpha;

            yield return new WaitForSeconds(fadeInSpeed);
        }
    }
}
