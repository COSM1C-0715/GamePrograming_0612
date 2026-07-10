using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    InputSystem_Actions action;

    [SerializeField]
    float accel;

    [SerializeField]
    GameCamera _GameCamera;

    [SerializeField]
    float MoveSpeed;

    [SerializeField]
    float JumpSpeed;

    [SerializeField]
    float groundNormalYMin;

    [SerializeField]
    float groundDamping = 8f;

    [SerializeField]
    float airDamping = 0.5f;

    Vector2 InputVec;

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
    }

    void OnDisable()
    {
        action.Player.Move.performed -= OnMoving;
        action.Player.Move.canceled -= MoveCancel;
        action.Player.Jump.started -= OnJump;
        action.Player.Sprint.performed -= OnRun;
        action.Player.Sprint.canceled -= OnRunCancel;
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
        Vector3 InputDir = _GameCamera.transform.right * InputVec.x + _GameCamera.transform.forward * InputVec.y;

        InputDir.y = 0;

        float Dir = Mathf.Atan2(InputDir.x, InputDir.z);

        Quaternion AngleDir = Quaternion.Euler(0.0f,Dir * Mathf.Rad2Deg,0.0f);

        if(isAccel)
        {
            transform.position = transform.position + (InputDir.normalized * accel) * Time.deltaTime;
        }
        else
        {
            transform.position = transform.position + (InputDir.normalized * MoveSpeed) * Time.deltaTime;
        }

        AnimWalk();

        if (InputVec != Vector2.zero)
            transform.rotation = AngleDir;
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

    void OnCollisionStay(Collision col)
    {
        foreach(var contact in col.contacts)
        {
            if(contact.normal.y >= groundNormalYMin)
            {
                isGrounded = true;
            }
        }
    }
}
