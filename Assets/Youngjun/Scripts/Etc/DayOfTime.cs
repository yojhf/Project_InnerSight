using TMPro;
using UnityEngine;

public class DayOfTime : MonoBehaviour
{
    public Light sun; // Directional Light
    public float dayDuration = 300f; // �Ϸ縦 300�ʷ� ���� (5��)
    public TextMeshProUGUI timeText; // TextMeshPro�� �ð� ǥ��

    private float _timeElapsed;
    private float _timeAngle;
    private System.DateTime virtualDateTime; // ���� �ð� ����

    // ���� �ð� ����
    public int startYear = 357;
    public int startMonth = 1;
    public int startDay = 1;
    public int startHour = 9;
    public int startMinute = 0;

    public System.DateTime VirtualDateTime => virtualDateTime;

    void Start()
    {
        // ���� �ð��� �ʱ�ȭ
        virtualDateTime = new System.DateTime(startYear, startMonth, startDay, startHour, startMinute, 0);
    }

    void Update()
    {
        // ��� �ð� ���
        _timeElapsed = Time.deltaTime;
        _timeAngle = Time.deltaTime;

        //_timeAngle += Time.deltaTime;
        float dayProgress = (_timeElapsed / dayDuration) % 1; // �Ϸ� ���� ���� (0~1)
        float dayAngle = (_timeAngle / dayDuration) % 1;

        // �¾��� ���� ������Ʈ
        float sunAngle = dayAngle * 360f; // �Ϸ� ���� 360�� ȸ��
        sun.transform.rotation = Quaternion.Euler(sunAngle - 90f, 170f, 0f); // �¾� ȸ��

        // ���� �ð� ������Ʈ
        UpdateVirtualDateTime(dayProgress);

        // �ð� �ؽ�Ʈ ������Ʈ
        UpdateTimeText();
    }

    void UpdateVirtualDateTime(float dayProgress)
    {
        // �Ϸ縦 1440��(24�ð�)���� ��ȯ, ���� �ð� ����
        float minutesPassed = dayProgress * 1440f; // �Ϸ翡�� ���� �ð�(��)

        virtualDateTime = virtualDateTime.AddMinutes(minutesPassed);
        _timeElapsed %= dayDuration; // �Ϸ簡 ������ �ʱ�ȭ
    }

    void UpdateTimeText()
    {
        if (timeText != null)
        {
            // TextMeshPro�� ǥ���� �ؽ�Ʈ ����
            timeText.text = virtualDateTime.ToString("yyyy�� MM�� dd�� HH��:mm��");
        }
    }
}
