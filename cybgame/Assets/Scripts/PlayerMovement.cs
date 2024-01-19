using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float roomEntryPlayerSpacing;
    [SerializeField] float enterRoomX;
    [SerializeField] float enterRoomZ;
    Vector3 input;
    Rigidbody body;

    void Start()
    {
        body = GetComponent<Rigidbody>();
    }

    public void OnInput(InputAction.CallbackContext context)
    {
        Vector2 newInput = context.ReadValue<Vector2>();
        input = new Vector3(newInput.x, 0, newInput.y);
    }

    void FixedUpdate()
    {
        body.velocity = input * speed;
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
