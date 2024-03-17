using System.Collections;
using Kino;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Zoom")]
    [SerializeField] float zoomSpeed;
    [SerializeField] float minDistance;

    [Header("Transitions")]
    [SerializeField] bool lerpTransition;
    [SerializeField] bool glitchTransition;
    [SerializeField] bool fadeInNOutTransition;

    [Header("Lerp Settings")]

    [SerializeField] AudioSource lerpSound;
    public float moveSpeed;
    public bool isMoving;

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
    bool isZooming;
    
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
        isMoving = false;

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
            //Debug.Log("ERROR: Too many or no camera transition chosen!");
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
        if (isZooming)
            return;

        currentRoom = RoomController.instance.currentRoom;
        if (currentRoom == null)
            return;
        
        Vector3 targetPos = GetCameraTargetPosition();

        if (transform.position == targetPos || isFading || isGlitching)
        {
            isMoving = false;
            return;
        }

        if (lerpTransition)
        {
            isMoving = true;
            if (!lerpSound.isPlaying)
                lerpSound.Play();
            transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * moveSpeed);
            return;
        }
        else if (fadeInNOutTransition)
            StartCoroutine(FadeTransition(targetPos));
        else
            StartCoroutine(GlitchTransition(targetPos));
    }

    public bool IsMoving()
    {
        return isMoving;
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

    public IEnumerator ZoomCameraToTarget(Vector3 target)
    {
        isZooming = true;

        float distance = Vector3.Distance(transform.position, target);
        while (distance > minDistance)
        {
            float step = zoomSpeed * Time.deltaTime;

            if (distance > minDistance)
                transform.position = Vector3.MoveTowards(transform.position, target, step);
            
            distance = Vector3.Distance(transform.position, target);
            yield return new WaitForEndOfFrame();
        }
    }
}
