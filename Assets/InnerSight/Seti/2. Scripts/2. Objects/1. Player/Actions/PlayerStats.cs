using UnityEngine;

namespace InnerSight_Seti
{
    public class PlayerStats : Singleton<PlayerStats>
    {
        // �ʵ�

        // �Ӽ�
        #region Properties
        public int CurrentGold { get; private set; }    // ���� ������
        public int RevenueGold { get; private set; }    // ���� �Ż�
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
            CurrentRevenue();
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
                return true;
            }
        }

        public void CurrentRevenue()
        {
            Debug.Log(RevenueGold);
        }
        #endregion
    }
}