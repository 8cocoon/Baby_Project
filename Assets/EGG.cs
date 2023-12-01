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
        if (currentHealth <= 0 && !isDead)
        {
            Die();
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead || isTakingDamage)
            return;

        currentHealth -= damage;

        if (currentHealth == 1)
        {
            // currentHealth가 1이 되면 "eggmove2" 애니메이션을 재생
            animator.SetTrigger("eggmove2");
        }
    }

    void Die()
    {
        isDead = true;
        // "eggmove3" 애니메이션을 재생하고 재생이 끝나면 오브젝트를 파괴
        animator.SetTrigger("eggmove3");
    }

    // 애니메이션 이벤트로부터 호출되는 메서드
    public void DestroyObject()
    {
        Destroy(gameObject);
    }
}
