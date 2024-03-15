using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class AIManager : MonoBehaviour
{
    Room room;

    [Header("Colors")]
    [SerializeField] Color greenColor;
    [SerializeField] Color whiteColor;
    [Header("Interact")]
    [SerializeField] GameObject blueCheckmark;
    [SerializeField] GameObject orangeCheckmark;
    [SerializeField] GameObject screenBackground;
    [SerializeField] GameObject interactText;
    [SerializeField] float turnOnWaitTime;
    [SerializeField] Trigger trigger;
    [SerializeField] float timeBeforeFadeOut;
    [SerializeField] float timeBeforeCelebration;

    [Header("Text")]
    [SerializeField] TextMeshProUGUI numRewards;
    [SerializeField] GameObject errorCanvas;
    [SerializeField] GameObject interactCanvas;

    [Header("Blinking door")]
    [SerializeField] GameObject blinkingDoor;
    [SerializeField] DoorController door;

    [Header("Lights")]
    [SerializeField] GameObject spinningLight;
    [SerializeField] GameObject pointLight;
    [SerializeField] Material greenLitMaterial;
    [SerializeField] GameObject spinningLightObject;
    [SerializeField] int blinkTimes;
    [SerializeField] float blinkPause;
    [SerializeField] float lightIntensity;

    [Header("Audio")]
    [SerializeField] AudioSource lowPitchSound;
    [SerializeField] AudioSource highPitchSound;
    [SerializeField] AudioSource startUpSound;

    int currentNum;
    int neededNum;
    bool waitingOnPlayers = false;

    PlayerInputManager player1Input;
    PlayerInputManager player2Input;
    Animator player1Animator;
    Animator player2Animator;

    void Start()
    {
        player1Input = GameObject.FindWithTag("Player1").GetComponent<PlayerInputManager>();
        player2Input = GameObject.FindWithTag("Player2").GetComponent<PlayerInputManager>();
        player1Animator = GameObject.FindWithTag("Player1").GetComponentInChildren<Animator>();
        player2Animator = GameObject.FindWithTag("Player2").GetComponentInChildren<Animator>();

        room = GetComponentInParent<Room>();

        blueCheckmark.SetActive(false);
        orangeCheckmark.SetActive(false);
        screenBackground.SetActive(false);
        interactCanvas.SetActive(false);

        screenBackground.GetComponent<Image>().color = greenColor;
    }

    // Update is called once per frame
    void Update()
    {
        currentNum = RewardManager.instance.GetCount();
        neededNum = RewardManager.instance.GetNeeded();

        BlinkingDoor();
        UpdateScreens();

        if (currentNum < neededNum)
            return;
        
        if (!waitingOnPlayers)
            StartCoroutine(HandleStartUp());

    }


    void BlinkingDoor()
    {
        if (RoomController.instance.currentRoom == room)
        {
            blinkingDoor.GetComponent<Blink>().SetAplhaZero();
            blinkingDoor.SetActive(false);
        }
        else
        {
            if (!CameraController.instance.isMoving)
            {
                blinkingDoor.SetActive(true);
            }
        }

    }
    void UpdateScreens()
    {
        numRewards.text = currentNum.ToString() + " / " + neededNum.ToString();

        if (currentNum >= neededNum)
        {
            numRewards.color = greenColor;
            errorCanvas.SetActive(false);
            interactCanvas.SetActive(true);
        }
    }

   IEnumerator HandleStartUp()
    {
        waitingOnPlayers = true;
        bool player1Ready = false;
        bool player2Ready = false;

        while (!player1Ready || !player2Ready)
        {
            if (player1Ready || player2Ready)
            {
                if (!door.GetIsLocked())
                    door.CloseDoor();       
            }

            if (player1Input.GetInteract() && trigger.Player1Close() && !player1Ready)
            {
                blueCheckmark.SetActive(true);
                interactText.SetActive(false);

                player1Ready = true;

                if (!player2Ready)
                {
                    lowPitchSound.Play();
                }
                else
                {
                    highPitchSound.Play();
                }
            }
            else if (player2Input.GetInteract() && trigger.Player2Close() && !player2Ready)
            {
                orangeCheckmark.SetActive(true);
                interactText.SetActive(false);
                player2Ready = true;

                if (!player1Ready)
                {
                    lowPitchSound.Play();
                }
                else
                {
                    highPitchSound.Play();
                }
            }

            VisualCue(player1Ready, player2Ready);

            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(turnOnWaitTime);

        TurnOnAI();
    }

    void VisualCue(bool p1Ready, bool p2Ready)
    {
        if (trigger.Player1Close() && !p1Ready)
        {
            screenBackground.SetActive(true);
        }
        else if (trigger.Player2Close() && !p2Ready)
        {
            screenBackground.SetActive(true);
        }
        else
        {
            screenBackground.SetActive(false);
        }
    }

    void TurnOnAI()
    {
        startUpSound.Play();
        room.StopAmbience();

        StartCoroutine(LightManager.instance.TurnOnDirectionalBlink(blinkTimes, blinkPause, lightIntensity));
        spinningLight.SetActive(false);
        pointLight.SetActive(false);

        Material[] materials = spinningLightObject.GetComponent<MeshRenderer>().materials;
        materials[1] = greenLitMaterial;
        spinningLightObject.GetComponent<MeshRenderer>().materials = materials;

        StartCoroutine(Celebrate());
    }

    IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(timeBeforeFadeOut);

        Debug.Log("Fading...");
    }

    IEnumerator Celebrate()
    {
        yield return new WaitForSeconds(timeBeforeCelebration);

        player1Input.ToggleInputOffOn();
        player2Input.ToggleInputOffOn();

        player1Input.transform.GetComponent<NPCMove>().trigger = true;
        player2Input.transform.GetComponent<NPCMove>().trigger = true;

        player1Input.GetComponent<Collider>().isTrigger = true;
        player2Input.GetComponent<Collider>().isTrigger = true;

        StartCoroutine(CameraController.instance.ZoomCameraToTarget(room.GetRoomCenter()));

        while (!player1Input.transform.GetComponent<NPCMove>().ready || !player2Input.transform.GetComponent<NPCMove>().ready)
        {
            yield return new WaitForEndOfFrame();
        }
        
        player1Animator.SetBool("dance1", true);
        player2Animator.SetBool("dance2", true);

        StartCoroutine(FadeOut());
    }
}
