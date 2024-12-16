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
            CurrentGold = 10000;
            RevenueGold = 0;
        }
        #endregion

        // �޼���
        #region Methods
        // �� ���� �޼���
        public void EarnGold(int amount)
        {
            RevenueGold += amount;
        }

        // ����
        public void SetGold(int amount)
        {
            CurrentGold = amount;
            RevenueGold = 0;
        }

        // �� ���� �޼���
        public bool SpendGold(int amount)
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
        #endregion
    }
}