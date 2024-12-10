using System.Collections;
using TMPro;
using UnityEngine;
using Noah;

// 9�� ��� ~ 23�� ��� (�뷫 4�� �̻�)
public class DayOfTime : MonoBehaviour
{
    private Transform player;

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

    // ResetTime
    public int resetTime = 11;
    private float timeScale = 0;
    private bool isReset = false;

    // Instance
    [SerializeField] private InGameUI_DayCycle inGameUI_DayCycle;

    public System.DateTime VirtualDateTime => virtualDateTime;
    public bool IsReset => isReset;

    void Start()
    {
        InitDay();
    }

    void Update()
    {
        // ��� �ð� ���
        _timeElapsed = Time.deltaTime;
        _timeAngle = Time.time;

        //_timeAngle += Time.deltaTime;
        float dayProgress = (_timeElapsed / dayDuration) % 1; // �Ϸ� ���� ���� (0~1)
        float dayAngle = (_timeAngle / dayDuration) % 1;

        // �¾��� ���� ������Ʈ
        float sunAngle = dayAngle * 360f; // �Ϸ� ���� 360�� ȸ��

        sun.transform.rotation = Quaternion.Euler(sunAngle - 10f, 170f, 0f); // �¾� ȸ��

        // ���� �ð� ������Ʈ
        UpdateVirtualDateTime(dayProgress);
        UpdateVirtualDateTime(dayProgress);

        // �ð� �ؽ�Ʈ ������Ʈ
        UpdateTimeText();

        // �Ϸ� ����Ŭ �� => ����Ÿ��
        ResetDate();
    }

    void InitDay()
    {
        // ���� �ð��� �ʱ�ȭ
        virtualDateTime = new System.DateTime(startYear, startMonth, startDay, startHour, startMinute, 0);

        player = FindAnyObjectByType<PlayerSetting>().transform;
    }

    void UpdateVirtualDateTime(float dayProgress)
    {
        // �Ϸ縦 1440��(24�ð�)���� ��ȯ, ���� �ð� ����
        float minutesPassed = dayProgress * 1380f; // �Ϸ翡�� ���� �ð�(��)

        virtualDateTime = virtualDateTime.AddMinutes(minutesPassed);

        _timeElapsed %= dayDuration; // �Ϸ簡 ������ �ʱ�ȭ
        _timeAngle %= dayDuration;
    }

    void UpdateTimeText()
    {
        if (timeText != null)
        {
            // TextMeshPro�� ǥ���� �ؽ�Ʈ ����
            timeText.text = virtualDateTime.ToString("yyyy�� MM�� dd�� HH��:mm��");
        }
    }

    // �Ϸ� ���� ���
    void ResetDate()
    {
        if (virtualDateTime.Hour >= resetTime)
        {
            if (!isReset)
            {
                StartCoroutine(StartReset());
            }     
        }
    }

    IEnumerator StartReset()
    {
        isReset = true;

        // ���� ����
        // => �ð� ���� -> ���� UI �� -> ���� UI �� -> fadeout -> fadein -> �ð� ����ȭ -> ����ð� ���� -> �÷��� 
        Pause();

        inGameUI_DayCycle.DayResetUI();

        yield return new WaitForSecondsRealtime(5f);

        SceneFade.instance.FadeOut(null);

        yield return new WaitForSecondsRealtime(1f);

        SceneFade.instance.FadeIn(null);

        ResetPause();
    }

    public void Pause()
    {
        Time.timeScale = timeScale;
    }

    public void ResetPause()
    {
        Time.timeScale = 1.0f;

        // ���� �ؾߵ� �ý���
        // ��¥ ������Ʈ
        CheckDayTransition();
        // ������ ������Ʈ ������
        SpwanManager.Instance.SpwanCon();
        // ���� ����
        PlayerCostManager.Instance.UpdateShopTax();
        // �÷��̾� ��ġ �ʱ�ȭ
        player.position = player.GetComponent<PlayerSetting>().StartPos;

        isReset = false;
    }

    void CheckDayTransition()
    {      
        // ���� ���� ��ȯ
        virtualDateTime = virtualDateTime.AddDays(1).Date; // ��¥ ����, �ð� 00:00���� �ʱ�ȭ
        virtualDateTime = virtualDateTime.AddHours(9); // 9�÷� �ʱ�ȭ

        // �¾��� ���� �ʱ�ȭ (9�ÿ� �ش��ϴ� ����)
        sun.transform.rotation = Quaternion.Euler(-10f, 170f, 0f);

        // Ÿ�� �ʱ�ȭ
        _timeElapsed = 0f;
        _timeAngle = 0f;   
    }


}
