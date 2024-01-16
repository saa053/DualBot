using UnityEngine;

public class DoorController : MonoBehaviour
{

    [SerializeField] Vector2 direction;

    PlayerMovement player1;
    PlayerMovement player2;

    void Start()
    {
        player1 = GameObject.FindWithTag("Player1").GetComponent<PlayerMovement>();
        player2 = GameObject.FindWithTag("Player2").GetComponent<PlayerMovement>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player1" || other.tag == "Player2")
        {
            RoomController.instance.OnPlayerEnterDoor(direction);
            player1.EnterDoor(-direction);
            player2.EnterDoor(-direction);
        }
    }
}
