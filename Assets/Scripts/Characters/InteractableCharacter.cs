using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterMovement))]
public class InteractableCharacter : Interacion_Objetos
{
    public CharacterData characterData;
    NPCRelationshipState relationship;
    Quaternion defaultRotation;
    bool isTurning = false;
    CharacterMovement movement;
    private void Start()
    {
        relationship = RelationshipStats.GetRelationship(characterData);
        movement = GetComponent<CharacterMovement>();
        defaultRotation = transform.rotation;
        GameStateManager.Instance.onIntervalUpdate.AddListener(OnIntervalUpdate);
    }
    void OnIntervalUpdate()
    {
        NPCLocationState locationState = NPCManager.Instance.GetNPCLocation(characterData.name);
        movement.MoveTo(locationState);
        StartCoroutine(LookAt(Quaternion.Euler(locationState.facing)));
    }
    public override void Pickup()
    {
        LookAtPlayer();
        TriggerDialogue();
    }
    private void TriggerDialogue()
    {
        
        movement.ToggleMovement(false);
        if (Inventory.instance.GetMaterial(MaterialType.Material))
        {
            GiftDialogue();
            return;
        }
        List<DialogueLine> conversationToHave = characterData.defaultDialogue;

        System.Action onDialogueEnd = () =>
        {
            movement.ToggleMovement(true);
            OnIntervalUpdate();
        };
        //onDialogueEnd += ResetRotation;
        if (RelationshipStats.FirstMeeting(characterData))
        {
            conversationToHave = characterData.onFirstMeet;
            onDialogueEnd += OnFirstMeeting;
        }
        if (RelationshipStats.IsFirstConversationOfTheDay(characterData))
        {
            Debug.Log("this is the first conversation of the day");
            onDialogueEnd += OnFirstConversation;
        }
        DialogueManager.instance.StartDialogue(conversationToHave, onDialogueEnd);
    }
    void LookAtPlayer()
    {
        Transform player = FindObjectOfType<Player_Move>().transform;
        Vector3 dir = player.position - transform.position;
        dir.y = 0;
        Quaternion lookRot = Quaternion.LookRotation(dir);
        StartCoroutine(LookAt(lookRot));
    }
    IEnumerator LookAt(Quaternion lookRot)
    {
        if (isTurning)
            isTurning = false;
        else
            isTurning = true;
        while (transform.rotation != lookRot)
        {
            if (!isTurning)
            {
                yield break;
            }
            if(!movement.IsMoving()) transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRot, 720 * Time.fixedDeltaTime);
            yield return new WaitForFixedUpdate();
        }
        isTurning = false;
    }
    void ResetRotation()
    {
        Time.timeScale = 1;
        StartCoroutine(LookAt(defaultRotation));
    }
    void GiftDialogue()
    {
        if (!EligibleForGift()) return;
        ItemData_Oficial handSlot = Inventory.instance.GetMaterial(MaterialType.Material);
        List<DialogueLine> dialogueToHave = characterData.neutralGiftDialogue;
        System.Action onDialogueEnd = () =>
        {
            OnIntervalUpdate();
            movement.ToggleMovement(true);
            relationship.giftGivenToday = true;
            ItemData_Material currentItem = Inventory.instance.GetMaterial(MaterialType.Material);
            Inventory.instance.RemoveMaterial(currentItem);
        };
        //onDialogueEnd += ResetRotation;
        bool isBirthday = RelationshipStats.IsBirthday(characterData);
        float pointsToAdd = 0;
        switch (RelationshipStats.GetGiftReaction(characterData, handSlot))
        {
            case RelationshipStats.GiftReaction.Like:
                dialogueToHave = characterData.likedGiftDialogue;
                pointsToAdd = 80;
                if (isBirthday) dialogueToHave = characterData.birthdayLikedGiftDialogue;
                break;
            case RelationshipStats.GiftReaction.Dislike:
                dialogueToHave = characterData.dislikedGiftDialogue;
                pointsToAdd = -20;
                if (isBirthday) dialogueToHave = characterData.birthdayDislikedGiftDialogue;
                break;
            case RelationshipStats.GiftReaction.Neutral:
                dialogueToHave = characterData.neutralGiftDialogue;
                pointsToAdd = 20;
                if (isBirthday) dialogueToHave = characterData.birthdayNeutralGiftDialogue;
                break;
        }
        if(isBirthday)
        {
            pointsToAdd *= 8;
        }
        RelationshipStats.AddFriendPoints(characterData,(int) pointsToAdd);
        DialogueManager.instance.StartDialogue(dialogueToHave, onDialogueEnd);
    }
    bool EligibleForGift()
    {
        if (RelationshipStats.FirstMeeting(characterData))
        {
            DialogueManager.instance.StartDialogue(DialogueManager.CreateSimpleMessage("You have not unlocked this character yet"));
            return false;
        }
        if(RelationshipStats.GiftGivenToday(characterData))
        {
            DialogueManager.instance.StartDialogue(DialogueManager.CreateSimpleMessage($"You have already given {characterData.name}a gift today"));
            return false;
        }
        return true;
    }
    void OnFirstMeeting()
    {
        RelationshipStats.UnlockCharacter(characterData);
        relationship = RelationshipStats.GetRelationship(characterData);
    }
    void OnFirstConversation()
    {
        RelationshipStats.AddFriendPoints(characterData, 20);
        relationship.hasTalkedToday = true;
    }
}
