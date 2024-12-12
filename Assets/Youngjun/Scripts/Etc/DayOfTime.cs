using TMPro;
using UnityEngine;
using Noah;

// 9시 출근 ~ 23시 퇴근 (대략 4분 이상)
public class DayOfTime : MonoBehaviour
{
    public Light sun; // Directional Light
    public float dayDuration = 300f; // 하루를 300초로 설정 (5분)
    public TextMeshProUGUI timeText; // TextMeshPro로 시간 표시

    private float _timeElapsed;
    private float _timeAngle;
    private System.DateTime virtualDateTime; // 가상 시간 저장

    // 시작 시간 설정
    public int startYear = 357;
    public int startMonth = 1;
    public int startDay = 1;
    public int startHour = 9;
    public int startMinute = 0;

    // ResetTime
    public int resetTime = 11;

    public System.DateTime VirtualDateTime => virtualDateTime;

    void Start()
    {
        InitDay();
    }

    void Update()
    {
        // 경과 시간 계산
        _timeElapsed = Time.deltaTime;
        _timeAngle = Time.time;

        //_timeAngle += Time.deltaTime;
        float dayProgress = (_timeElapsed / dayDuration) % 1; // 하루 진행 비율 (0~1)
        float dayAngle = (_timeAngle / dayDuration) % 1;


        // 태양의 각도 업데이트
        float sunAngle = dayAngle * 360f; // 하루 동안 360도 회전

        Debug.Log(sunAngle);

        sun.transform.rotation = Quaternion.Euler(sunAngle + (-10f), 170f, 0f); // 태양 회전

        // 가상 시간 업데이트
        UpdateVirtualDateTime(dayProgress);

        // 시간 텍스트 업데이트
        UpdateTimeText();

        // 하루 사이클 끝 => 정산타임
        ResetDate();
    }

    void InitDay()
    {
        // 가상 시간을 초기화
        virtualDateTime = new System.DateTime(startYear, startMonth, startDay, startHour, startMinute, 0);
    }

    void UpdateVirtualDateTime(float dayProgress)
    {
        // 하루를 1440분(24시간)으로 변환, 가상 시간 진행
        float minutesPassed = dayProgress * 1380f; // 하루에서 지난 시간(분)

        virtualDateTime = virtualDateTime.AddMinutes(minutesPassed);

        _timeElapsed %= dayDuration; // 하루가 지나면 초기화
        _timeAngle %= dayDuration;
    }

    void UpdateTimeText()
    {
        if (timeText != null)
        {
            // TextMeshPro에 표시할 텍스트 설정
            timeText.text = virtualDateTime.ToString("yyyy년 MM월 dd일 HH시:mm분");
        }
    }

    // 하루 정산 기능
    void ResetDate()
    {
        if (virtualDateTime.Hour >= resetTime)
        {

            ResetManager.Instance.DailyReset();
        }
    }

    public void CheckDayTransition()
    {      
        // 다음 날로 전환
        virtualDateTime = virtualDateTime.AddDays(1).Date; // 날짜 증가, 시간 00:00으로 초기화
        virtualDateTime = virtualDateTime.AddHours(9); // 9시로 초기화

        // 태양의 각도 초기화 (9시에 해당하는 각도)
        sun.transform.rotation = Quaternion.Euler(-10f, 170f, 0f);

        // 타임 초기화
        _timeElapsed = 0f;
        _timeAngle = 0f;   
    }


}
