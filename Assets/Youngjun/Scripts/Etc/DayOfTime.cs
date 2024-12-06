using InnerSight_Seti;
using MyFPS;
using System.Collections;
using TMPro;
using UnityEngine;

// 9�� ��� ~ 23�� ��� (�뷫 4�� �̻�)
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

    // ResetTime
    public int resetTime = 11;
    private float timeScale = 0;


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
        _timeAngle = Time.time;

        //_timeAngle += Time.deltaTime;
        float dayProgress = (_timeElapsed / dayDuration) % 1; // �Ϸ� ���� ���� (0~1)
        float dayAngle = (_timeAngle / dayDuration) % 1;

        // �¾��� ���� ������Ʈ
        float sunAngle = dayAngle * 360f; // �Ϸ� ���� 360�� ȸ��

        sun.transform.rotation = Quaternion.Euler(sunAngle - 10f, 170f, 0f); // �¾� ȸ��

        // ���� �ð� ������Ʈ
        UpdateVirtualDateTime(dayProgress);

        // �ð� �ؽ�Ʈ ������Ʈ
        UpdateTimeText();

        // �Ϸ� ����Ŭ �� => ����Ÿ��
        //ResetDate();
    }

    void UpdateVirtualDateTime(float dayProgress)
    {
        // �Ϸ縦 1440��(24�ð�)���� ��ȯ, ���� �ð� ����
        float minutesPassed = dayProgress * 1440f; // �Ϸ翡�� ���� �ð�(��)

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
        if (virtualDateTime.Hour == resetTime)
        {
            StartCoroutine(StartReset());
        }
    }

    IEnumerator StartReset()
    {
        // ���� ����
        // => �ð� ���� -> ���� UI �� -> ���� UI �� -> fadeout -> fadein -> �ð� ����ȭ -> ����ð� ���� -> �÷��� 
        Pause();




        SceneFade.instance.FadeOut(null);


        yield return null;  
    }

    public void Pause()
    {
        Time.timeScale = timeScale;

        Debug.Log(Time.timeScale);
    }

    public void ResetPause()
    {
        //isPause = false;
        //pauseUI.gameObject.SetActive(false);
        // �Ͻ����� ����
        Time.timeScale = 1.0f;
        //Cursor.lockState = CursorLockMode.Locked;
        //player.GetComponent<FirstPersonController>().enabled = true;
    }
}
