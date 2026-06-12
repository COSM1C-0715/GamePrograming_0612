using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    InputSystem_Actions action;

    [SerializeField]
    float MoveSpeed;

    [SerializeField]
    Transform MainCamera;

    [SerializeField]
    Vector2 InputVec = new Vector2();

    [SerializeField]
    Vector2 LookVec = new Vector2();

    void Awake()
    {
        action = new InputSystem_Actions();
    }

    void OnEnable()
    {
        action.Enable();
        action.Player.Move.performed += OnMoving;
        action.Player.Move.canceled += MoveCancel;
        action.Player.Look.performed += OnLookCamera;
    }

    void OnDisable()
    {
        action.Player.Move.performed -= OnMoving;
        action.Player.Move.canceled -= MoveCancel;
        action.Player.Look.performed -= OnLookCamera;
        action.Disable();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 InputDir = new Vector3(InputVec.x,0.0f,InputVec.y);

        float Dir = Mathf.Atan2(InputVec.x, InputVec.y);

        Quaternion AngleDir = Quaternion.Euler(0.0f,Dir * Mathf.Rad2Deg,0.0f);

        transform.position = transform.position + (InputDir.normalized * MoveSpeed) * Time.deltaTime;

        if(InputVec!=Vector2.zero)
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

    void OnLookCamera(InputAction.CallbackContext cont)
    {
        if(cont.performed)
        {
            LookVec = cont.ReadValue<Vector2>();
        }
    }
}
