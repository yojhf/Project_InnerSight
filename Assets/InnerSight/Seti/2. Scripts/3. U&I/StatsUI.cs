using UnityEngine;
using TMPro;

namespace InnerSight_Seti
{
    /// <summary>
    /// 소지금/매출 표시 UI
    /// </summary>
    public class StatsUI : MonoBehaviour
    {
        // 필드
        #region Variables
        // 컴포넌트
        private TextMeshProUGUI timeCount;
        private TextMeshProUGUI currentGold;
        private TextMeshProUGUI revenueGold;

        // 클래스
        private DayOfTime dayCycle;
        #endregion

        // 라이프 사이클
        #region Life Cycle
        private void Start()
        {
            // 세팅
            //PlayerStats.Instance.SetStatsUI(this);
            dayCycle = FindFirstObjectByType<DayOfTime>();
            timeCount = transform.GetChild(0).Find("TimeCount").GetComponent<TextMeshProUGUI>();
            currentGold = transform.GetChild(0).Find("CurrentGold").GetComponent<TextMeshProUGUI>();
            revenueGold = transform.GetChild(0).Find("RevenueGold").GetComponent<TextMeshProUGUI>();
        }

        private void Update()
        {
            //SetTime();
            SetCurrentGold();
            SetRevenueGold();
        }
        #endregion

        // 메서드
        #region Methods
        public void SetCurrentGold()
        {
            currentGold.text = PlayerStats.Instance.CurrentGold.ToString("N0");
        }

        public void SetRevenueGold()
        {
            revenueGold.text = PlayerStats.Instance.RevenueGold.ToString("N0");
        }

        private void SetTime()
        {
            timeCount.text = dayCycle.VirtualDateTime.ToString("yyyy년 MM월 dd일 HH시:mm분");
        }
        #endregion
    }
}