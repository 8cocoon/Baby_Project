using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class execution : MonoBehaviour
{
    public GameObject healthIndicatorPrefab;

    // 이미 생성된 표시기 오브젝트를 저장하는 리스트
    private List<GameObject> activeIndicators = new List<GameObject>();

    void Update()
    {
        // 현재 씬에 있는 모든 Enemy 찾기
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in enemies)
        {
            // Enemy 스크립트가 부착되어 있어야 함
            EM enemyScript = enemy.GetComponent<EM>();

            // 이미 생성된 표시기가 없고, Enemy의 currentHealth가 1일 때
            if (enemyScript != null && enemyScript.currentHealth == 1 && !IsIndicatorActiveForEnemy(enemy))
            {
                // 이미지 UI를 생성하거나 활성화시킴
                ShowHealthIndicator(enemy.transform.position);
            }
        }
    }

    void ShowHealthIndicator(Vector3 position)
    {
        // healthIndicatorPrefab을 사용하여 UI 이미지 생성
        GameObject healthIndicator = Instantiate(healthIndicatorPrefab, position, Quaternion.identity);

        // 생성된 표시기를 리스트에 추가
        activeIndicators.Add(healthIndicator);

        // 필요에 따라 healthIndicator를 조작하여 UI를 설정
        // 예: healthIndicator.GetComponent<HealthIndicatorUI>().Setup();
    }

    // 이미 생성된 표시기가 있는지 확인하는 함수
    bool IsIndicatorActiveForEnemy(GameObject enemy)
    {
        foreach (GameObject indicator in activeIndicators)
        {
            // 이미 생성된 표시기의 위치가 현재 Enemy의 위치와 일치하는지 확인
            if (indicator.transform.position == enemy.transform.position)
            {
                return true;
            }
        }
        return false;
    }
}
