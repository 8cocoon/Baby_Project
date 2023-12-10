using UnityEngine;

public class EGG : MonoBehaviour
{
    private bool isDead = false;
    public int maxHealth = 2;
    public int currentHealth;
    private bool isTakingDamage = false;
    private Animator animator;
    public GameObject bossObject; // 보스 오브젝트를 연결할 변수
    private eggsound eggSound;

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        eggSound = GetComponent<eggsound>();
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
    
    if (eggSound != null)
        {
            eggSound.eggbraekSound();
        }

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

        // 보스 오브젝트가 존재하고 활성화되어 있다면
        if (bossObject != null && bossObject.activeSelf)
        {
            // 보스 오브젝트의 BM 스크립트에서 Think 코루틴을 실행시킴
            BM bossScript = bossObject.GetComponent<BM>();
            if (bossScript != null)
            {
                StartCoroutine(bossScript.Think());
            }
        }
    }

private void OnDestroy()
    {
        if (bossObject != null)
        {
            bossObject.SetActive(true);
        }
    }

}
