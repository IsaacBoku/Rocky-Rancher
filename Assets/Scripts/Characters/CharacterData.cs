using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="character/Character")]
public class CharacterData : ScriptableObject
{
    public GameTimeStap birthday;
    public List<ItemData_Oficial> likes;
    public List<ItemData_Oficial> dislikes;
    public GameObject prefab;
    public Sprite icon;

    [Header("Dialogue")]
    public List<DialogueLine> onFirstMeet;
    public List<DialogueLine> defaultDialogue;

    public List<DialogueLine> likedGiftDialogue;
    public List<DialogueLine> dislikedGiftDialogue;
    public List<DialogueLine> neutralGiftDialogue;

    public List<DialogueLine> birthdayLikedGiftDialogue;
    public List<DialogueLine> birthdayDislikedGiftDialogue;
    public List<DialogueLine> birthdayNeutralGiftDialogue;

}
