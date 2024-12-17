using System.Collections.Generic;
using UnityEngine;

namespace InnerSight_Seti
{
    /// <summary>
    /// 상인 NPC의 부모 클래스
    /// </summary>
    /// 1. 상인에게 다가가면 UI 등장
    /// 2. UI 안내에 따라 키 입력을 하면 거래 시작 - 거래용 UI
    /// 3. 상인이 판매 중인 물품 목록, 선택한 구매 예정 물품 목록
    /// 4. 물건을 선택하고 정산 버튼을 누르면 소비 및 아이템 구매 이력 적용
    /// 5. 거래 끝!
    public abstract class NPC_Merchant : NPC
    {
        // 필드
        #region Variables
        // 거래 조건
        protected bool OnTrade = false;
        protected bool CanTrade = false;
        protected const int standardDis = 15;
        [SerializeField] protected PlayerSetting player;

        // 엘릭서 검색 기준
        protected int firstElixir;
        protected bool isFirstElixir = false;
        protected const int identifier = 4000;

        [SerializeField]
        protected ItemDatabase itemDatabase;
        public ShopManager shopManager;
        public Dictionary<ItemKey, ItemValueShop> shopDict = new();

        // 임시 - 플레이어와의 거리 측정
        public float distance;
        #endregion

        // 라이프 사이클
        #region Life Cycle
        protected virtual void Update()
        {
            distance = DistanceToPlayer();
            if (distance < standardDis)
            {
                CanTrade = true;
                player.Merchant = this;
            }
            else
            {
                CanTrade = false;
                player.Merchant = null;
            }
            shopManager.DistanceCheck(CanTrade);
        }
        #endregion

        // 오버라이드
        #region Override
        public override void Interaction()
        {
            if (!CanTrade) return;
            shopManager.SwitchUI(OnTrade = !OnTrade);
            shopManager.SetPlayer(player);
        }

        protected override void AIBehaviour(NPC_Behaviour npcBehaviour)
        {
            
        }

        protected abstract void Initialize();
        #endregion

        // 기타 유틸리티
        #region Utilities
        float DistanceToPlayer() => Vector3.Distance(transform.position, player.transform.position);
        #endregion
    }
}