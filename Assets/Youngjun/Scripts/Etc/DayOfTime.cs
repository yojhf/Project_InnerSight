using System.Collections;
using TMPro;
using UnityEngine;
using Noah;

// 9시 출근 ~ 23시 퇴근 (대략 4분 이상)
public class DayOfTime : MonoBehaviour
{
    private Transform player;

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
        // 경과 시간 계산
        _timeElapsed = Time.deltaTime;
        _timeAngle = Time.time;

        //_timeAngle += Time.deltaTime;
        float dayProgress = (_timeElapsed / dayDuration) % 1; // 하루 진행 비율 (0~1)
        float dayAngle = (_timeAngle / dayDuration) % 1;

        // 태양의 각도 업데이트
        float sunAngle = dayAngle * 360f; // 하루 동안 360도 회전

        sun.transform.rotation = Quaternion.Euler(sunAngle - 10f, 170f, 0f); // 태양 회전

        // 가상 시간 업데이트
        UpdateVirtualDateTime(dayProgress);
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

        player = FindAnyObjectByType<PlayerSetting>().transform;
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
            if (!isReset)
            {
                StartCoroutine(StartReset());
            }     
        }
    }

    IEnumerator StartReset()
    {
        isReset = true;

        // 리셋 순서
        // => 시간 멈춤 -> 정산 UI 켬 -> 정산 UI 끔 -> fadeout -> fadein -> 시간 정상화 -> 가상시간 리셋 -> 플레이 
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

        // 리셋 해야될 시스템
        // 날짜 업데이트
        CheckDayTransition();
        // 쓰레기 오브젝트 리스폰
        SpwanManager.Instance.SpwanCon();
        // 월세 증가
        PlayerCostManager.Instance.UpdateShopTax();
        // 플레이어 위치 초기화
        player.position = player.GetComponent<PlayerSetting>().StartPos;

        isReset = false;
    }

    void CheckDayTransition()
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
