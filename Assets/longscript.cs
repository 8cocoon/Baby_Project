using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class longscript : MonoBehaviour
{
    public float speed;
    public int damageAmount = 1;
    private Vector3 initialDirection;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("DestroyLong", 2);
        SetInitialDirection();
    }

    void Update()
    {
        MoveLong();
    }

    void SetInitialDirection()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            SpriteRenderer playerSpriteRenderer = player.GetComponent<SpriteRenderer>();

            if (playerSpriteRenderer != null && playerSpriteRenderer.flipX)
            {
                initialDirection = Vector3.left;
                FlipLong();
            }
            else
            {
                initialDirection = Vector3.right;
            }
        }
    }

    void MoveLong()
    {
        // 초기 방향으로 이동
        transform.Translate(initialDirection * speed * Time.deltaTime);
    }

    void FlipLong()
    {
        // 스프라이트 렌더러가 있을 경우 flipX 값을 변경
        SpriteRenderer longSpriteRenderer = GetComponent<SpriteRenderer>();
        if (longSpriteRenderer != null)
        {
            longSpriteRenderer.flipX = true;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            EM currentHealth = collision.gameObject.GetComponent<EM>();
            if (currentHealth != null)
            {
                currentHealth.TakeDamage(damageAmount);
            }

            // 발사체가 적과 충돌하면 발사체 파괴
            Destroy(gameObject);
        }
    }
  
    void DestroyLong()
    {
        Destroy(gameObject);  // 현재 스크립트가 연결된 GameObject를 파괴
    }
}
