using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public int width;
    public int height;
    public int x;
    public int y;

    void Start()
    {
        if(RoomController.instance == null)
        {
            Debug.Log("You pressed play in the wrong scene!");
            return;
        }

        RoomController.instance.RegisterRoom(this);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(width, 0, height));
    }

    public Vector3 GetRoomCenter()
    {
        float paddingX = RoomController.instance.paddingX;
        float paddingY = RoomController.instance.paddingY;
        return new Vector3(x * width * paddingX, 0, y * height * paddingY);
    }

    public (int, int) GetGridPos()
    {
        return (x, y);
    }
}
