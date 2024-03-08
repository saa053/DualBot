using System.Collections;
using Kino;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Transitions")]
    [SerializeField] bool lerpTransition;
    [SerializeField] bool glitchTransition;
    [SerializeField] bool fadeInNOutTransition;

    [Header("Lerp Settings")]

    [SerializeField] AudioSource lerpSound;
    public float moveSpeed;

    [Header("Fade Settings")]
    [SerializeField] GameObject fadeObject;
    [SerializeField] float fadeOutTime;
    [SerializeField] float fadeInTime;

    bool isFading; 

    [Header("Glitch Settings")]
    [SerializeField] AudioSource glitchSound;
    [SerializeField] DigitalGlitch digitalGlitch;
    [SerializeField] AnalogGlitch analogGlitch;
    [SerializeField] float glitchTime;

    [SerializeField] float digitalIntensity;
    [SerializeField] float colorDrift;
    [SerializeField] float scanLineJitter;
    [SerializeField] float horizontalShake;

    bool isGlitching;
    
    [Header("Other")]
    public static CameraController instance;
    public Room currentRoom;

    Vector3 initalPosition;
    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        isGlitching = false;
        isFading = false;

        initalPosition = transform.position;

        int n = 0;
        if (lerpTransition)
            n++;
        if (glitchTransition)
            n++;
        if (fadeInNOutTransition)
            n++;
        
        if (n != 1)
        {
            Debug.Log("ERROR: Too many or no camera transition chosen!");
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePosition();
    }

    void UpdatePosition()
    {
        currentRoom = RoomController.instance.currentRoom;
        if (currentRoom == null)
            return;
        
        Vector3 targetPos = GetCameraTargetPosition();

        if (transform.position == targetPos || isFading || isGlitching)
        {
            if (lerpSound.isPlaying)
                lerpSound.Stop();
                
            return;
        }

        if (lerpTransition)
        {
            lerpSound.Play();
            transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * moveSpeed);
            return;
        }
        else if (fadeInNOutTransition)
            StartCoroutine(FadeTransition(targetPos));
        else
            StartCoroutine(GlitchTransition(targetPos));
    }

    Vector3 GetCameraTargetPosition()
    {
        if (currentRoom == null)
            return transform.position;

        Vector3 targetPos = currentRoom.GetRoomCenter();
        targetPos += initalPosition;

        return targetPos;
    }

    IEnumerator FadeTransition(Vector3 targetPos)
    {
        yield return new WaitForSeconds(fadeOutTime);

        yield return new WaitForSeconds(fadeInTime);
    }

    IEnumerator GlitchTransition(Vector3 targetPos)
    {
        Debug.Log("Test");
        isGlitching = true;
        
        glitchSound.Play();

        digitalGlitch.intensity = digitalIntensity;
        analogGlitch.colorDrift = colorDrift;
        analogGlitch.scanLineJitter = scanLineJitter;
        analogGlitch.horizontalShake = horizontalShake;

        yield return new WaitForSeconds(glitchTime / 2);

        transform.position = targetPos;

        yield return new WaitForSeconds(glitchTime / 2);

        glitchSound.Stop();

        digitalGlitch.intensity = 0;
        analogGlitch.colorDrift = 0;
        analogGlitch.scanLineJitter = 0;
        analogGlitch.horizontalShake = 0;

        isGlitching = false;
    }
}
