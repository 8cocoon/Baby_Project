using System.Collections;
using UnityEngine;

public class scene3bgm : MonoBehaviour
{
    public AudioSource bgm3;

    void Start()
    {
        // 씬 시작 시 배경음악 재생
        if (bgm3 != null)
        {
            bgm3.Play();
        }

        // 보스 오브젝트 찾기
        GameObject bossObject = GameObject.FindGameObjectWithTag("Boss");

        // 보스 오브젝트가 존재하고 활성화되어 있다면
        if (bossObject != null && bossObject.activeSelf)
        {
            // 빈 오브젝트 활성화
            ActivateBackgroundObject();
        }
    }

    // 보스 오브젝트의 OnDestroy 이벤트에 연결할 함수
    void BossDestroyed()
    {
        // 빈 오브젝트 비활성화
        DeactivateBackgroundObject();
    }

    // 빈 오브젝트 활성화 함수
    void ActivateBackgroundObject()
    {
        gameObject.SetActive(true);
    }

    // 빈 오브젝트 비활성화 함수
    void DeactivateBackgroundObject()
    {
        gameObject.SetActive(false);
    }
}
