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

    // 공격 딜레이
    [Range(0.5f, 2.0f)] public float attackDelay = 0.5f;
    // 콤보 가능시간
    public float comboCheckTime = 0.3f;
    float comboTimer = 0;
    int comboCount = 0;
    float attackTimer = 0;
    bool isAttack = false;
    bool isCombo = false;

    [Header("Effect Manager Script")]
    [SerializeField] EffectManager effectManager;

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

        // 공격시 정지
        if (isAttack) return;

        anim.SetFloat("MoveX", dir.x);
        anim.SetFloat("MoveY", dir.y);

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
                    effectManager.Effect_ON(Effect.Melee);
                }
                break;
        }
    }

    void Attack()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            ChanageState(PlayerState.ATTACK);

            // 콤보 타이밍 = 콤보타이머의 30% ~ 콤보타이머 || 콤보 횟수 = 0,1
            if(comboTimer < comboCheckTime && comboTimer > (comboCheckTime * 0.3f) && comboCount < 1)
            {
                isCombo = true;
                comboCount++;
                anim.SetInteger("comboCount", comboCount);
            }
        }

        if(isAttack)
        {
            // 공격종료
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
