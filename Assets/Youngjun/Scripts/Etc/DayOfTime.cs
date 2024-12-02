using UnityEngine;

public class DayOfTime : MonoBehaviour
{
    public Light sun; // Directional Light
    public float dayDuration = 300f; // �Ϸ縦 300�ʷ� ���� (5��)

    private float _timeElapsed;

    void Update()
    {
        // ��� �ð��� �������� �¾��� ���� ����
        _timeElapsed += Time.deltaTime;
        float dayProgress = (_timeElapsed / dayDuration) % 1; // 0~1 �������� �Ϸ� ���� ���� ���

        float sunAngle = dayProgress * 360f; // �Ϸ� ���� 360�� ȸ��
        sun.transform.rotation = Quaternion.Euler(sunAngle - 90f, 170f, 0f); // ȸ�� ����
    }
}
