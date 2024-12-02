using UnityEngine;

public class DayOfTime : MonoBehaviour
{
    public Light sun; // Directional Light
    public float dayDuration = 300f; // 하루를 300초로 설정 (5분)

    private float _timeElapsed;

    void Update()
    {
        // 경과 시간을 기준으로 태양의 각도 조정
        _timeElapsed += Time.deltaTime;
        float dayProgress = (_timeElapsed / dayDuration) % 1; // 0~1 범위에서 하루 진행 비율 계산

        float sunAngle = dayProgress * 360f; // 하루 동안 360도 회전
        sun.transform.rotation = Quaternion.Euler(sunAngle - 90f, 170f, 0f); // 회전 적용
    }
}
