using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PLayerAnimationTriggers : MonoBehaviour
{
    
    private Player_Move player => GetComponentInParent<Player_Move>();
    private void AnimationTrigger()
    {
        player.AnimationTrigger();
       
    }
    private void AttackTrigger(RaycastHit hitplayer)
    {
        Collider colliders = hitplayer.collider;
        if(colliders.tag == "Enemy")
        {
            if (colliders.GetComponent<Enemy>() != null)
            {
                EnemyStats _target = colliders.GetComponent<EnemyStats>();

                Debug.Log("toco");

                player.stats.DoDamage(_target);
            }
        }
    }
}
