using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bosslong : MonoBehaviour
{
    public int bossdamage = 1;

    void Start()
    {
        Invoke("Destroybosslong", 1);
    }

    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PH currentHealth = collision.gameObject.GetComponent<PH>();
            if (currentHealth != null)
            {
                currentHealth.TakeDamage(bossdamage);
            }

            // 충돌한 후에 bosslong 오브젝트를 파괴
            Destroybosslong();
        }
    }

    void Destroybosslong()
    {
        Destroy(gameObject);
    }
}
