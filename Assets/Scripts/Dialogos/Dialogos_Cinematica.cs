using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Dialogos_Cinematica : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;

    public string[] lines;

    public float textSpeed = 0.1f;

    int index;

    private void Start()
    {
        dialogueText.text = string.Empty;
        StartDialogue();
    }
    void StartDialogue()
    {
        index = 0;

        StartCoroutine(WriteLine());
    }

    IEnumerator WriteLine()
    {
        foreach(char letter in lines[index].ToCharArray())
        {
            dialogueText.text += letter;

            yield return new WaitForSeconds(textSpeed);
        }
    }
    public void NextLine()
    {
        if(index < lines.Length - 1)
        {
            index++;
            dialogueText.text = string.Empty;
            StartCoroutine (WriteLine());

        }
        else
        {
            SceneManager.LoadScene("Cinematica");
        }
    }
}
