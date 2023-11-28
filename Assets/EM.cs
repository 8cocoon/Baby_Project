using System.Collections;
using UnityEngine;

public class EM : MonoBehaviour
{
    public float moveSpeed = 1.5f;
    public int maxHealth = 3;
    public int currentHealth;
    private bool isDead = false;
    private Rigidbody2D rigid;
    private Transform player;
    public float aggroRange = 3f;
    private bool followPlayer = false;
    private Animator animator;

    // 추가된 변수들
    public float knockbackForce = 3f;
    public float knockbackDuration = 0.2f;
    public float recoveryDuration = 0.5f; // 밀려난 후 일정 시간 동안 제자리에 머물러 있는 시간
    private bool isTakingDamage = false;

    // 추가된 변수: 공격 쿨타임 및 마지막으로 공격한 시간
    public float attackCooldown = 5f;
    private float lastAttackTime = 0f;

    // 추가된 변수: 공격 가능한 상태
    private bool canAttack = false;

    //히트박스
    public GameObject hitbox;
    public Vector2 hitboxSize = new Vector2(1f, 1f);


    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        StartCoroutine("Think");
    }

    void FixedUpdate()
    {
        if (!isDead && !isTakingDamage)
        {
            if (followPlayer)
            {
                MoveTowardsPlayer();
                LookAtPlayer(); // 어그로가 끌릴 때만 플레이어를 바라보도록 호출

                // 추가된 부분: 플레이어와의 거리를 계산하여 공격 실행
                float distanceToPlayer = Vector2.Distance(transform.position, player.position);
                if (distanceToPlayer <= aggroRange && canAttack && Time.time - lastAttackTime >= attackCooldown)
                {
                    StartCoroutine(Attack());
                    lastAttackTime = Time.time;
                }
            }
        }
    }

    IEnumerator Think()
    {
        while (!isDead)
        {
            CheckAggroRange();
            yield return null;
        }
    }

    void CheckAggroRange()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer < aggroRange)
        {
            followPlayer = true;
            // 추가된 부분: 공격 가능한 상태로 설정
            canAttack = true;
        }
        else
        {
            // 추가된 부분: 공격 불가능한 상태로 설정
            canAttack = false;
        }
    }

    void MoveTowardsPlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        rigid.velocity = new Vector2(direction.x * moveSpeed, rigid.velocity.y);
    }

    void LookAtPlayer()
    {
        // 플레이어를 바라보도록 회전
        Vector3 direction = (player.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // flip.x 사용
        if (direction.x > 0)
            transform.localScale = new Vector3(-1, 1);
        else if (direction.x < 0)
            transform.localScale = new Vector3(1, 1);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (hitbox.activeSelf && CompareTag("Player"))
        {
            Debug.Log("플레이어 히트");

             //히트박스가 플레이어와 충돌하면 플레이어에게 데미지를 입힘
            //PH playerHealth = other.GetComponent<PH>();
            //if (playerHealth != null)
            //{
             //   playerHealth.TakeDamage(playerDamage);
            //}
        }
    }

    IEnumerator Attack()
    {
        // 추가된 부분: 공격 애니메이션 재생
        animator.SetTrigger("ant_atk");

        // 추가된 부분: 공격 가능한 상태 해제
        canAttack = false;

        // 추가된 부분: 히트박스 활성화
        Debug.Log("Hitbox activated");
        hitbox.SetActive(true);

        // 5초 동안 기다림
        yield return new WaitForSeconds(5f);

        // 추가된 부분: 히트박스 비활성화
        Debug.Log("Hitbox deactivated");
        hitbox.SetActive(false);

        // 추가된 부분: 공격 가능한 상태로 설정
        canAttack = true;
    }

    void OnDrawGizmos()
    {
        // 히트박스의 위치와 크기를 시각적으로 표시
        if (hitbox != null)
        {
            Gizmos.color = Color.red;
            
            // 히트박스의 크기를 표시
            Gizmos.DrawWireCube(hitbox.transform.position, new Vector3(hitboxSize.x, hitboxSize.y, 0f));
        }
    }


    public void TakeDamage(int damage)
{
    if (isDead || isTakingDamage)
        return;

    currentHealth -= damage;

    if (currentHealth <= 0)
    {
        Die();
    }
    else
    {
        // 플레이어에게 맞았을 때 밀려나는 효과 적용
        StartCoroutine(Knockback());
    }
}

     void Die()
    {
        isDead = true;
        Destroy(gameObject);
    }

    IEnumerator Knockback()
    {
        isTakingDamage = true;

        // 플레이어 방향에 따라 밀려나도록 설정
        Vector2 knockbackDirection = (transform.position - player.position).normalized;
        knockbackDirection.x = Mathf.Sign(knockbackDirection.x);

        // 밀려나는 힘을 적용
        rigid.velocity = new Vector2(knockbackDirection.x * knockbackForce, knockbackForce);

        // 밀려난 후 일정 시간 동안 제자리에 머물러 있는 시간
        yield return new WaitForSeconds(recoveryDuration);

        // 밀려난 후 제자리로 돌아가는 힘을 적용
        rigid.velocity = Vector2.zero;

        isTakingDamage = false;
    }
}

//노찬바보멍청이 이밈