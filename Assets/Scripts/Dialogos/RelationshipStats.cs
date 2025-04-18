using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelationshipStats : MonoBehaviour
{
    public static List<NPCRelationshipState> relationships = new List<NPCRelationshipState>();

    public enum GiftReaction
    {
        Like,Dislike,Neutral
    }
    public static bool FirstMeeting(CharacterData character)
    {
        return !relationships.Exists(i => i.name == character.name);
    }
    public static NPCRelationshipState GetRelationship(CharacterData character)
    {
        if(FirstMeeting(character)) return null;
        return relationships.Find(i => i.name == character.name);
    }
    public static void UnlockCharacter(CharacterData character)
    {
        relationships.Add(new NPCRelationshipState(character.name));
    }
    public static void AddFriendPoints(CharacterData character, int points)
    {
        if(FirstMeeting(character)) return;
        GetRelationship(character).friendshipPoints += points;
    }
    public static bool IsFirstConversationOfTheDay(CharacterData character)
    {
        if(FirstMeeting(character)) return true;

        NPCRelationshipState npc = GetRelationship(character);

        return !npc.hasTalkedToday;
    }

    public static bool GiftGivenToday(CharacterData character)
    {
        NPCRelationshipState npc = GetRelationship(character);
        return npc.giftGivenToday;
    }
    public static GiftReaction GetGiftReaction(CharacterData character,ItemData_Oficial item) 
    {
        if(character.likes.Contains(item)) return GiftReaction.Like;

        if (character.dislikes.Contains(item)) return GiftReaction.Dislike;

        return GiftReaction.Neutral;

    }

    public static bool IsBirthday(CharacterData character)
    {
        GameTimeStap birthday = character.birthday;
        GameTimeStap today = TimeManager.Instance.GetGameTimesStap();

        return (today.day == birthday.day)&& (today.season == birthday.season);
    }

}
