using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NPCRelationShipLIsting : MonoBehaviour
{
    List<CharacterData> characters;

    [Header("Sprites")]
    public Sprite emptyHeart, fullHeart;

    public Image[] hearts;

    [Header("UI Elements")]
    public Image image;
    public TextMeshProUGUI nameText;


    public void Display(CharacterData characterData,NPCRelationshipState relationship)
    {
        image.sprite = characterData.icon;
        nameText.text = relationship.name;

        

       //if(characterData == null) LoadAllCharacters();

        DisplayHearts(relationship.Hearts());
    }

    void DisplayHearts(float number)
    {
        foreach (Image heart in hearts)
        {
            heart.sprite = emptyHeart;
        }

        for(int i = 0; i < number; i++)
        {
            hearts[i].sprite = fullHeart;
        }
    }

    void LoadAllCharacters()
    {
        characters = NPCManager.Instance.Characters();
    }
}
