using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Plates : MonoBehaviour
{
    [Header ("Objects")]
    [SerializeField] Renderer plate;
    [SerializeField] GameObject progressBar;
    [SerializeField] TextMeshProUGUI text;

    [Header ("Materials")]
    [SerializeField] Material activated;
    [SerializeField] Material defaultMaterial;

    [SerializeField] Color defaultTextColor;
    [SerializeField] Color activatedColor;

    [Header ("Progress bar")]

    [SerializeField] float finishLength;
    [SerializeField] float speedInSeconds;

    [SerializeField] float updateFrequency;

    [SerializeField] AudioSource audioSource;

    [Header ("Player settings")]
    [SerializeField] Vector3 restartPos;

    Transform player1;
    Transform player2;

    float progress = 0;
    bool player1OnPlate = false;
    bool player2OnPlate = false;

    void Start()
    {
        Room room = FindObjectOfType<Room>();
        restartPos += room.GetRoomCenter();
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
        
        if (player1OnPlate && player2OnPlate)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }

            StartCoroutine(UpdateProgress());
        }
        else
        {
            progress = 0;
        }
        
        UpdateProgressBar();

        if (progress == 1)
        {
            if (player1OnPlate)
                player1.position = restartPos + new Vector3(1, 0, 0);
            if (player2OnPlate)
                player2.position = restartPos + new Vector3(-1, 0, 0);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player1")
        {
            player1OnPlate = true;
            player1 = other.transform;
        }

        if (other.tag == "Player2")
        {
            player2OnPlate = true;
            player2 = other.transform;
        }

        other.transform.position = new Vector3(other.transform.position.x, transform.position.y, other.transform.position.z);
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player1")
        {
            progress = 0;
            player1OnPlate = false;
            audioSource.Stop();
            player1 = null;
        }

        if (other.tag == "Player2")
        {
            player2OnPlate = false;
            progress = 0;
            audioSource.Stop();
            player2 = null;
        }

        
        other.transform.position = new Vector3(other.transform.position.x, 0, other.transform.position.z);
    }

    IEnumerator UpdateProgress()
    {
        yield return new WaitForSeconds(updateFrequency);

        float increment = (1f / speedInSeconds) * updateFrequency;
        progress += increment;
        progress = Mathf.Clamp(progress, 0, 1);
    }

    void UpdateProgressBar()
    {
        Vector3 scale = progressBar.transform.localScale;

        float targetScaleY = Mathf.Lerp(0f, finishLength, progress);
        progressBar.transform.localScale = new Vector3(targetScaleY, scale.y, scale.z);
        
        if (progress == 0)
            progressBar.SetActive(false);
        else
            progressBar.SetActive(true);
    }
}