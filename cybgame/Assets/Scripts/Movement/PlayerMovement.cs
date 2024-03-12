using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float speed;

    [SerializeField] float rotationSpeed;
    [SerializeField] float roomEntryPlayerSpacing;
    [SerializeField] float enterRoomX;
    [SerializeField] float enterRoomZ;

    Rigidbody body;
    PlayerInputManager inputManager;
    NPCMove npcMove;

    Animator animator;

    void Start()
    {
        body = GetComponent<Rigidbody>();
        inputManager = GetComponent<PlayerInputManager>();
        animator = GetComponentInChildren<Animator>();
        npcMove = GetComponent<NPCMove>();
    }

    void Update()
    {
        if (!animator || npcMove.GetManualMove())
            return; 
            
        if (inputManager.isMoving())
            animator.SetBool("isMoving", true);
        else
            animator.SetBool("isMoving", false);
    }

    void FixedUpdate()
    {
        if (npcMove.GetManualMove())
            return;
            
        Vector3 input = inputManager.GetMoveInput();
        if (DialogueManager.instance.dialogueIsPlaying)
            input = Vector3.zero;

        body.velocity = input * speed;

        if (inputManager.isMoving())
        {
            RotatePlayer();
        }
    }

    void RotatePlayer()
    {
        Vector3 desiredDirection = body.velocity.normalized;
        Quaternion targetRotation = Quaternion.LookRotation(desiredDirection, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    public void EnterDoor(Vector2 direction)
    {
        Vector3 enterRoomPos = new Vector3(enterRoomX * direction.x, 0, enterRoomZ * direction.y);
        Vector3 targetPos = RoomController.instance.currentRoom.GetRoomCenter() + enterRoomPos;

        if (direction.x == 0)
        {
            targetPos.x += (this.tag == "Player1") ? roomEntryPlayerSpacing : -roomEntryPlayerSpacing;
        }
        else
        {
            targetPos.z += (this.tag == "Player1") ? roomEntryPlayerSpacing : -roomEntryPlayerSpacing;
        }


        targetPos.y = transform.position.y;
        transform.position = targetPos;
    }
}
