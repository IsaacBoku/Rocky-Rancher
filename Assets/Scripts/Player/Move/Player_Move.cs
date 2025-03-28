using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using static Land_Farm;

public class Player_Move : Entity
{
    #region Move
    [Header("Move info")]
    public new Transform camera;
    public float speed = 4;
    public float gravity = -9.8f;
    Vector3 motionVector;
    public Vector3 lastMotionVector;
    [Header("Cooldowns tools")]
    [SerializeField] private float cooldownTime;
    [SerializeField] private float cooldownHacha;
    [SerializeField] private float cooldownPico;
    [SerializeField] private float cooldownHoz;
    [SerializeField] private float cooldownRegadera;
    [SerializeField] private float cooldownAzada;

    #endregion
    #region Roll
    [Header("Roll")]
    [SerializeField] private float rollUsageTimer;
    [SerializeField] private float rollTime;
    [SerializeField] private float rollSpeed;
    public float rollCooldown;
    #endregion
    #region Component
    public CharacterController characterController;
    Animator ani;
    RaycastHit hit;
    Player_Interacion playerInteract;
    [HideInInspector] public Player_Stamina staminaController;
    protected bool triggerCalled;
    protected bool usedAni;

    public AudioSource pasos;
    private bool Hactivo;
    private bool vactivo;
    [Header("SFX")]
    [SerializeField] private AudioClip audioHacha;
    [SerializeField] private AudioClip audioPico;
    [SerializeField] private AudioClip audioAzada;
    [SerializeField] private AudioClip audioRegadera;
    [SerializeField] private AudioClip audioHoz;
    #endregion
    protected override void Start()
     {
        base.Start();
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = -1;
        ani = GetComponentInChildren<Animator>();
        characterController = GetComponent<CharacterController>();
        playerInteract = GetComponentInChildren<Player_Interacion>();
        staminaController = GetComponent<Player_Stamina>(); 
        triggerCalled = false;
     }
    private void Update()
    {
        
        Interact();
        
        Move();
       
        if (Input.GetKeyDown(KeyCode.Alpha2))
            Inventory.instance.UseFlask();
       
        if( Input.GetKeyDown(KeyCode.Space))
            DialogueManager.instance.UpdateDialogue();

        if(Input.GetKeyDown(KeyCode.V))
            UIManager.Instance.ToggleRelationshipPanel();
        CheckForRollInput();
    }
    #region RollCheck
    protected virtual void CheckForRollInput()
    {
        rollUsageTimer -= Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.LeftShift) && rollUsageTimer < 0)
        {
            
            rollUsageTimer = rollCooldown;
            ani.SetTrigger("roll");
            StartCoroutine(Roll());
           
            return;
            
        }
       
    }
    protected virtual IEnumerator Roll()
    {
        float startTime = Time.time;
        
        while (Time.time < startTime + rollTime)
        {
            
            transform.Translate(Vector3.forward * rollSpeed* Time.deltaTime);
            yield return null;
        }
    }
    #endregion
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }
    
    #region Movement Player
    public void Move()
    {
        float hor = Input.GetAxis("Horizontal");
        float ver = Input.GetAxis("Vertical");
        Vector3 movement = Vector3.zero;
        float movementSpeed = 0;
        motionVector = new Vector3(hor, ver);
        if (hor != 0 || ver != 0 && IsGroundDetected())
        {
            
            lastMotionVector = new Vector3(hor, ver).normalized;
            
            Vector3 forward = camera.forward;
            forward.y = 0;
            forward.Normalize();

            Vector3 right = camera.right;
            forward.y = 0;
            forward.Normalize();

            Vector3 dire = forward * ver + right * hor;
            movementSpeed = Mathf.Clamp01(dire.magnitude);
            dire.Normalize();

            movement = dire * speed * movementSpeed * Time.deltaTime;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dire), 0.2f);
        }

        if (Input.GetButtonDown("Horizontal"))
        {
            if(vactivo ==false)
            {

            Hactivo = true;
            pasos.Play();
            }

        }
        if (Input.GetButtonDown("Vertical"))
        {
            if(Hactivo ==false)
            {

            vactivo = true;
            pasos.Play();
            }
        }
        if (Input.GetButtonUp("Horizontal"))
        {
            Hactivo = false;
            if(vactivo == false)
                pasos.Pause();
        }
        if (Input.GetButtonUp("Vertical"))
        {
            vactivo = false ;
            if (Hactivo == false)
                pasos.Pause();
        }



        movement.y += gravity * Time.deltaTime;
        characterController.Move(movement);
        ani.SetFloat("Speed_Move", movementSpeed);
        
    }
    #endregion
    public void Interact()
    {
        cooldownTime -= Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Mouse1) && cooldownTime <0)
        {
            characterController.enabled = false;
            ItemData_Equipment equimentTool = Inventory.instance.GetEquipment(EquipmentType.Tool);
            if (equimentTool != null)
            {
                ItemData_Equipment.ToolType tooltype = equimentTool.toolType;

                switch (tooltype)
                {
                    case ItemData_Equipment.ToolType.Hoe:
                        cooldownTime = cooldownAzada;
                        ani.SetTrigger("Azada");
                        GetComponent<AudioSource>().PlayOneShot(audioAzada);
                        characterController.enabled = false;
                        StartCoroutine(AnimacionesAzada());
                        break;
                    case ItemData_Equipment.ToolType.WateringCan:
                        cooldownTime = cooldownRegadera;
                        ani.SetTrigger("Saxofon");
                        GetComponent<AudioSource>().PlayOneShot(audioRegadera);
                        characterController.enabled = false;
                        StartCoroutine(AnimacionesSaxo());
                        break;
                    case ItemData_Equipment.ToolType.Hoz:
                        cooldownTime = cooldownHoz;
                        ani.SetTrigger("Hoz");
                        GetComponent<AudioSource>().PlayOneShot(audioHoz);
                        characterController.enabled = false;
                        StartCoroutine(AnimacionesPico());
                        break;
                    case ItemData_Equipment.ToolType.Axe:
                        cooldownTime = cooldownHacha;
                        ani.SetTrigger("hacha");
                        GetComponent<AudioSource>().PlayOneShot(audioHacha);
                        characterController.enabled = false;
                        StartCoroutine(AnimacionesHacha());
                        break;
                    case ItemData_Equipment.ToolType.Pickaxe:
                        cooldownTime = cooldownPico;
                        ani.SetTrigger("Pico");
                        GetComponent<AudioSource>().PlayOneShot(audioPico);
                        characterController.enabled = false;
                        StartCoroutine(AnimacionesPico());
                        break;
                }
                return;
            }
        }
        if (Input.GetKeyDown(KeyCode.F))
            playerInteract.InteractObject();
        if (Input.GetKeyDown(KeyCode.F))
            playerInteract.InteractSeed();
    }
    IEnumerator AnimacionesHacha()
    {
        yield return new WaitForSeconds(3.3f);
        playerInteract.InteractItems();
        characterController.enabled = true;
    }
    IEnumerator AnimacionesAzada()
    {
        yield return new WaitForSeconds(2f);
        playerInteract.InteractItems();
        characterController.enabled = true;
    }
    IEnumerator AnimacionesPico()
    {
        yield return new WaitForSeconds(1.5f);
        playerInteract.InteractItems();
        characterController.enabled = true;
    }
    IEnumerator AnimacionesSaxo()
    {
        yield return new WaitForSeconds(2.5f);
        playerInteract.InteractItems();
        characterController.enabled = true;
    }


    #region AnimationTrigger
    public void AnimationTrigger()=> AnimationFinishTrigger();
    public virtual void AnimationFinishTrigger()=> triggerCalled = true;
    #endregion
}
