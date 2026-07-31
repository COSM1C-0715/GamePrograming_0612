using Cysharp.Threading.Tasks.Triggers;
using R3;
using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Action<float, float> OnUpdateHp;
    [SerializeField]
    float moveSpeed = 3;
    [SerializeField]
    float rotateSpeed = 3;

    ReactiveProperty<float> hp = new ReactiveProperty<float>(MaxHp);

    const float MaxHp = 3;
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
        hp.Subscribe(Currenthp =>
        {
            hp.Value = Mathf.Clamp(Currenthp, 0, MaxHp);
            OnUpdateHp(Currenthp, MaxHp);
        });
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

    public void OnHPMethod(Action<float, float> hpmethod)
    {
        OnUpdateHp = hpmethod;
    }
    void OnCollisionStay(Collision col)
    {
        var attackObj = col.gameObject.GetComponent<AttackObject>();

        if(attackObj!=null&&invincibleTime <= 0)
        {
            hp.Value -= attackObj.power;
            invincibleTime = invincibleTimeMax;
            if(hp.Value <= 0)
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
