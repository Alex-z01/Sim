using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    GameObject player;

    Camera mainCam;

    public Room currentRoom;
    public List<Room> rooms;

    public float camX, camY;
    public float camSpeed;

    private void Start()
    {
        mainCam = Camera.main;
        player = GameManager.instance.Player;
        currentRoom = rooms[0];
    }

    private void Update()
    {
        camX = player.transform.position.x;
        camY = player.transform.position.y;



        camX = Mathf.Clamp(camX, currentRoom.leftX + (mainCam.orthographicSize*2), currentRoom.rightX - (mainCam.orthographicSize * 2));
        camY = Mathf.Clamp(camY, currentRoom.botY+mainCam.orthographicSize, currentRoom.topY-mainCam.orthographicSize);

        mainCam.transform.position = Vector3.MoveTowards(mainCam.transform.position, new Vector3(camX, camY, -10), camSpeed * Time.deltaTime);
    }

    public void SetRoom(int roomIdx)
    {
        currentRoom = rooms[roomIdx];
    }

    public void SetRoom(string roomName)
    {
        currentRoom = rooms.Find(x => x.name == roomName);
    }
    
}

[Serializable]
public class Room
{
    public string name;

    public float leftX, rightX;
    public float topY, botY;
}
