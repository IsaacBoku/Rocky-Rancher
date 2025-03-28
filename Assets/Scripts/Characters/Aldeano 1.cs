using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aldeano1 : InteracbleObject
{
    public List<DialogueLine> lines;
    public override void Pickup()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        DialogueManager.instance.StartDialogue(lines);
    }
}
