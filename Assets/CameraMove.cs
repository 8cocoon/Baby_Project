using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform target; // 따라갈 대상, 여기서는 플레이어
    public Vector2 center;
    public Vector2 size;
    float height;
    float width;

    public float yOffset = 1.0f; // 카메라가 플레이어 위로 올라갈 양
    
    void Start()
    {
        height = Camera.main.orthographicSize;
        width = height * Screen.width / Screen.height;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(center, size);
    }

    void LateUpdate()
    {
        // 플레이어 위치를 따라가도록 카메라의 위치를 업데이트합니다.
        float lx = size.x * 0.5f - width;
        float clampX = Mathf.Clamp(target.position.x, -lx + center.x, lx + center.x);

        float ly = size.y * 0.5f - height;
        float clampY = Mathf.Clamp(target.position.y, -ly + center.y, ly + center.y);

        transform.position = new Vector3(clampX, clampY + yOffset, -10f);
    }    
}