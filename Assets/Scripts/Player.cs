using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Video;

public class Player : MonoBehaviour
{
    InputSystem_Actions action;

    [SerializeField]
    GameObject FirePrefab;

    [SerializeField]
    float accel;

    [SerializeField]
    GameCamera _GameCamera;

    [SerializeField]
    float MoveSpeed;

    [SerializeField]
    float JumpSpeed;

    [SerializeField]
    float RotateSpeed;

    [SerializeField]
    float FireSpeed;

    [SerializeField]
    float groundNormalYMin;

    [SerializeField]
    float groundDamping = 8f;

    [SerializeField]
    float airDamping = 0.5f;

    [SerializeField]
    float hp = 3;

    [SerializeField]
    float invincibleTimeMax = 0.5f;

    [SerializeField]
    float knockbackSpeed = 5;

    float invincibleTime;

    Vector2 InputVec;

    Vector3 offset = new Vector3(0.0f,0.0f,1.0f);

    Rigidbody rb;

    Animator animator;

    bool isGrounded;

    bool isAccel;
    void Awake()
    {
        action = new InputSystem_Actions();
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    void OnEnable()
    {
        action.Enable();
        action.Player.Move.performed += OnMoving;
        action.Player.Move.canceled += MoveCancel;
        action.Player.Jump.started += OnJump;
        action.Player.Sprint.performed += OnRun;
        action.Player.Sprint.canceled += OnRunCancel;
        action.Player.Attack.started += OnAttack;
    }

    void OnDisable()
    {
        action.Player.Move.performed -= OnMoving;
        action.Player.Move.canceled -= MoveCancel;
        action.Player.Jump.started -= OnJump;
        action.Player.Sprint.performed -= OnRun;
        action.Player.Sprint.canceled -= OnRunCancel;
        action.Player.Attack.started -= OnAttack;
        action.Disable();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    void FixedUpdate()
    {
        if(isGrounded)
        {
            rb.linearDamping = groundDamping;
        }
        else
        {
            rb.linearDamping = airDamping;
        }
        isGrounded = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (invincibleTime > 0)
        {
            invincibleTime -= Time.deltaTime;
        }
        Vector3 InputDir = _GameCamera.transform.right * InputVec.x + _GameCamera.transform.forward * InputVec.y;

        InputDir.y = 0;
        if(isAccel)
        {
            transform.position = transform.position + (InputDir.normalized * accel) * Time.deltaTime;
        }
        else
        {
            transform.position = transform.position + (InputDir.normalized * MoveSpeed) * Time.deltaTime;
        }

        AnimWalk();

        if (InputVec != Vector2.zero&&transform.forward!=InputDir)
            transform.forward = Vector3.Slerp(transform.forward,InputDir,RotateSpeed * Time.deltaTime);
    }
    void OnMoving(InputAction.CallbackContext cont)
    {
        if(cont.performed)
        {
            InputVec = cont.ReadValue<Vector2>();
        }
    }

    void MoveCancel(InputAction.CallbackContext cont)
    {
        if (cont.canceled)
        {
            InputVec = Vector2.zero;
        }
    }

    void OnJump(InputAction.CallbackContext cont)
    {
        if(cont.started&&!isGrounded)
        {
            Vector3 JumpVec = new Vector3(0.0f,JumpSpeed,0.0f);

            rb.AddForce(JumpVec,ForceMode.VelocityChange);
        }
    }

    void OnRun(InputAction.CallbackContext cont)
    {
        if(cont.performed&&isGrounded)
        {
            isAccel = true;
        }
    }

    void OnRunCancel(InputAction.CallbackContext cont)
    {
        if(cont.canceled)
        {
            isAccel = false;
        }
    }

    void AnimWalk()
    {
        Vector3 velocityXZ = rb.linearVelocity;
        velocityXZ.y = 0;
        animator.SetFloat("MoveSpeed",velocityXZ.magnitude);
        if(isAccel)
        {
            
        }
        else
        {

        }
    }

    void OnAttack(InputAction.CallbackContext cont)
    {
        if(cont.started)
        {
            var pos = transform.position + transform.forward;

            GameObject obj = Instantiate(FirePrefab, pos, Quaternion.identity);

            var obj_rb = obj.GetComponent<Rigidbody>();

            obj_rb.linearVelocity = transform.forward * FireSpeed;
        }
    }

    void OnCollisionStay(Collision col)
    {
        foreach(var contact in col.contacts)
        {
            if(contact.normal.y >= groundNormalYMin)
            {
                isGrounded = true;
            }
        }
        var attackObj = col.gameObject.GetComponent<AttackObject>();

        if (attackObj != null && invincibleTime <= 0)
        {
            hp -= attackObj.power;
            invincibleTime = invincibleTimeMax;
            if (hp <= 0)
            {
                Destroy(gameObject);
            }

            var dir = transform.position - col.transform.position;
            dir.y = 0;
            var knockbackVec = dir.normalized * knockbackSpeed;
            rb.AddForce(knockbackVec,ForceMode.VelocityChange);
        }
    }
}
