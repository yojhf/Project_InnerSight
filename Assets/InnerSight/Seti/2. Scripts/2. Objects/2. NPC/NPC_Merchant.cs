using System.Collections.Generic;
using Unity.VRTemplate;
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
    public class NPC_Merchant : NPC
    {
        // 필드
        #region Variables
        protected bool CanTrade = false;
        protected const int standardDis = 20;
        [SerializeField] protected PlayerSetting player;

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
        }
        #endregion

        // 오버라이드
        #region Override
        public override void Interaction() { }

        protected override void AIBehaviour(NPC_Behaviour npcBehaviour) { }
        #endregion

        // 기타 유틸리티
        #region Utilities
        float DistanceToPlayer() => Vector3.Distance(transform.position, player.transform.position);
        #endregion
    }
}