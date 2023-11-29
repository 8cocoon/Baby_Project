using System.Collections;
using UnityEngine;

public class PS : MonoBehaviour
{
    public float dashDistance = 5f;        // 대시 거리
    public float dashDuration = 0.5f;      // 대시 지속 시간
    public float dashCooldown = 3f;        // 대시 쿨다운 시간
    private bool isDashing = false;        // 현재 대시 중인지 여부
    private bool isLong = false;
    public int dashdamage = 1;
    public Transform pos;  // longpos 변수를 추가
    public GameObject longeffect;

    private Animator animator;
    private Rigidbody2D rigid;

    void Start()
    {
        animator = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Z 키를 누르면 대시 실행
        if (Input.GetKeyDown(KeyCode.Z) && !isDashing)
        {
            StartCoroutine(Dash());
        }

        // X 키를 누르면 원거리 공격 실행
        else if (Input.GetKeyDown(KeyCode.X))
        {
            animator.SetTrigger("long");

        Instantiate(longeffect,pos.position,transform.rotation);
        }
    }

    IEnumerator Dash()
{
    // 대시 시작
    isDashing = true;

    // 대시 애니메이션 재생
    animator.SetTrigger("dash");

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