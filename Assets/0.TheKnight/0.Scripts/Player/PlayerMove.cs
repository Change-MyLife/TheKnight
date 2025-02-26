using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    Rigidbody2D rb;
    Animator anim;

    Vector2 dir;
    public float speed = 5;

    private void Awake()
    {
        rb = transform.GetComponent<Rigidbody2D>();
        anim = transform.GetComponent<Animator>();
    }

    void Update()
    {
        Move();
        SetAnim();
    }

    void Move()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        if(horizontal == 0 && vertical == 0)
        {
            rb.linearVelocity = Vector2.zero;
            anim.SetBool("isRun", false);
            return;
        }

        anim.SetBool("isRun", true);
        dir = new Vector2(horizontal, vertical);
        rb.linearVelocity = dir * speed * Time.fixedDeltaTime;
    }

    void SetAnim()
    {
        anim.SetFloat("MoveX", dir.x);
        anim.SetFloat("MoveY", dir.y);
    }
}
