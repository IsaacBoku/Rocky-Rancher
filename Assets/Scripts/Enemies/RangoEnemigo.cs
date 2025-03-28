using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangoEnemigo : MonoBehaviour
{
    public Kincu enemy;
    public Animator ani;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ani.SetBool("walk", false);
            ani.SetBool("run", false);
            ani.SetBool("attack", true);
            enemy.atacando = true;
                Debug.Log("toco");
            GetComponent<CapsuleCollider>().enabled = false;
            PlayerStats _target = other.GetComponent<PlayerStats>();
            _target.DecreaseHealthBy(10);
        }
    }

}
