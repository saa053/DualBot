using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float speed;

    [SerializeField] float rotationSpeed;
    [SerializeField] float roomEntryPlayerSpacing;
    [SerializeField] float enterRoomX;
    [SerializeField] float enterRoomZ;

    Rigidbody body;
    PlayerInputManager inputManager;

    Animator animator;

    void Start()
    {
        body = GetComponent<Rigidbody>();
        inputManager = GetComponent<PlayerInputManager>();
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (!animator)
            return; 
            
        if (inputManager.isMoving())
            animator.SetBool("isMoving", true);
        else
            animator.SetBool("isMoving", false);
    }

    void FixedUpdate()
    {
        Vector3 input = inputManager.GetMoveInput();
        if (DialogueManager.instance.dialogueIsPlaying)
            input = Vector3.zero;

        body.velocity = input * speed;

        if (inputManager.isMoving())
        {
            Vector3 desiredDirection = body.velocity.normalized;
            Quaternion targetRotation = Quaternion.LookRotation(desiredDirection, Vector3.up);
            animator.transform.rotation = Quaternion.Slerp(animator.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
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
