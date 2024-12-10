using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace InnerSight_Seti
{
    public static class NPC_State
    {
        public static string IsEntered = "IsEntered";
        public static string Distance = "Distance";
        public static string SelfDistance = "SelfDistance";
    }

    /// <summary>
    /// NPC 상인의 부모 클래스
    /// </summary>
    /// 1. 상인에게 다가가면 UI 등장
    /// 2. UI 안내에 따라 키 입력을 하면 거래 시작 - 거래용 UI
    /// 3. 상인이 판매 중인 물품 목록, 선택한 구매 예정 물품 목록
    /// 4. 물건을 선택하고 정산 버튼을 누르면 소비 및 아이템 구매 이력 적용
    /// 5. 물약 첫 거래는 비싸게(물약값+노하우), 선반
    /// 6. 거래 끝!
    public abstract class NPC_Merchant : NPC
    {
        // 필드
        #region Variables
        #endregion

        // 라이프 사이클
        #region Life Cycle
        #endregion

        // 메서드
        #region Methods
        #endregion

        // 이벤트 메서드
        #region Event Methods
        #endregion
    }
}