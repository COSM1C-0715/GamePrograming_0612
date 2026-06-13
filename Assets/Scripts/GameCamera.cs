using UnityEngine;
using UnityEngine.InputSystem;

public class GameCamera : MonoBehaviour
{
    InputSystem_Actions action;

    [SerializeField]
    Vector2 LookVec;

    [SerializeField]
    Transform LookTarget;

    [SerializeField]
    Vector3 OffSet;
    [SerializeField]
    float TargetDistance;
    [SerializeField]
    float RotateSpeed;

    float pitch;
    float yaw;

    void OnEnable()
    {
        action.Enable();
        action.Player.Look.performed += OnLookCamera;
    }
    void OnDisable()
    {
        action.Player.Look.performed -= OnLookCamera;
        action.Disable();
    }

    void Awake()
    {
        action = new InputSystem_Actions();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pitch = 0;
        yaw = 90;
    }

    // Update is called once per frame
    void Update()
    {
        yaw += LookVec.x * RotateSpeed * Time.deltaTime;

        pitch -= LookVec.y * RotateSpeed * Time.deltaTime;

        var target = LookTarget.position + OffSet;

        var rotation = Quaternion.Euler(pitch,yaw,0.0f);

        var position = rotation * new Vector3 (0,0,-TargetDistance) + target;

        transform.rotation = rotation;
        transform.position = position;
    }
    void OnLookCamera(InputAction.CallbackContext cont)
    {
        if (cont.performed)
        {
            LookVec = cont.ReadValue<Vector2>();
        }
    }
}
