using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float speed;
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
}
