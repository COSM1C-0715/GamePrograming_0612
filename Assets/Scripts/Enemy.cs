using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    float moveSpeed = 3;

    Rigidbody rb;

    public Collider playerCollider {  get; set; }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        var direction = playerCollider.bounds.center - rb.position;

        bool isSeenPlayer = true;
        if(Physics.Raycast(rb.position, direction.normalized, out var hitInfo))
        {
            if(hitInfo.collider != playerCollider)
            {
                isSeenPlayer = false;
            }
        }

        if (isSeenPlayer)
        {
            var subVec = playerCollider.bounds.center - rb.position;
            subVec.y = 0;
            rb.linearVelocity = subVec.normalized * moveSpeed;
        }
    }
}
