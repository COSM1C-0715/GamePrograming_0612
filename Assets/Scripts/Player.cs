using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    InputSystem_Actions action;

    [SerializeField]
    GameCamera _GameCamera;

    [SerializeField]
    float MoveSpeed;

    [SerializeField]
    Vector2 InputVec;
    void Awake()
    {
        action = new InputSystem_Actions();
    }

    void OnEnable()
    {
        action.Enable();
        action.Player.Move.performed += OnMoving;
        action.Player.Move.canceled += MoveCancel;
    }

    void OnDisable()
    {
        action.Player.Move.performed -= OnMoving;
        action.Player.Move.canceled -= MoveCancel;
        action.Disable();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var CameraDir = _GameCamera.transform.forward + _GameCamera.transform.right;

        Debug.Log(CameraDir);

        CameraDir.y = 0f;

        Vector3 InputDir = new Vector3(InputVec.x * CameraDir.x,0.0f,InputVec.y * CameraDir.z);

        float Dir = Mathf.Atan2(InputDir.x, InputDir.z);

        Quaternion AngleDir = Quaternion.Euler(0.0f,Dir * Mathf.Rad2Deg,0.0f);

        transform.position = transform.position + (InputDir.normalized * MoveSpeed) * Time.deltaTime;

        //if(InputVec!=Vector2.zero)
        //transform.rotation = AngleDir;
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
}
