using System.Collections;
using UnityEngine;

public class BM : MonoBehaviour
{
    public float moveSpeed = 1.5f;
    public int maxHealth = 10;
    public int bossHealth;
    private bool isDead = false;
    private Rigidbody2D rigid;
    private Transform player;
    public float aggroRange = 3f;
    private bool followPlayer = false;
    private Animator animator;

    private bool isTakingDamage = false;

    // 추가된 변수: 공격 쿨타임 및 마지막으로 공격한 시간
    public float attackCooldown = 5f;
    private float lastAttackTime = 0f;

    // 추가된 변수: 공격 가능한 상태
    private bool canAttack = false;

    private bool isInvincible = false; // 추가: 무적 상태 여부를 나타내는 변수
    public float invincibilityDuration = 1.0f; // 추가: 무적 지속 시간

    public float rangedAttackCooldown = 7f;
    private float lastRangedAttackTime = 0f;

    // 추가된 변수: 원거리 공격 가능한 상태
    private bool canRangedAttack = false;

    //히트박스
    //public GameObject hitbox;
   // public Vector2 hitboxSize = new Vector2(1f, 1f);


    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        bossHealth = maxHealth;
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
                LookAtPlayer();

                float distanceToPlayer = Vector2.Distance(transform.position, player.position);
                if (distanceToPlayer <= aggroRange && canAttack && Time.time - lastAttackTime >= attackCooldown)
                {
                    StartCoroutine(Attack());
                    lastAttackTime = Time.time;
                }

                // 추가된 부분: 원거리 공격 실행
                if (canRangedAttack && Time.time - lastRangedAttackTime >= rangedAttackCooldown)
                {
                    RangedAttack();
                    lastRangedAttackTime = Time.time;
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

    void RangedAttack()
    {
        // 여기에 원거리 공격에 관련된 코드를 작성하세요.
        // 예를 들어, 총알을 생성하고 플레이어를 향해 발사하는 등의 동작을 구현할 수 있습니다.
        // 샘플 코드:
        // Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        // projectilePrefab은 보스가 발사할 원거리 공격의 프리팹입니다.
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

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
       // if (hitbox.activeSelf && CompareTag("Player"))
        //{
           // Debug.Log("플레이어 히트");

             //히트박스가 플레이어와 충돌하면 플레이어에게 데미지를 입힘
            //PH playerHealth = other.GetComponent<PH>();
            //if (playerHealth != null)
            //{
             //   playerHealth.TakeDamage(playerDamage);
            //}
       // }
  //  }

    IEnumerator Attack()
    {
        // 추가된 부분: 공격 애니메이션 재생
        //animator.SetTrigger("boss_atk");

        // 추가된 부분: 공격 가능한 상태 해제
        canAttack = false;

        // 추가된 부분: 히트박스 활성화
       // Debug.Log("Hitbox activated");
       // hitbox.SetActive(true);

        // 5초 동안 기다림
        yield return new WaitForSeconds(5f);

        // 추가된 부분: 히트박스 비활성화
        //Debug.Log("Hitbox deactivated");
       // hitbox.SetActive(false);

        // 추가된 부분: 공격 가능한 상태로 설정
        canAttack = true;
    }

   // void OnDrawGizmos()
   // {
        // 히트박스의 위치와 크기를 시각적으로 표시
      //  if (hitbox != null)
       // {
         //   Gizmos.color = Color.red;
            
            // 히트박스의 크기를 표시
       //     Gizmos.DrawWireCube(hitbox.transform.position, new Vector3(hitboxSize.x, hitboxSize.y, 0f));
       // }
  //  }


    public void TakeDamage(int damage)
{
    Debug.Log("아파요");

    if (isDead || isTakingDamage || isInvincible)
        return;

    bossHealth -= damage;

    if (bossHealth <= 0)
    {
        Die();
    }
    else
    {
        StartCoroutine(InvincibilityCooldown());
    }
}

IEnumerator InvincibilityCooldown()
{
    isInvincible = true;
    yield return new WaitForSeconds(invincibilityDuration);
    isInvincible = false;
}


     void Die()
    {
        isDead = true;
        Destroy(gameObject);
        Debug.Log("사망");
    }

}

//노찬바보멍청이 이밈