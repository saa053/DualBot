using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public class RoomInfo
{
    public string name;
    public int x;
    public int y;
}

public class RoomController : MonoBehaviour
{
    [Header ("Map Settings")]
    [SerializeField] List<RoomInfo> roomsToLoad;

    public static RoomController instance;

    RoomInfo currentLoadRoomData;

    public Room currentRoom;

    Queue<RoomInfo> loadRoomQueue = new Queue<RoomInfo>();

    public List<Room> loadedRooms = new List<Room>();

    bool isLoadingRoom = false;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        foreach (RoomInfo room in roomsToLoad)
        {
            LoadRoom(room.name, room.x, room.y);
        }
    }

    void Update()
    {
        UpdateRoomQueue();
    }

    void UpdateRoomQueue()
    {
        if (isLoadingRoom)
            return;

        if (loadRoomQueue.Count == 0)
            return;

        currentLoadRoomData = loadRoomQueue.Dequeue();
        isLoadingRoom = true;
        StartCoroutine(LoadRoomRoutine(currentLoadRoomData));
    }

    public void LoadRoom(string name, int x, int y)
    {
        if (DoesRoomExist(x, y))
            return;

        RoomInfo newRoomData = new RoomInfo
        {
            name = name,
            x = x,
            y = y
        };

        loadRoomQueue.Enqueue(newRoomData);
    }

    IEnumerator LoadRoomRoutine(RoomInfo info)
    {
        string roomName = info.name;

        AsyncOperation loadRoom = SceneManager.LoadSceneAsync(roomName, LoadSceneMode.Additive);

        while(!loadRoom.isDone)
            yield return null;
    }

    public void RegisterRoom(Room room)
    {
        room.transform.position = new Vector3
        (
            currentLoadRoomData.x * room.width,
            currentLoadRoomData.y * room.height,
            0
        );

        room.x = currentLoadRoomData.x;
        room.y = currentLoadRoomData.y;
        room.name = currentLoadRoomData.name + " " + room.x + ", " + room.y;
        room.transform.parent = transform;

        if (loadedRooms.Count == 0)
        {
            currentRoom = room;
        }

        loadedRooms.Add(room);

        isLoadingRoom = false;
    }

    public bool DoesRoomExist(int x, int y)
    {
        return loadedRooms.Find(item => item.x == x && item.y == y) != null;
    }

    public void OnPlayerEnterDoor(Vector2 direction)
    {
        (int x, int y) = currentRoom.GetGridPos();

        Vector2 currentGridPos = new Vector2(x, y);
        Vector2 targetGridPos = currentGridPos + direction;

        Room targetRoom = loadedRooms.Find(item => item.x == targetGridPos.x && item.y == targetGridPos.y);
        if (targetRoom == null)
            return;

        currentRoom = targetRoom;
    }
}
