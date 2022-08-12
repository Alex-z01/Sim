using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcInteractArgs : EventArgs
{
    public PlayerMovement PlayerMovement { get; }
    public List<Dialogue> Dialogues { get; }
    public List<Prompt> Prompts { get; }
    public NpcInteractArgs(PlayerMovement plyrMov, List<Dialogue> dialgs, List<Prompt> prmpts)
    {
        PlayerMovement = plyrMov;
        Dialogues = dialgs;
        Prompts = prmpts;
    }
}

public abstract class NPC : Interactable
{
    public string NPC_Name;

    [SerializeField]
    private List<Dialogue> _dialogues;
    [SerializeField]
    private List<Prompt> _options;

    public EventHandler<NpcInteractArgs> OnInteractEvent;

    private void Start()
    {
        StartCoroutine(LazyLoad());
    }

    IEnumerator LazyLoad()
    {
        yield return new WaitForSeconds(1);
        if (GameManager.instance != null)
            GameManager.instance.UIManager.SubscribeNpcEvent(this);
    }


    private void OnDisable()
    {
        GameManager.instance.UIManager.UnsubscribeNpcEvent(this);
    }

    public override void OnInteract()
    {
        if (OnInteractEvent != null)
        {
            OnInteractEvent.Invoke(this, new NpcInteractArgs(GameManager.instance.Player.GetComponent<PlayerMovement>(), _dialogues, _options));
        }        
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