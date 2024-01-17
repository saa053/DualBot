using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float roomEntryPlayerSpacing;
    [SerializeField] Vector2 enterRoomPos;
    Vector2 input;
    Rigidbody2D body;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }

    public void OnInput(InputAction.CallbackContext context)
    {
        input = context.ReadValue<Vector2>();
    }

    void FixedUpdate()
    {
        body.velocity = input * speed;
    }

    public void EnterDoor(Vector2 direction)
    {
        Vector2 targetPos = RoomController.instance.currentRoom.GetRoomCenter() + enterRoomPos * direction;
        if (direction.x == 0)
        {
            targetPos.x += (this.tag == "Player1") ? roomEntryPlayerSpacing : -roomEntryPlayerSpacing;
        }
        else
        {
            targetPos.y += (this.tag == "Player1") ? roomEntryPlayerSpacing : -roomEntryPlayerSpacing;
        }


        transform.position = targetPos;
    }
}
