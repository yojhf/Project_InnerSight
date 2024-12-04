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
        private TextMeshProUGUI timeCount;
        private TextMeshProUGUI currentGold;
        private TextMeshProUGUI revenueGold;
        #endregion

        // 라이프 사이클
        #region Life Cycle
        private void Start()
        {
            // 세팅
            PlayerStats.Instance.SetStatsUI(this);

            timeCount = transform.GetChild(0).Find("TimeCount").GetComponent<TextMeshProUGUI>();
            currentGold = transform.GetChild(0).Find("CurrentGold").GetComponent<TextMeshProUGUI>();
            revenueGold = transform.GetChild(0).Find("RevenueGold").GetComponent<TextMeshProUGUI>();
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
        #endregion
    }
}