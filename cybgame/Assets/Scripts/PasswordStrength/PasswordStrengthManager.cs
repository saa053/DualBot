using UnityEngine;
using Ink.Runtime;

public class PasswordStrengthManager : MonoBehaviour
{
    [Header("Mini-game settings")]
    [SerializeField] int numPasswords;

    [Header("NPC")]
    [SerializeField] Vector3 NPCLocalTargetPos;
    [SerializeField] Quaternion NPCTargetRot;
    [SerializeField] float NPCMoveSpeed;
    [SerializeField] float NPCRotSpeed;
    [SerializeField] Animator NPCAnimator;
    [SerializeField] Rigidbody NPC;
    [SerializeField] TextAsset inkJSON;
    Vector3 WorldTargetPos;

    bool NPCReady = false;
    bool screenOn = false;
    bool gameComplete = false;

    bool setTargetPos = false;


    int numAnswered = 0;

    public static PasswordStrengthManager instance;
    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if (gameComplete)
            return;

        if (screenOn)
        {
            if (numAnswered == numPasswords)
                GameComplete();
            return;
        }

        if (!ShouldStart())
            return;
        
        if (NPCReady)
        {
            PasswordScreen.instance.TurnOn();
            screenOn = true;
        }
    }

    void FixedUpdate()
    {
        if (!ShouldStart() || gameComplete || screenOn || NPCReady)
            return;
        
        MoveNPC();
    }

    bool ShouldStart()
    {
        Story story = new Story(inkJSON.text);
        string saveString = (string)story.variablesState["saveString"];

        bool result = DialogueSaveManager.instance.GetBool(story, saveString, "GO");
        return result;
    }

    void GameComplete()
    {
        gameComplete = true;

        PasswordScreen.instance.TurnOff();
        screenOn = false;
    }

    void MoveNPC()
    {
        if (!setTargetPos)
        {
            WorldTargetPos = NPC.transform.position + NPCLocalTargetPos;
            setTargetPos = true;
        }

        Vector3 direction = WorldTargetPos - NPC.transform.position;

        if (direction.magnitude > 0.1f)
        {
            NPC.velocity = direction.normalized * NPCMoveSpeed;
            NPCAnimator.SetBool("isMoving", true);

            Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(NPC.transform.rotation, toRotation, Time.deltaTime * NPCRotSpeed);
        }
        else
        {
            NPC.velocity = Vector3.zero;
            NPCAnimator.SetBool("isMoving", false);

            if (NPC.transform.rotation != NPCTargetRot)
            {
                RotateNPCToTarget();
            }
            else
                NPCReady = true;    
        }
    }

    void RotateNPCToTarget()
    {
        Quaternion toRotation = NPCTargetRot;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, Time.deltaTime * NPCRotSpeed);
    }

    public void SubmitAnswer(PlateType plateType)
    {
        switch (plateType)
        {
            case PlateType.Svakt:
                break;
            case PlateType.Middels:
                break;
            case PlateType.Sterkt:
                break;
            default:
                break;
        }

        numAnswered++;
        PasswordScreen.instance.NextPassword();
    }
}
