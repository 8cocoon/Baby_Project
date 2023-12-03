using UnityEngine;

public class optionbutton : MonoBehaviour
{
    public GameObject opwindow; // Inspector에서 설정 창 UI GameObject를 연결

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
        // 설정 창을 활성화
        if (opwindow != null)
        {
            opwindow.SetActive(true);
        }
    }
}
