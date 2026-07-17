using Cysharp.Threading.Tasks.Triggers;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    float moveSpeed = 3;
    [SerializeField]
    float rotateSpeed = 3;
    [SerializeField]
    int hp;
    [SerializeField]
    float invincibleTimeMax = 0.5f;
    [SerializeField]
    float knockbackSpeed = 5;

    Rigidbody rb;
    float invincibleTime;

    Animator anim;

    public Collider playerCollider {  get; set; }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(invincibleTime > 0)
        {
            invincibleTime -= Time.deltaTime;
        }
        var direction = playerCollider.bounds.center - rb.position;

        bool isSeenPlayer = true;
        if(Physics.Raycast(rb.position, direction.normalized, out var hitInfo))
        {
            if(hitInfo.collider != playerCollider)
            {
                isSeenPlayer = false;
            }
        }

        if (isSeenPlayer&&invincibleTime <= 0)
        {
            var subVec = playerCollider.bounds.center - rb.position;
            subVec.y = 0;
            rb.linearVelocity = subVec.normalized * moveSpeed;
            var rotatetarget = subVec.normalized;
            Vector3 forward = transform.forward;
            transform.forward = Vector3.Slerp(forward,rotatetarget,rotateSpeed * Time.deltaTime);
            anim.SetFloat("MoveSpeed",subVec.magnitude);
        }
    }
    void OnCollisionStay(Collision col)
    {
        var attackObj = col.gameObject.GetComponent<AttackObject>();

        if(attackObj!=null&&invincibleTime <= 0)
        {
            hp -= attackObj.power;
            invincibleTime = invincibleTimeMax;
            if(hp <= 0)
            {
                Destroy(gameObject);
            }

            var dir = transform.position - col.transform.position;
            dir.y = 0;
            var knockbackVec = dir.normalized * knockbackSpeed;
            rb.linearVelocity = knockbackVec;
        }
    }
}
