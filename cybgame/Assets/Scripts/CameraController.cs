using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;
    public Room currentRoom;
    public float moveSpeed;

    Vector3 initalPosition;
    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        initalPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePosition();
    }

    void UpdatePosition()
    {
        currentRoom = RoomController.instance.currentRoom;
        if (currentRoom == null)
            return;
        
        Vector3 targetPos = GetCameraTargetPosition();
        transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * moveSpeed);
    }

    Vector3 GetCameraTargetPosition()
    {
        if (currentRoom == null)
            return transform.position;

        Vector3 targetPos = currentRoom.GetRoomCenter();
        targetPos += initalPosition;

        return targetPos;
    }
}
