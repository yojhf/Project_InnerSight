using UnityEngine;

namespace InnerSight_Seti
{
    public class PlayerStats : Singleton<PlayerStats>
    {
        // �Ӽ�
        #region Properties
        public int CurrentGold { get; private set; }    // ���� ������
        public int RevenueGold { get; private set; }    // ���� �Ż�
        public StatsUI StatsUI { get; private set; }    // ������ �� �Ż� ǥ��
        #endregion

        // ������ ����Ŭ
        #region Life Cycle
        protected override void Awake()
        {
            base.Awake();
            CurrentGold = 1000;
            RevenueGold = 0;
        }
        #endregion

        // �޼���
        #region Methods
        // ���̻���Ŭ �ʱ�ȭ
        public void InitializeDays()
        {
            CurrentGold += RevenueGold;
            RevenueGold = 0;
        }

        // �� ���� �޼���
        public void EarnGold(int amount)
        {
            RevenueGold += amount;
            StatsUI.SetRevenueGold();
        }

        // �� ���� �޼���
        public void SetGold(int amount)
        {
            CurrentGold = amount;
        }

        // �� ���� �޼���
        public bool SpendColl(int amount)
        {
            if (CurrentGold < amount)
            {
                return false;
            }
            else
            {
                CurrentGold -= amount;
                StatsUI.SetCurrentGold();
                return true;
            }
        }

        public void SetStatsUI(StatsUI statsUI)
        {
            StatsUI = statsUI;
            InitializeDays();
        }
        #endregion
    }
}