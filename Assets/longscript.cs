using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class longscript : MonoBehaviour
{
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("DestroyLong", 2);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(transform.right * speed * Time.deltaTime);
    }

    void DestroyLong()
    {
        Destroy(gameObject);  // 현재 스크립트가 연결된 GameObject를 파괴
    }
}
