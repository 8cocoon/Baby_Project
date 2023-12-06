using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PH : MonoBehaviour
{
    public int maxHealth = 5; // 최대 체력
    private int currentHealth; // 현재 체력

    public Image[] heartImages; // 하트 이미지 배열
    public Sprite fullHeart; // 꽉 찬 하트 스프라이트
    public Sprite emptyHeart; // 빈 하트 스프라이트

    private bool isInvincible = false; // 무적 상태 여부

    private void Start()
    {
        currentHealth = maxHealth;
        UpdateHearts();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && !isInvincible)
        {
            TakeDamage(1); // 적에게 닿을 때마다 1의 데미지
        }
        else if (collision.gameObject.CompareTag("boss") && !isInvincible)
    {
        TakeDamage(1); // 히트박스에 닿을 때마다 1의 데미지
    }
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(BlinkEffect()); // 무적 상태로 진입
        }

        UpdateHearts(); // 체력 변화마다 하트 업데이트
    }

    IEnumerator BlinkEffect()
    {
        isInvincible = true;

        // 깜빡거리는 효과를 위한 로직 추가

        yield return new WaitForSeconds(2.0f); // 무적 시간 동안 대기

        isInvincible = false; // 무적 상태 해제
    }

    private void UpdateHearts()
    {
        for (int i = 0; i < heartImages.Length; i++)
        {
            if (i < currentHealth)
            {
                heartImages[i].sprite = fullHeart; // 꽉 찬 하트
            }
            else
            {
                heartImages[i].sprite = emptyHeart; // 빈 하트
            }
        }
    }

    private void Die()
    {
        // 플레이어가 죽었을 때 실행되는 로직 추가

        Destroy(gameObject); // 플레이어 객체를 파괴하여 사라지게 함
    }
}
