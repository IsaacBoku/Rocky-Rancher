using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Entity : MonoBehaviour
{
    public static Entity instance;

    #region Componentes
    public Rigidbody rb {  get; private set; }
    public Animator anim {  get; private set; }
    public CharacterStats stats { get; private set; }



    #endregion



    [SerializeField] protected LayerMask whatIsCollision;

    [Header("Collision info")]
    public Transform attackCheck;
    public float attackCheckRadius;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] public LayerMask whatIsGroud;
    [SerializeField] public float sizeOfInteractableArea;


    public System.Action onFlipped;
    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
    }


    protected virtual void FixedUpdate()
    {
        
    }

    #region Collision
    public virtual bool IsGroundDetected() => Physics.Raycast(groundCheck.position, Vector3.down, groundCheckDistance, whatIsGroud);
    public virtual bool IsRollDetected() => Physics.Raycast(transform.position, Vector3.down, whatIsCollision);

   

    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance, groundCheck.position.z));
        Gizmos.DrawWireSphere(attackCheck.position, attackCheckRadius);

    }
    #endregion

    #region RollMove


    /*protected virtual void RollController(InputAction.CallbackContext callback)
    {

        if (callback.performed)
        {
            if (rollUsageTimer < 0)
            {
                rollUsageTimer = rollCooldown;
                StartCoroutine(Roll());
                return;
            }
        }

    }*/



    #endregion

    #region Velocity
    public void ZeroVelocity() => rb.velocity = new Vector3(0, 0, 0);
    public void SetVelocity(float _xVelocity, float _yVelocity, float _zVelocity)
    {
        rb.velocity = new Vector3(_xVelocity, _yVelocity, _zVelocity);
    }
    #endregion

    #region Flip
    public virtual void Flip()
    {


        if (onFlipped != null)
            onFlipped();
    }
    #endregion
    public virtual void Die()
    {

    }
}
