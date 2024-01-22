using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class DeviceHandler : MonoBehaviour
{
    [SerializeField] int playerIndex;
    PlayerInput playerInput;
    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        InputUser.PerformPairingWithDevice(Keyboard.current, playerInput.user, InputUserPairingOptions.None);
    }

    void Update()
    {
        PairControllers();
    }

    void PairControllers()
    {
        if (Gamepad.all.Count == 0)
            return;
        
        if (playerIndex == 0)
            InputUser.PerformPairingWithDevice(Gamepad.all[0], playerInput.user, InputUserPairingOptions.None);

        if (playerIndex == 1 && Gamepad.all.Count > 1)
            InputUser.PerformPairingWithDevice(Gamepad.all[1], playerInput.user, InputUserPairingOptions.None);
        
    }
}
