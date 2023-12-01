using UnityEngine;
using UnityEngine.SceneManagement;

public class StageTrigger : MonoBehaviour
{
    public string nextSceneName; // 다음으로 넘어갈 씬의 이름

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // 플레이어와 충돌했을 때만 처리
        {
            SceneManager.LoadScene(nextSceneName); // 다음 씬으로 이동
        }
    }
}