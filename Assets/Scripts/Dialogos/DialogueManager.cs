using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance { get; private set; }

    [Header("Dialogue componets")]
    public GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI speakerText;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private Image sprite;

    Player_Move player;

    CharacterData characterData;

    Queue<DialogueLine> dialogue;
    Action onDialogueEnd = null;

    bool isTyping = false;
    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this);
        else
            instance = this;
     
    }
    private void Start()
    {
        player = GetComponent<Player_Move>();
    }
    public void StartDialogue(List<DialogueLine> dialogueLinesToQueue)
    {
        dialogue = new Queue<DialogueLine>(dialogueLinesToQueue);
         UpdateDialogue();

    }
    public void StartDialogue(List<DialogueLine> dialogueLinesToQueue,Action onDialogueEnd)
    {
        StartDialogue(dialogueLinesToQueue);
       
        this.onDialogueEnd = onDialogueEnd; 
    }
    public void UpdateDialogue()
    {
        if (isTyping)
        {
            isTyping = false;
            return;
        }
        dialogueText.text = string.Empty;
        if(dialogue.Count == 0)
        {
            EndDialogue();
            return;
        }
        DialogueLine line = dialogue.Dequeue();
        
        Talk(line.speaker, line.message);
    }
    public void EndDialogue()
    {
        dialoguePanel.SetActive(false);

        onDialogueEnd?.Invoke();

        onDialogueEnd = null;
    }
    public void Talk(string speaker, string message)
    {
        dialoguePanel.SetActive(true);
        speakerText.text = speaker;
        speakerText.transform.parent.gameObject.SetActive(speaker != "");
        StartCoroutine(TypeText(message));
      
    }
    IEnumerator TypeText(string textToType)
    {
        isTyping = true;
        char[] charsToType = textToType.ToCharArray();
        for(int i = 0; i < charsToType.Length; i++)
        {
            dialogueText.text += charsToType[i];
            yield return new WaitForEndOfFrame();
            if (!isTyping)
            {
                dialogueText.text = textToType;
                break;
            }
        }
        isTyping = false;
    }

    public static List<DialogueLine> CreateSimpleMessage(string message)
    {
        DialogueLine messageDialogueLine = new DialogueLine("",message);

        List<DialogueLine> listToReturn = new List<DialogueLine>();

        listToReturn.Add(messageDialogueLine);

        return listToReturn;
    }

    public void CambiarSprite()
    {
        sprite.sprite = characterData.icon;
    }
}
