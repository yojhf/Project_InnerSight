using UnityEngine;
using TMPro;

namespace InnerSight_Seti
{
    /// <summary>
    /// ������/���� ǥ�� UI
    /// </summary>
    public class StatsUI : MonoBehaviour
    {
        // �ʵ�
        #region Variables
        // ������Ʈ
        private TextMeshProUGUI timeCount;
        private TextMeshProUGUI currentGold;
        private TextMeshProUGUI revenueGold;

        // Ŭ����
        private DayOfTime dayCycle;
        #endregion

        // ������ ����Ŭ
        #region Life Cycle
        private void Start()
        {
            // ����
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

        // �޼���
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
            timeCount.text = dayCycle.VirtualDateTime.ToString("yyyy�� MM�� dd�� HH��:mm��");
        }
        #endregion
    }
}