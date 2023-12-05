using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class skill1_c : MonoBehaviour
{
    public Image Image_skill1;

    void Start()
    {
        
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Z))
        {
            StartCoroutine(CoolTime(1.0f));
        }
    }

    IEnumerator CoolTime(float cool)
    {
        print("쿨타임 코루틴");

        float startTime = Time.time;

        while (Time.time - startTime < cool)
        {
            float elapsedTime = Time.time - startTime;
            float fillRatio = elapsedTime / cool;
            Image_skill1.fillAmount = fillRatio;
            yield return null;
        }

        Image_skill1.fillAmount = 1f; // 쿨타임이 완료되면 fillAmount를 1로 설정
        print("쿨타임 코루틴 완");
    }
}
