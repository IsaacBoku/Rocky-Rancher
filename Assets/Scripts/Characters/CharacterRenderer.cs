using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterMovement),typeof(Animator))]
public class CharacterRenderer : MonoBehaviour
{
    CharacterMovement movement;
    Animator ani;
    void Start()
    {
        ani = GetComponent<Animator>();
        movement = GetComponent<CharacterMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        ani.SetBool("Walk", movement.IsMoving());
    }
}
