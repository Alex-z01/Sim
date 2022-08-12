using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BartyTheCat : NPC
{
    public GameObject shopMenuPrefab;

    public override void OnNoPrompt(string responseActionName)
    {
        print("Selected type no option");
        if(responseActionName == "Shop")
        {
            print("Shop was denied, exit npc interaction");
            OnFinshInteraction();
        }
    }

    public override void OnOtherPrompt(string responseActionName)
    {
        print("Selected type other option");
    }

    public override void OnYesPrompt(string responseActionName)
    {
        print("Selected type yes option");
        if(responseActionName == "Shop")
        {
            GameManager.instance.UIManager.inPrompt = false;
            GameManager.instance.UIManager.ToggleDialogue();
            GameManager.instance.UIManager.OpenNpcMenu(shopMenuPrefab);
        }
    }

    public override void OnFinshInteraction()
    {
        print("Done with this NPC");
        GameManager.instance.UIManager.EndNPC();
        GameManager.instance.Player.GetComponent<PlayerMovement>().CanMove = true;
        GameManager.instance.Player.GetComponent<PlayerMovement>().Busy = false;
    }
}
