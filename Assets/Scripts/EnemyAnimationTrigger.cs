using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationTrigger : MonoBehaviour
{
    protected Kincu enemy => GetComponentInParent<Kincu>();

    private void AnimationTrigger()
    {
        enemy.AnimationTrigger();
    }

    private void Update()
    {
        RaycastHit hitEnemy;
        if (Physics.Raycast(transform.position, Vector3.down, out hitEnemy, 1))
            AttackTrigger(hitEnemy);
    }
    private void AttackTrigger(RaycastHit hit_enemy)
    {

        Collider[] colliders = Physics.OverlapSphere(enemy.attackCheck.position, enemy.attackCheckRadius);

        foreach(var hit in colliders)
        {
            if(hit.GetComponent<Player_Move>() != null)
            {
                PlayerStats target = hit.GetComponent<PlayerStats>();
                enemy.stats.DoDamage(target);
            }
        }
        




    }
}
