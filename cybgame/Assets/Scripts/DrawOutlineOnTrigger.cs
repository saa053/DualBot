using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawOutlineOnTrigger : MonoBehaviour
{
    Outline outline;
    Trigger trigger;
    bool transparent = false;
    void Start()
    {
        outline = GetComponent<Outline>();
        trigger = GetComponent<Trigger>();
    }

    void Update()
    {
        if (trigger.Player1Close() || trigger.Player2Close())
        {
            if (transparent)
            {
                outline.OutlineWidth = 0;
                return;
            }
            
            outline.OutlineWidth = 10;
        }
        else
        {
            outline.OutlineWidth = 0;
        }
    }

    public void SetTransparent(bool value)
    {
        transparent = value;
    }
}
