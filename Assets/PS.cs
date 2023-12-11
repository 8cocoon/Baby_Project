using System.Collections;
using UnityEngine;

public class PS : MonoBehaviour
{
    public float dashDistance = 5f;
    public float dashDuration = 0.5f;
    public float dashCooldown = 1.75f; // 대시 쿨다운 시간 추가
    private bool isDashing = false;
    private bool isParrying = false;
    private bool isExecute = false;
    public int dashdamage = 1;
    public int dashdamage2 = 1;
    public int executedmg = 1;
    public float executeDuration = 0.5f;
    public float invincibilityDuration = 1.0f; // 추가: 무적 지속 시간
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
        else if(Input.GetKeyDown(KeyCode.V) && !isExecute)
        {
            StartCoroutine(Execute());
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

    IEnumerator Execute()
{
    // 쿨타임 무시하고 진행
    isExecute = true;

    animator.SetTrigger("execute");

    if (playerSound != null)
            {
                playerSound.executeSound();
            }

    // 대시 중에는 물리적인 충돌을 무시
    Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("enemyLayer"), true);
    Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("boss"), true);

    float elapsedTime = 0f;
    while (elapsedTime < executeDuration)
    {
        // 데미지 입히는 코드
        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, new Vector2(1f, 1f), 0f);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                EM enemyHealth = collider.GetComponent<EM>();
                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(executedmg);
                }
            }
        }

        GameObject player = GameObject.FindGameObjectWithTag("Player");
PH playerHealth = player.GetComponent<PH>();

// 예: 1의 체력 회복
playerHealth.Heal(1);

        // 경과 시간 업데이트
        elapsedTime += Time.deltaTime;

        yield return null;
    }

    // 실행이 완료되면 쿨타임과 무시한 충돌을 원래대로 복구
    isExecute = false;

    Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("enemyLayer"), false);
    Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("boss"), false);
}

    IEnumerator Parrying()
    {
        isParrying = true;
        
        animator.SetTrigger("parrying");

        if (playerSound != null)
            {
                playerSound.parryingSound();
            }

        yield return null;
        
        isParrying = false;
    }

    IEnumerator Dash()
    {

        if (isDashing)
        yield break;

        // 대시 시작
        isDashing = true;

        if(isDashing)
        {
            Debug.Log("대쉬시작");
        }

        // 대시 애니메이션 재생
        animator.SetTrigger("dash");

        if (playerSound != null)
        {
            playerSound.dashSound();
        }

        // 대시 중에는 물리적인 충돌을 무시
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("enemyLayer"), true);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("boss"), true);

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
                    EM enemyHealth = collider.GetComponent<EM>();  // 수정된 부분: EM 스크립트로 변경
                    if (enemyHealth != null)
                    {
                        enemyHealth.TakeDamage(dashdamage);  // 수정된 부분: EM의 TakeDamage 메서드 호출
                    }
                }
                else if (collider.CompareTag("boss"))
                {
                    BM bossHealth = collider.GetComponent<BM>();
                    if (bossHealth != null)
                    {
                        bossHealth.TakeDamage(dashdamage2);
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
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("boss"), false);

        // 대시 쿨다운 동안 대기
        yield return new WaitForSeconds(dashCooldown);
        }   
         }

