using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{

    protected bool triggerCalled;
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override void Start()
    {
        base.Start();
        triggerCalled = false;
    }
    
    public void AnimationTrigger() => AnimationFinishTrigger();
    public virtual void AnimationFinishTrigger()
    {
        triggerCalled = true;
    }
}
