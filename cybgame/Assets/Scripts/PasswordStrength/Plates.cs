using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public enum PlateType
{
    Svakt,
    Middels,
    Sterkt
}

public class Plates : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] AudioSource progressBarSound;
    [SerializeField] AudioSource onSound1;
    [SerializeField] AudioSource onSound2;

    [Header ("Objects")]
    [SerializeField] PlateType plateType;
    [SerializeField] Renderer plate;
    [SerializeField] GameObject progressBar;
    [SerializeField] TextMeshProUGUI text;

    [Header ("Materials")]
    [SerializeField] Material activated;
    [SerializeField] Material defaultMaterial;

    [SerializeField] Color defaultTextColor;
    [SerializeField] Color activatedColor;

    [Header ("Progress bar")]
    [SerializeField] float progressBarLength;
    [SerializeField] float speedInSeconds;

    [Header ("Player settings")]
    Transform player1;
    Transform player2;

    float progress = 0;
    public bool player1OnPlate = false;
    public bool player2OnPlate = false;

    void Start()
    {
        progressBar.SetActive(false);
    }

    void Update()
    {
        if (player1OnPlate || player2OnPlate)
        {
            plate.material = activated;
            text.color = activatedColor;
        }
        else
        {
            plate.material = defaultMaterial;
            text.color = defaultTextColor;
        }

        // UPDATE PROGRESS BAR
        if (PasswordStrengthManager.instance.gameActive && !PasswordStrengthManager.instance.displayingResult)
        {
            if (player1OnPlate && player2OnPlate && !PasswordScreen.instance.isTyping)
            {
                UpdateProgress();
            }
            else
            {
                progress = 0;
            }
            
            UpdateProgressBar();

            if (progress == 1)
            {
                PasswordStrengthManager.instance.SubmitAnswer(plateType);
                progress = 0;
            }
        }

        if (PasswordStrengthManager.instance.gameComplete)
        {
            if (progress == 0)
                progressBar.SetActive(false);
            else
                progressBar.SetActive(true);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player1")
        {
            player1OnPlate = true;
            player1 = other.transform;

            if (player2OnPlate)
                onSound2.Play();
            else
                onSound1.Play();
        }

        if (other.tag == "Player2")
        {
            player2OnPlate = true;
            player2 = other.transform;

            if (player1OnPlate)
                onSound2.Play();
            else
                onSound1.Play();
        }

        other.transform.position = new Vector3(other.transform.position.x, transform.position.y, other.transform.position.z);
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player1")
        {
            progress = 0;
            player1OnPlate = false;
            player1 = null;
        }

        if (other.tag == "Player2")
        {
            player2OnPlate = false;
            progress = 0;
            player2 = null;
        }

        
        other.transform.position = new Vector3(other.transform.position.x, 0, other.transform.position.z);
    }

    void UpdateProgress()
    {
        float increment = Time.deltaTime / speedInSeconds;
        progress += increment;
        progress = Mathf.Clamp01(progress);
    }

    void UpdateProgressBar()
    {
        Vector3 scale = progressBar.transform.localScale;

        float targetScaleY = Mathf.Lerp(0f, progressBarLength, progress);
        progressBar.transform.localScale = new Vector3(targetScaleY, scale.y, scale.z);

        if (progress == 0)
        {
            progressBarSound.Stop();
            progressBar.SetActive(false);
        }
        else
        {   
            if (!progressBarSound.isPlaying)
                progressBarSound.Play();
                
            progressBar.SetActive(true);
        }
    }
}
