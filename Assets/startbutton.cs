using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class startbutton : MonoBehaviour
{
    private void Start()
    {
        // 버튼 클릭 이벤트 리스너 추가
        Button startButton = GetComponent<Button>();
        if (startButton != null)
        {
            startButton.onClick.AddListener(StartButtonClicked);
        }
    }

    private void StartButtonClicked()
    {
        // 씬 전환
        SceneManager.LoadScene("stage1");
    }
}