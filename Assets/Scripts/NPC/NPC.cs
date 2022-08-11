using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NPC : Interactable
{
    public string NPC_Name;

    public List<Dialogue> dialogues;
    public List<Prompt> prompts;

    public override void OnInteract()
    {
        print($"Interacted with {NPC_Name}");
        GameManager.instance.Player.GetComponent<PlayerMovement>().canMove = false;
        GameManager.instance.Player.GetComponent<PlayerMovement>().busy = true;
        GameManager.instance.UIManager.SetNpcDialogue(dialogues);
        GameManager.instance.UIManager.SetNpcPrompt(prompts);
        GameManager.instance.UIManager.SetNPC(this);
        GameManager.instance.UIManager.StartNPC();
    }

    public abstract void OnYesPrompt(string responseActionName);
    public abstract void OnNoPrompt(string responseActionName);
    public abstract void OnOtherPrompt(string responseActionName);
    public abstract void OnFinshInteraction();
}

[Serializable]
public class Dialogue
{
    public string text;
    public int prompt; // -1 no prompt, other numbers point to index in prompts list
}

[Serializable]
public class Prompt
{
    public string ResponseActionName;
    public enum PromptResponseType { Yes, No, Other };
    public string[] prompts;
    public PromptResponseType[] types;
}