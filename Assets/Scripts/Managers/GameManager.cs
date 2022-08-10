using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }
    public RoomManager RoomManager { get; private set; }
    public UIManager UIManager { get; private set; }
    public GameObject Player;

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(this);
            return;
        }
        instance = this;
        RoomManager = GetComponent<RoomManager>();
        UIManager = GetComponent<UIManager>();
    }

}
