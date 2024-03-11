using UnityEngine;

public class DoorController : MonoBehaviour
{
    AudioSource[] doorSounds;
    [SerializeField] Vector2 direction;

    Animator animator;

    bool hidden = false;
    public bool locked = false;
    bool noRoom = false;

    PlayerMovement player1;
    PlayerMovement player2;

    PlayerInputManager player1Input;
    PlayerInputManager player2Input;

    void Start()
    {
        animator = GetComponent<Animator>();

        player1 = GameObject.FindWithTag("Player1").GetComponent<PlayerMovement>();
        player2 = GameObject.FindWithTag("Player2").GetComponent<PlayerMovement>();

        player1Input = GameObject.FindWithTag("Player1").GetComponent<PlayerInputManager>();
        player2Input = GameObject.FindWithTag("Player2").GetComponent<PlayerInputManager>();

        doorSounds = GetComponents<AudioSource>();
    }

    void Update()
    {
        if (RoomController.instance.isLoadingRoom)
            return;

        if (!DoesDoorLeadToRoom())
        {
            noRoom = true;
            //SetSpriteTransparency(GetComponent<SpriteRenderer>(), 0);
        }

        UpdateAnimation();
    }

    public void OpenDoor()
    {
        locked = false;
        doorSounds[0].Play();
    }
    public void CloseDoor()
    {
        locked = true;
        doorSounds[1].Play();
        Debug.Log("Closing");
    }

    void UpdateAnimation()
    { 
        if (locked)
            animator.SetBool("isOpen", false);
        else
            animator.SetBool("isOpen", true);
    }

    bool DoesDoorLeadToRoom()
    {
        Room room = GetComponentInParent<Room>();

        (int x, int y) = room.GetGridPos();
        x += (int)direction.x;
        y += (int)direction.y;

        return RoomController.instance.DoesRoomExist(x, y);
    }

    void OnTriggerEnter(Collider other)
    {
        if (locked || hidden || noRoom)
            return;

        if (player1Input.IsLifting() || player2Input.IsLifting())
            return;

        if (other.tag == "Player1" || other.tag == "Player2")
        {
            RoomController.instance.OnPlayerEnterDoor(direction);
            player1.EnterDoor(-direction);
            player2.EnterDoor(-direction);
        }
    }
}
