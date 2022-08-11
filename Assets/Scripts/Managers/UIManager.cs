using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject DialogueBox;
    public GameObject PromptBox;
    public GameObject PromptContainer;
    //public GameObject ShopDisplay;

    public Canvas mainCanvas;

    public NPC currentNPC;
    public List<Dialogue> NPCdialogue;
    public List<Prompt> NPCprompts;

    public GameObject promptPrefab;

    public int dialogueIdx, promptIdx;
    public int promptChoice;

    public bool inDialogue, inPrompt;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && inDialogue && !inPrompt)
        {
            dialogueIdx++;
            try
            {
                DialogueBox.GetComponentInChildren<Text>().text = NPCdialogue[dialogueIdx].text;
                if(NPCdialogue[dialogueIdx].prompt != -1)
                {
                    PromptBox.SetActive(true);
                    promptIdx = NPCdialogue[dialogueIdx].prompt;

                    for(int i = NPCprompts[promptIdx].prompts.Length-1; i > -1; i--)
                    {
                        GameObject prompt = Instantiate(promptPrefab, PromptContainer.transform);
                        prompt.name = $"Prompt {i}";
                        prompt.GetComponent<Text>().text = NPCprompts[promptIdx].prompts[i];
                    }

                    inPrompt = true;
                }
            }
            catch(IndexOutOfRangeException e)
            {
                Debug.Log(e);
                DialogueBox.SetActive(false);
                currentNPC.OnFinshInteraction();
            }
        }

        if(inPrompt)
        {
            if(Input.GetKeyDown(KeyCode.UpArrow))
            {
                promptChoice++;
                promptChoice = Mathf.Clamp(promptChoice, 0, NPCprompts[promptIdx].prompts.Length - 1);

                int counter = NPCprompts[promptIdx].prompts.Length - 1;
                foreach (Transform textObj in PromptContainer.transform)
                {
                    textObj.GetComponent<Text>().text = NPCprompts[promptIdx].prompts[counter];
                    counter--;
                }
            }
            if(Input.GetKeyDown(KeyCode.DownArrow))
            {
                promptChoice--;
                promptChoice = Mathf.Clamp(promptChoice, 0, NPCprompts[promptIdx].prompts.Length - 1);

                int counter = NPCprompts[promptIdx].prompts.Length - 1;
                foreach (Transform textObj in PromptContainer.transform)
                {
                    textObj.GetComponent<Text>().text = NPCprompts[promptIdx].prompts[counter];
                    counter--;
                }
            }

            PromptContainer.transform.Find($"Prompt {promptChoice}").GetComponent<Text>().text = $">> {NPCprompts[promptIdx].prompts[promptChoice]}";


            if(Input.GetKeyDown(KeyCode.Return))
            {
                if(NPCprompts[promptIdx].types[promptChoice] == Prompt.PromptResponseType.Yes)
                {
                    currentNPC.OnYesPrompt(NPCprompts[promptIdx].ResponseActionName);
                }
                else if (NPCprompts[promptIdx].types[promptChoice] == Prompt.PromptResponseType.No)
                {
                    currentNPC.OnNoPrompt(NPCprompts[promptIdx].ResponseActionName);
                }
                else if (NPCprompts[promptIdx].types[promptChoice] == Prompt.PromptResponseType.Other)
                {
                    currentNPC.OnOtherPrompt(NPCprompts[promptIdx].ResponseActionName);
                }
                PromptBox.SetActive(false);
                ClearPromptContainer();
            }
        }
    }

    public void ClearPromptContainer()
    {
        foreach (Transform obj in PromptContainer.transform)
        {
            Destroy(obj.gameObject);
        }
    }

    public void OpenNpcMenu(GameObject menu)
    {
        GameObject npcMenu = Instantiate(menu, mainCanvas.transform);
        npcMenu.name = $"{currentNPC.NPC_Name}'s {menu.name}";
    }

    public void ToggleDialogue()
    {
        bool toggle = DialogueBox.activeSelf ? false : true;

        DialogueBox.SetActive(toggle);
    }

    public void SetNPC(NPC npc)
    {
        currentNPC = npc;
    }

    public void SetNpcDialogue(List<Dialogue> npcDialogue)
    {
        NPCdialogue = npcDialogue;
    }

    public void SetNpcPrompt(List<Prompt> npcPrompt)
    {
        NPCprompts = npcPrompt;
    }

    public void StartNPC()
    {
        DialogueBox.SetActive(true);
        inDialogue = true;
        DialogueBox.GetComponentInChildren<Text>().text = NPCdialogue[0].text;
    }

    public void EndNPC()
    {
        DialogueBox.SetActive(false);
        inDialogue = false;
        inPrompt = false;

        currentNPC = null;
        NPCdialogue = null;
        NPCprompts = null;

        dialogueIdx = 0;
        promptIdx = 0;
        promptChoice = 0;
    }
}
