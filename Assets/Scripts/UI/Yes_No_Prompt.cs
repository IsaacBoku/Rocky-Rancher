using System;
using TMPro;
using UnityEngine;

public class Yes_No_Prompt : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI promptText;

    Action onYesSelected = null;

    public void CreatePrompt(string message, Action onYesSelected)
    {
        Cursor.visible = true;
        this.onYesSelected = onYesSelected;
        promptText.text = message;
    }

    public void Answer(bool yes)
    {
        if (yes && onYesSelected != null)
        {
            onYesSelected();
            Time.timeScale = 1f;
        }

        onYesSelected = null;
        Time.timeScale = 1f;
        gameObject.SetActive(false);
        Cursor.visible = false;
    }
}
