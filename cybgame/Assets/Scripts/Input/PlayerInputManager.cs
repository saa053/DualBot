using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour
{
    bool interact = false;
    bool move = false;

    Vector2 newInput;
    Vector3 input;

    public void InteractPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
            interact = true;
        else
            interact = false;
    }

    public void MovePressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            move = true;
            newInput = context.ReadValue<Vector2>();
            input = new Vector3(newInput.x, 0, newInput.y);
        }
        else
        {
            move = false;
            input = Vector3.zero;
        }
    }

    public bool GetInteract()
    {
        return interact;
    }

    public bool GetMove()
    {
        return move;
    }

    public Vector3 GetMoveInput()
    {
        return input;
    }
}
