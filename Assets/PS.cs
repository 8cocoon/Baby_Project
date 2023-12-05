using System.Collections;
using UnityEngine;

public class PS : MonoBehaviour
{
    public float dashDistance = 5f;
    public float dashDuration = 0.5f;
    public float dashCooldown = 1.75f; // 대시 쿨다운 시간 추가
    private bool isDashing = false;
    private bool isParrying = false;
    public int dashdamage = 1;
    public int longdamage = 1;
    public Transform pos;
    public GameObject longeffect;
    public float cooltime;
    private float curtime;

    private Animator animator;
    private Rigidbody2D rigid;
    private playersound playerSound;

    void Start()
    {
        animator = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        playerSound = GetComponent<playersound>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z) && !isDashing)
        {
            StartCoroutine(Dash());
        }
        else if(Input.GetKeyDown(KeyCode.C) && !isParrying)
        {
            StartCoroutine(Parrying());
        }
        else if (curtime <= 0 && Input.GetKeyDown(KeyCode.X))
        {
            // 원거리 공격 로직
            animator.SetTrigger("long");

            if (playerSound != null)
            {
                playerSound.longSound();
            }

            // 플레이어가 왼쪽을 바라보는 경우에는 flipX를 사용하여 반전
            bool isFacingLeft = GetComponent<SpriteRenderer>().flipX;
            Vector3 spawnPosition = pos.position;
            if (isFacingLeft)
            {
                spawnPosition.x -= 1f; // 원하는 위치로 조정
            }

            Instantiate(longeffect, spawnPosition, Quaternion.identity);

            // 쿨다운 설정
            curtime = cooltime;
        }

        // 쿨다운 감소
        curtime -= Time.deltaTime;
    }

    IEnumerator Parrying()
    {
        isParrying = true;
        
        animator.SetTrigger("parrying");

        yield return null;
        
        isParrying = false;
    }

    IEnumerator Dash()
    {
        // 대시 시작
        isDashing = true;

        // 대시 애니메이션 재생
        animator.SetTrigger("dash");

        if (playerSound != null)
        {
            playerSound.dashSound();
        }

        // 대시 중에는 물리적인 충돌을 무시
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("enemyLayer"), true);

        // 플레이어가 바라보는 방향을 기준으로 대시 방향 설정
        Vector2 dashDirection = transform.right;

        // 만약 SpriteRenderer가 존재하고, 플레이어가 왼쪽을 바라보고 있다면
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null && spriteRenderer.flipX)
        {
            dashDirection = -transform.right;
        }

        float elapsedTime = 0f;

        while (elapsedTime < dashDuration)
        {
            // 대시 이동
            rigid.velocity = dashDirection * (dashDistance / dashDuration);

            // 대시 도중 충돌 여부 체크
            Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, new Vector2(1f, 1f), 0f);
            foreach (Collider2D collider in colliders)
            {
                if (collider.CompareTag("Enemy"))
                {
                    // 충돌한 대상이 적이라면 데미지를 입히기
                    EM currentHealth = collider.GetComponent<EM>();
                    if (currentHealth != null)
                    {
                        currentHealth.TakeDamage(dashdamage);
                    }
                }
            }

            // 경과 시간 업데이트
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        // 대시 지속 시간 동안 대기
        yield return new WaitForSeconds(dashDuration);

        // 대시 종료
        isDashing = false;

        // 대시 중인 동안 무시한 충돌을 다시 활성화
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("enemyLayer"), false);

        // 대시 쿨다운 동안 대기
        yield return new WaitForSeconds(dashCooldown);
    }
}
