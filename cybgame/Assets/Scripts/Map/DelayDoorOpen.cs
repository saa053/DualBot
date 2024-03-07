using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayDoorOpen : MonoBehaviour
{
    [SerializeField] float waitForNSeconds;
    DoorController doorController;
    void Start()
    {
        doorController = transform.GetComponent<DoorController>();
        StartCoroutine(DelayOpen());
    }

    IEnumerator DelayOpen()
    {
        yield return new WaitForSeconds(waitForNSeconds);
        doorController.OpenDoor();
    }
}
