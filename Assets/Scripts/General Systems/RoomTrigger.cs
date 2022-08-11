using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTrigger : MonoBehaviour
{
    RoomManager RoomManager;

    public int roomIdx;

    private void Start()
    {
        RoomManager = GameManager.instance.RoomManager;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            RoomManager.SetRoom(roomIdx);
        }
    }
}
