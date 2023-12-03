using UnityEngine;

public class EGG : MonoBehaviour
{
    private bool isDead = false;
    public int maxHealth = 2;
    public int currentHealth;
    private bool isTakingDamage = false;
    private Animator animator;

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // 불필요한 중복 호출을 피하기 위해 이미 죽은 상태인지 확인
        if (currentHealth <= 0 && !isDead)
        {
            Die();
        }
    }

    public void TakeDamage(int damage)
{
    // 이미 죽었거나 데미지를 받고 있는 중인 경우 처리 중단
    if (isDead || isTakingDamage)
        return;

    currentHealth -= damage;

    // 체력이 0 이하인 경우 Die() 호출
    if (currentHealth <= 0)
    {
        Die();
    }
    else if (currentHealth == 1)
    {
        // currentHealth가 1이 되면 "eggmove2" 애니메이션을 재생
        animator.SetTrigger("eggmove2");
    }
}

void Die()
{
    isDead = true;
    // "eggmove3" 애니메이션을 재생
    animator.SetTrigger("eggmove3");

    // "eggmove3" 애니메이션이 끝난 후에 몬스터 파괴
    // 여기서는 1초 뒤에 DestroyObject 메서드를 호출하도록 설정
    Invoke("DestroyObject", 1f);
}

// 애니메이션 이벤트로부터 호출되는 메서드
void DestroyObject()
{
    // 오브젝트를 파괴
    Destroy(gameObject);
    isTakingDamage = false; // 데미지를 받고 있는 중이 아님을 표시
}
}
