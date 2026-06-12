using UnityEngine;

public class GameCamera : MonoBehaviour
{
    InputSystem_Actions action;

    [SerializeField]
    Transform LookTarget;
    [SerializeField]
    float TargetDistance;
    [SerializeField]
    float RotateSpeed;

    void OnEnable()
    {
        action.Enable();
    }
    void OnDisable()
    {
        action.Disable();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
