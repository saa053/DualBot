using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour
{
    bool interact = false;
    bool move = false;

    bool isLifting = false;

    Vector2 newInput;
    Vector3 input;

    public void InteractPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
            interact = true;
        else if (context.canceled)
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
        else if (context.canceled)
        {
            move = false;
            input = Vector3.zero;
        }
    }

    public bool GetInteract()
    {
        bool result = interact;
        interact = false;
        return result;
    }

    public bool GetMove()
    {
        bool result = move;
        move = false;
        return result;
    }

    public Vector3 GetMoveInput()
    {
        return input;
    }

    public bool IsLifting()
    {
        return isLifting;
    }

    public void SetIsLifting(bool res)
    {
        isLifting = res;
    }
}
