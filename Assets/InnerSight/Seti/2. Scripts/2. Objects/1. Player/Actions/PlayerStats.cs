using UnityEngine;

namespace InnerSight_Seti
{
    public class PlayerStats : Singleton<PlayerStats>
    {
        // 필드

        // 속성
        #region Properties
        public int CurrentGold { get; private set; }    // 현재 소지금
        public int RevenueGold { get; private set; }    // 당일 매상
        #endregion

        // 라이프 사이클
        #region Life Cycle
        protected override void Awake()
        {
            base.Awake();
            CurrentGold = 1000;
            RevenueGold = 0;
        }
        #endregion

        // 메서드
        #region Methods
        // 데이사이클 초기화
        public void InitializeDays()
        {
            CurrentGold += RevenueGold;
            RevenueGold = 0;
        }

        // 돈 버는 메서드
        public void EarnGold(int amount)
        {
            RevenueGold += amount;
            CurrentRevenue();
        }

        // 돈 쓰는 메서드
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