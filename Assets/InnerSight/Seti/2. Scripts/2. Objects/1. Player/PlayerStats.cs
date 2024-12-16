using UnityEngine;

namespace InnerSight_Seti
{
    public class PlayerStats : Singleton<PlayerStats>
    {
        // 속성
        #region Properties
        public int CurrentGold { get; private set; }    // 현재 소지금
        public int RevenueGold { get; private set; }    // 당일 매상
        public StatsUI StatsUI { get; private set; }    // 소지금 및 매상 표시
        #endregion

        // 라이프 사이클
        #region Life Cycle
        protected override void Awake()
        {
            base.Awake();
            CurrentGold = 10000;
            RevenueGold = 0;
        }
        #endregion

        // 메서드
        #region Methods
        // 돈 버는 메서드
        public void EarnGold(int amount)
        {
            RevenueGold += amount;
        }

        // 정산
        public void SetGold(int amount)
        {
            CurrentGold = amount;
            RevenueGold = 0;
        }

        // 돈 쓰는 메서드
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