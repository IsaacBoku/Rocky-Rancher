using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bed : Interacion_Objetos
{
    public override void Pickup()
    {
        UIManager.Instance.TriggerYesNoPrompt("Do you want to sleep?", GameStateManager.Instance.Sleep);
    }
}
