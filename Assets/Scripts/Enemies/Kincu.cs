using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Kincu : Enemy
{
    public int rutina;
    public float cronometro;
    public Quaternion angulo;
    public float grado;
    Animator ani;

    public GameObject target;
    public bool atacando;

    public RangoEnemigo rango;

    protected override void Start()
    {
        base.Start();
        ani = GetComponent<Animator>();
        target = GameObject.Find("Personaje");
    }
    private void Update()
    {
        COmportamiento_Enemigo();
    }
    public void COmportamiento_Enemigo()
    {
        if (Vector3.Distance(transform.position, target.transform.position) > 7)
        {
            ani.SetBool("run", false);
        cronometro += 1 * Time.deltaTime;

        if(cronometro >= 4)
        {
            rutina = Random.Range(0, 2);
            cronometro = 0;
        }

        switch (rutina)
        {
            case 0:
                ani.SetBool("walk", false);
                break;
            case 1:
                grado = Random.Range(0, 360);
                angulo = Quaternion.Euler(0,grado,0);
                rutina++;
                break;
            case 2:
                transform.rotation = Quaternion.RotateTowards(transform.rotation, angulo, 0.5f);
                transform.Translate(Vector3.forward*1*Time.deltaTime);
                ani.SetBool("walk",true); break;
        }

        }
        else
        {
            var lookPos = target.transform.position - transform.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);
            if(Vector3.Distance(transform.position,target.transform.position) > 0.5f && !atacando)
            {

            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 2);
            ani.SetBool("walk", false);

            ani.SetBool("run", true);
            transform.Translate(Vector3.forward * 2* Time.deltaTime);

                ani.SetBool("attack", false);
            }
            else
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 2);
                ani.SetBool("walk", false);
                ani.SetBool("run", false);

             
            }
        }

    }

    public void Final_Ani()
    {
        ani.SetBool("attack", false);
        atacando = false;
        //rango.GetComponent<CapsuleCollider>().enabled = true;
        
        
    }

}
