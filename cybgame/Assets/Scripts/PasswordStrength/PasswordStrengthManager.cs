using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;

public class PasswordStrengthManager : MonoBehaviour
{
    void Update()
    {
        bool result = DialogueManager.instance.GetBool("GO");
        if (result)
            Debug.Log("SUCCESS");
    }
}
