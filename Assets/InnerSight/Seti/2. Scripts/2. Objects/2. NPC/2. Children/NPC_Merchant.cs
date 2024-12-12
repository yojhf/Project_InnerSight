using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace InnerSight_Seti
{
    /// <summary>
    /// 상인 NPC의 부모 클래스
    /// </summary>
    /// 1. 상인에게 다가가면 UI 등장
    /// 2. UI 안내에 따라 키 입력을 하면 거래 시작 - 거래용 UI
    /// 3. 상인이 판매 중인 물품 목록, 선택한 구매 예정 물품 목록
    /// 4. 물건을 선택하고 정산 버튼을 누르면 소비 및 아이템 구매 이력 적용
    /// 5. 물약 첫 거래는 비싸게(물약값+노하우), 선반
    /// 6. 거래 끝!
    public class NPC_Merchant : NPC
    {
        // 필드
        #region Variables
        private bool OnTrade = false;
        private bool CanTrade = false;
        private const int identifier = 4000;
        private const int standardDis = 20;
        [SerializeField] private ItemDatabase itemDatabase;
        [SerializeField] private PlayerSetting player;
        [SerializeField] private ShopUI shopUI;

        private int firstElixir;
        private bool isFirstElixir = false;
        public Dictionary<ItemKey,ItemValueShop> shopDict = new();



        public float distance;
        #endregion

        // 라이프 사이클
        #region Life Cycle
        private void Start()
        {
            Initialize();
            shopUI.SetItemInfo(shopDict);
        }

        private void Update()
        {
            distance = DistanceToPlayer();
            if (distance < standardDis)
            {
                CanTrade = true;
            }
            else CanTrade = false;

            if (Input.GetKeyDown(KeyCode.K))
                Interaction();
        }
        #endregion

        // 오버라이드
        #region Override
        public override void Interaction()
        {
            if (!CanTrade) return;
            shopUI.SwitchUI(OnTrade = !OnTrade);
        }

        protected override void AIBehaviour(NPC_Behaviour npcBehaviour)
        {
            throw new System.NotImplementedException();
        }
        #endregion

        // 메서드
        #region Methods
        // 초기화 - 아이템DB로부터 엘릭서를 읽어와 도감 딕셔너리에 저장
        private void Initialize()
        {
            // 데이터베이스를 순회하되
            for (int i = 0; i < itemDatabase.itemList.Count; i++)
            {
                // itemID > 4000인 아이템, 엘릭서만 읽고
                if (itemDatabase.itemList[i].itemID - identifier > 0)
                {
                    if (!isFirstElixir)
                    {
                        isFirstElixir = true;
                        firstElixir = i;
                    }
                    // 딕셔너리에 저장
                    ItemValueShop valueShop = new()
                    {
                        itemIndex = i - firstElixir,
                        itemCost = itemDatabase.itemList[i].itemPrice / 2,
                        itemCost_Knowhow = itemDatabase.itemList[i].itemPrice * 20,
                        itemKnowhow = false
                    };
                    shopDict.Add(itemDatabase.itemList[i], valueShop);
                }
            }
        }
        #endregion

        // 이벤트 메서드
        #region Event Methods
        #endregion

        // 기타 유틸리티
        #region Utilities
        float DistanceToPlayer() => Vector3.Distance(transform.position, player.transform.position);
        #endregion
    }
}