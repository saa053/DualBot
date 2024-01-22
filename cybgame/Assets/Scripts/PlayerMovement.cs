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

    void Start()
    {
        body = GetComponent<Rigidbody>();
        inputManager = GetComponent<PlayerInputManager>();
    }

    void FixedUpdate()
    {
        if (DialogueManager.instance.dialogueIsPlaying)
            return;

        Vector3 input = inputManager.GetMoveInput();
        body.velocity = input * speed;

        /* if (body.velocity.magnitude > 0.1f) // Check if the character is moving
        {
            Vector3 desiredDirection = body.velocity.normalized;
            Quaternion targetRotation = Quaternion.LookRotation(desiredDirection, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        } */
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
