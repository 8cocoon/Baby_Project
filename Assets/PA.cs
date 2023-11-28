using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PA : MonoBehaviour
{
    public int maxHealth = 5; // 최대 체력
    private int currentHealth; // 현재 체력

    public Transform heartsContainer; // 하트를 담고 있는 컨테이너
    public Sprite fullHeart;
    public Sprite emptyHeart;

    private int enemyHits = 0;
    public int maxHits = 3; // 적이 사라지기 전에 공격해야 하는 횟수

    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void UpdateHearts()
    {
        for (int i = 0; i < heartsContainer.childCount; i++)
        {
            Transform heart = heartsContainer.GetChild(i);
            heart.GetComponent<SpriteRenderer>().sprite = i < currentHealth ? fullHeart : emptyHeart;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("enemyLayer"))
        {
            TakeDamageFromEnemy(other.gameObject);
        }
    }

    private void TakeDamageFromEnemy(GameObject enemy)
    {
        enemyHits++;
        Debug.Log("적 공격!");

        if (enemyHits >= maxHits)
        {
            Destroy(enemy); // 적 제거
            enemyHits = 0; // 다시 초기화
        }
    }

    private void Die()
    {
        // 플레이어가 죽었을 때 실행되는 로직 추가
        Destroy(gameObject); // 플레이어 객체를 파괴하여 사라지게 함
    }
}
