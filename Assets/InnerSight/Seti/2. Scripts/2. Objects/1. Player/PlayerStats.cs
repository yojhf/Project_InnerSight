using InnerSight_Kys;
using UnityEngine;

namespace InnerSight_Seti
{
    public class PlayerStats : Singleton<PlayerStats>
    {
        // 속성
        #region Properties
        [SerializeField]
        private int initialGold = 1000;
        public int CurrentGold { get; private set; }    // 현재 소지금
        public int RevenueGold { get; private set; }    // 당일 매상
        public StatsUI StatsUI { get; private set; }    // 소지금 및 매상 표시
        public bool OnAutoLoot { get; private set; }    // 일괄 줍기 활성화 여부
        #endregion

        // 라이프 사이클
        #region Life Cycle
        protected override void Awake()
        {
            base.Awake();
            CurrentGold = initialGold;
            RevenueGold = 0;
        }
        #endregion

        // 메서드
        #region Methods
        // 돈 버는 메서드
        public void EarnGold(int amount)
        {
            AudioManager.Instance.Play("coin_6");
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

        public void OnAuto() => OnAutoLoot = true;
        #endregion
    }
}