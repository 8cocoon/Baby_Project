using UnityEngine;
using UnityEngine.UI;

public class optionbutton : MonoBehaviour
{
    public GameObject opwindow;
    private void Start()
    {
        // 처음에는 설정 창 비활성화
        if (opwindow != null)
        {
            opwindow.SetActive(false);
        }
    }

    public void OpenSettingsPanel()
    {
        Debug.Log("OpenSettingsPanel() called"); // 디버그 출력 추가
        // 설정 창을 활성화
        if (opwindow != null)
        {
            opwindow.SetActive(true);
        }
    }
}
