using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public NPC currentNPC;

    public bool inPrompt;
    [SerializeField]
    private GameObject dialogueBox;
    [SerializeField]
    private GameObject optionBox;
    [SerializeField]
    private GameObject promptContainer;
    [SerializeField]
    private GameObject npcMenuContainer;

    private List<Dialogue> _NPCdialogue;
    private List<Prompt> _NPCprompts;

    [SerializeField]
    private GameObject _fishingUI;
    [SerializeField]
    private GameObject _fishingMeter;
    [SerializeField]
    private GameObject _catchMeter;

    [SerializeField]
    private GameObject _promptPrefab;

    private int _dialogueIdx, _optionBoxIdx;
    private int _promptChoice;

    public void SubscribeNpcEvent(NPC curNPC)
    {
        curNPC.OnInteractEvent += NpcInteractEvent;
    }

    public void UnsubscribeNpcEvent(NPC curNPC)
    {
        curNPC.OnInteractEvent -= NpcInteractEvent;
    }

    private void NpcInteractEvent(object sender, NpcInteractArgs e)
    {
        SetNpcDialogue(e.Dialogues);
        SetNpcPrompt(e.Prompts);
        SetNPC((NPC)sender);
        StartNPC();
    }

    void RaiseException()
    {
        throw new Exception("Dialog box index error!");
    }

    private void Update()
    {
        if (inPrompt)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow)) ChangeSelection(KeyCode.UpArrow);
            if (Input.GetKeyDown(KeyCode.DownArrow)) ChangeSelection(KeyCode.DownArrow);

            promptContainer.transform.Find($"Prompt {_promptChoice}").GetComponent<Text>().text = $">> {_NPCprompts[_optionBoxIdx].prompts[_promptChoice]}";

            if (Input.GetKeyDown(KeyCode.Return)) Respond();

        }
        if (Input.GetKeyDown(KeyCode.Space) && inPrompt) MakeSelection();
        if (Input.GetKeyDown(KeyCode.Space) && !inPrompt && currentNPC != null) AdvanceDialogue(); 

    }

    public void EnableFishingUI()
    {
        _fishingUI.SetActive(true);

        CreateFishingMeter();
    }

    public void DisableFishingUI()
    {
        _fishingUI.SetActive(false);
    }

    public void FishingSlider(float value)
    {
        _fishingUI.GetComponentInChildren<Slider>().value = value;
    }

    public RectTransform GetMeterInfo()
    {
        return _fishingMeter.GetComponent<RectTransform>();
    }

    public RectTransform GetCatchInfo()
    {
        return _catchMeter.GetComponent<RectTransform>(); 
    }

    private void CreateFishingMeter()
    {
        var meterHalf = _fishingMeter.GetComponent<RectTransform>().rect.width / 2;

        var catchWidth = UnityEngine.Random.Range(30, meterHalf);
        var catchHalf = catchWidth / 2;

        var minCenter = -meterHalf + catchHalf;
        var maxCenter = meterHalf - catchHalf;
        var catchCenter = UnityEngine.Random.Range(minCenter, maxCenter);

        Rect rect = new Rect();
        rect.x = catchCenter;
        rect.width = catchWidth;

        _catchMeter.GetComponent<RectTransform>().localPosition = new Vector2(catchCenter, _catchMeter.transform.localPosition.y);
        _catchMeter.GetComponent<RectTransform>().sizeDelta = new Vector2(catchWidth, _catchMeter.GetComponent<RectTransform>().rect.height);
    }



    private void AdvanceDialogue()
    {
        _dialogueIdx++;
        var box = dialogueBox.GetComponentInChildren<Text>();
        if (box == null || _dialogueIdx > _NPCdialogue.Count - 1)
            RaiseException();

        box.text = _NPCdialogue[_dialogueIdx].text;

        if (_NPCdialogue[_dialogueIdx].prompt < 0) return;

        CreatePrompts();
    }

    private void CreatePrompts()
    {
        optionBox.SetActive(true);
        _optionBoxIdx = _NPCdialogue[_dialogueIdx].prompt;

        for (int i = _NPCprompts[_optionBoxIdx].prompts.Length - 1; i > -1; i--)
        {
            GameObject prompt = Instantiate(_promptPrefab, promptContainer.transform);
            prompt.name = $"Prompt {i}";
            prompt.GetComponent<Text>().text = _NPCprompts[_optionBoxIdx].prompts[i];
        }
        inPrompt = true;
    }

    private void ChangeSelection(KeyCode arrowKey)
    {
        if (arrowKey == KeyCode.DownArrow)
            _promptChoice--;
        else
            _promptChoice++;
        _promptChoice = Mathf.Clamp(_promptChoice, 0, _NPCprompts[_optionBoxIdx].prompts.Length - 1);

        var counter = _NPCprompts[_optionBoxIdx].prompts.Length - 1;
        foreach (Transform textObj in promptContainer.transform)
        {
            textObj.GetComponent<Text>().text = _NPCprompts[_optionBoxIdx].prompts[counter];
            counter--;
        }

    }
    private void MakeSelection()
    {
        _optionBoxIdx = _NPCdialogue[_dialogueIdx].prompt;

        for (int i = _NPCprompts[_optionBoxIdx].prompts.Length - 1; i > -1; i--)
        {
            GameObject prompt = Instantiate(_promptPrefab, promptContainer.transform);
            prompt.name = $"Prompt {i}";
            prompt.GetComponent<Text>().text = _NPCprompts[_optionBoxIdx].prompts[i];
        }
    }
    private void Respond()
    {
        if (_NPCprompts[_optionBoxIdx].types[_promptChoice] == Prompt.PromptResponseType.Yes)
        {
            currentNPC.OnYesPrompt(_NPCprompts[_optionBoxIdx].ResponseActionName);
        }
        else if (_NPCprompts[_optionBoxIdx].types[_promptChoice] == Prompt.PromptResponseType.No)
        {
            currentNPC.OnNoPrompt(_NPCprompts[_optionBoxIdx].ResponseActionName);
        }
        else if (_NPCprompts[_optionBoxIdx].types[_promptChoice] == Prompt.PromptResponseType.Other)
        {
            currentNPC.OnOtherPrompt(_NPCprompts[_optionBoxIdx].ResponseActionName);
        }

        optionBox.SetActive(false);
        ClearPromptContainer();
    }
    public void ClearPromptContainer()
    {
        foreach (Transform obj in promptContainer.transform)
        {
            Destroy(obj.gameObject);
        }
    }
    public void OpenNpcMenu(GameObject menu)
    {
        foreach (Transform obj in npcMenuContainer.transform)
        {
            var transformShop = obj.GetComponent<Shop>();
            var menuShop = menu.GetComponent<Shop>();

            if (transformShop == null
                || menuShop == null)
                continue;

            if (ShopNamesInvalid(menuShop, transformShop))
            {
                transformShop.gameObject.SetActive(false);
                continue;
            }

            print($"{menuShop.ShopName}, {transformShop.ShopName}");
            OpenShopMenu(obj);

            return;
        }
        var npcMenu = Instantiate(menu, npcMenuContainer.transform);
        npcMenu.name = $"{currentNPC.NPC_Name} - {menu.GetComponent<Shop>().ShopName}";
    }
    private bool ShopNamesInvalid(Shop menuShop, Shop transformShop)
    {
        return menuShop.ShopName != transformShop.ShopName;
    }
    private void OpenShopMenu(Transform obj)
    {
        obj.gameObject.SetActive(true);
    }
    public void ToggleDialogue()
    {
        var toggle = !dialogueBox.activeSelf;
        dialogueBox.SetActive(toggle);
    }
    private void SetNPC(NPC npc)
    {
        currentNPC = npc;
    }
    private void SetNpcDialogue(List<Dialogue> npcDialogue)
    {
        _NPCdialogue = npcDialogue;
    }
    private void SetNpcPrompt(List<Prompt> npcPrompt)
    {
        _NPCprompts = npcPrompt;
    }
    private void StartNPC()
    {
        GameManager.instance.Player.GetComponent<PlayerMovement>().Busy = true;
        GameManager.instance.Player.GetComponent<PlayerMovement>().CanMove = false;

        dialogueBox.SetActive(true);
        dialogueBox.GetComponentInChildren<Text>().text = _NPCdialogue[0].text;
    }
    public void EndNPC()
    {
        dialogueBox.SetActive(false);
        inPrompt = false;

        currentNPC = null;
        _NPCdialogue = null;
        _NPCprompts = null;

        _dialogueIdx = 0;
        _optionBoxIdx = 0;
        _promptChoice = 0;
    }
}