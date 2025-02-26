using System.Collections;
using System.Threading;
using UnityEngine;

public enum PlayerState
{
    IDLE,
    MOVE,
    ATTACK
}

public class PlayerMove : MonoBehaviour
{
    Rigidbody2D rb;
    Animator anim;

    Vector2 dir;
    public float speed = 5;

    PlayerState state;

    // °ø°Ý µô·¹ÀÌ
    [Range(0.5f, 2.0f)] public float attackDelay = 0.5f;
    // ÄÞº¸ °¡´É½Ã°£
    public float comboCheckTime = 0.3f;
    float comboTimer = 0;
    int comboCount = 0;
    float attackTimer = 0;
    bool isAttack = false;
    bool isCombo = false;

    private void Awake()
    {
        rb = transform.GetComponent<Rigidbody2D>();
        anim = transform.GetComponent<Animator>();
    }

    void Update()
    {
        Attack();
        Move();
    }

    void Move()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        if(horizontal == 0 && vertical == 0)
        {
            rb.linearVelocity = Vector2.zero;
            ChanageState(PlayerState.IDLE);
            return;
        }

        ChanageState(PlayerState.MOVE);
        dir = new Vector2(horizontal, vertical);

        anim.SetFloat("MoveX", dir.x);
        anim.SetFloat("MoveY", dir.y);

        if (isAttack) return;
        rb.linearVelocity = dir * speed * Time.fixedDeltaTime;
    }

    void ChanageState(PlayerState _state)
    {
        if (state == _state) return;
        if (isAttack) return;

        state = _state;
        switch (_state)
        {
            case PlayerState.IDLE:
                {
                    anim.SetBool("isRun", false);
                }
                break;
            case PlayerState.MOVE:
                {
                    anim.SetBool("isRun", true);
                }
                break;
            case PlayerState.ATTACK:
                {
                    anim.SetTrigger("Attack");
                    rb.linearVelocity = Vector2.zero;
                    isAttack = true;
                }
                break;
        }
    }

    void Attack()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            ChanageState(PlayerState.ATTACK);

            // ÄÞº¸ Å¸ÀÌ¹Ö = ÄÞº¸Å¸ÀÌ¸ÓÀÇ 30% ~ ÄÞº¸Å¸ÀÌ¸Ó || ÄÞº¸ È½¼ö = 0,1,2
            if(comboTimer < comboCheckTime && comboTimer > (comboCheckTime * 0.3f) && comboCount < 1)
            {
                isCombo = true;
                comboCount++;
                anim.SetInteger("comboCount", comboCount);
            }
        }

        if(isAttack)
        {
            // °ø°ÝÁ¾·á
            if(attackTimer >= attackDelay)
            {
                if(isCombo)
                {
                    comboTimer = 0;
                    attackTimer = 0;
                    isCombo = false;
                }
                else
                {
                    AttackReset();
                }
            }
            else
            {
                attackTimer += Time.deltaTime;
                comboTimer += Time.deltaTime;
            }
        }
    }

    void AttackReset()
    {
        attackTimer = 0;
        comboTimer = 0;
        comboCount = 0;
        isAttack = false;
        anim.SetInteger("comboCount", comboCount);
        anim.ResetTrigger("Attack");
    }
}
