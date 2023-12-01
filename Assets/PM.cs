using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PM : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public bool isGrounded;
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    public int attackDamage = 1; // 근접 공격 데미지

    public Transform frontHitbox; // 전방 히트박스 위치
    public Transform backHitbox;  // 뒷방 히트박스 위치
    public Vector2 boxsize;      // 히트박스 크기
    public float knockbackForce = 5f; // 넉백 힘
    public float invincibleDuration = 2f; // 무적 시간
    private bool isInvincible = false;
    private Rigidbody2D playerRB;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
          playerRB = GetComponent<Rigidbody2D>();
          
    }

    private void Update()
    {
        // 플레이어 움직임
        float moveX = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveX * moveSpeed, rb.velocity.y);

        // 이동 애니메이션 제어
        if (Mathf.Abs(moveX) > 0)
        {
            animator.SetBool("run", true);
            if (moveX > 0)
            {
                spriteRenderer.flipX = false;
            }
            else if (moveX < 0)
            {
                spriteRenderer.flipX = true;
            }
        }
        else
        {
            animator.SetBool("run", false);
        }

        // 점프
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            isGrounded = false;
            animator.SetTrigger("jump");
        }

        // 공격
        if (Input.GetButtonDown("Fire1"))
        {
            // 공격 애니메이션 재생
            animator.SetTrigger("atk");

            // 근접 공격 로직을 추가
            PerformMeleeAttack();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 땅에 닿았는지 확인
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    
        else if (collision.gameObject.CompareTag("Enemy") && !isInvincible)
        {
            Vector2 knockbackDirection = (transform.position - collision.transform.position).normalized;
            knockbackDirection.x = Mathf.Sign(knockbackDirection.x);
            knockbackDirection.y = 1;

            StartCoroutine(BlinkEffect()); // BlinkEffect 코루틴 시작
            playerRB.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
        }
    
    }

      IEnumerator BlinkEffect()
    {
        float blinkInterval = 0.3f; // 깜빡거리는 간격
        float blinkDuration = 2.0f; // 깜빡이는 총 시간
        float elapsedTime = 0f;

        while (elapsedTime < blinkDuration)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled; // 깜빡거리는 효과
            elapsedTime += blinkInterval;

            yield return new WaitForSeconds(blinkInterval);
        }

        spriteRenderer.enabled = true; // 깜빡임 종료 후 다시 보이도록 설정
        isInvincible = false; // 깜빡임이 끝날 때 무적 상태 해제
    }

    private void PerformMeleeAttack()
    {
        // 현재 플레이어의 방향
        int playerDirection = spriteRenderer.flipX ? -1 : 1;

        // 히트박스 위치를 업데이트
        Vector3 frontHitboxPosition = frontHitbox.position;
        Vector3 backHitboxPosition = backHitbox.position;

        // 근접 공격 범위 내의 모든 적 캐릭터를 검색 (전방 히트박스)
        Collider2D[] frontColliders = Physics2D.OverlapBoxAll(frontHitboxPosition, boxsize, 0);
        
        // 각 적에게 데미지를 입히고 처리 (전방 히트박스)
       foreach (Collider2D collider in frontColliders)
{
    Debug.Log("히트(전방)");

    // EM 스크립트를 찾아봅니다.
    EM enemy = collider.GetComponent<EM>();

    // EGG 스크립트를 찾아봅니다.
    EGG egg = collider.GetComponent<EGG>();

    // EM 스크립트를 가진 오브젝트에게 데미지를 줍니다.
    if (enemy != null)
    {
        Debug.Log(enemy.currentHealth);
        enemy.TakeDamage(attackDamage);
    }

    // EGG 스크립트를 가진 오브젝트에게 데미지를 줍니다.
    if (egg != null)
    {
        egg.TakeDamage(attackDamage);
    }
}

        // 근접 공격 범위 내의 모든 적 캐릭터를 검색 (뒷방 히트박스)
        Collider2D[] backColliders = Physics2D.OverlapBoxAll(backHitboxPosition, boxsize, 0);
        
        // 각 적에게 데미지를 입히고 처리 (뒷방 히트박스)
        foreach (Collider2D collider in backColliders)
{
    Debug.Log("히트(전방)");

    // EM 스크립트를 찾아봅니다.
    EM enemy = collider.GetComponent<EM>();

    // EGG 스크립트를 찾아봅니다.
    EGG egg = collider.GetComponent<EGG>();

    // EM 스크립트를 가진 오브젝트에게 데미지를 줍니다.
    if (enemy != null)
    {
        Debug.Log(enemy.currentHealth);
        enemy.TakeDamage(attackDamage);
    }

    // EGG 스크립트를 가진 오브젝트에게 데미지를 줍니다.
    if (egg != null)
    {
        egg.TakeDamage(attackDamage);
    }
}
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(frontHitbox.position, boxsize);
        Gizmos.DrawWireCube(backHitbox.position, boxsize);
    }
}


