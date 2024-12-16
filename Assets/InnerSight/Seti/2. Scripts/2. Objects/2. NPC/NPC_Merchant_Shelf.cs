using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEngine;

namespace InnerSight_Seti
{
    /// <summary>
    /// 선반 판매 NPC
    /// </summary>
    /// 판매된 거 비활성화
    /// 총 6개
    /// 가격 모두 다르고 8000 / 20000
    /// 구매했으면 해당 선반 활성화
    /// 
    /// UI
    /// 선반 6개
    /// 어떤 엘릭서? 가격?
    /// 구매하시겠습니까?
    /// 구매 완료
    public class NPC_Merchant_Shelf : NPC_Merchant
    {
        // 필드
        #region Variables
        private int firstElixir;
        private bool isFirstElixir = false;
        private const int identifier = 4000;

        [SerializeField]
        private ItemDatabase itemDatabase;
        public ShelfShopManager shopManager;
        public Dictionary<ItemKey, ItemValueShop> shopDict = new();
        #endregion

        // 라이프 사이클
        #region Life Cycle
        private void Start()
        {
            Initialize();
            shopManager.SetItemInfo(shopDict);
        }
        #endregion

        // 메서드
        #region Methods
        public override void Interaction()
        {
            base.Interaction();
            if (!CanTrade) return;
            shopManager.SwitchUI(OnTrade = !OnTrade);
            shopManager.SetPlayer(player);
        }

        protected override void AIBehaviour(NPC_Behaviour npcBehaviour)
        {
            base.AIBehaviour(npcBehaviour);
        }

        void Initialize()
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
                    if (i - firstElixir < 2) continue;
                    int thirdElixir = firstElixir + 2;

                    // 딕셔너리에 저장
                    ItemValueShop valueShop = new()
                    {
                        itemIndex = i - thirdElixir,
                        itemCost = itemDatabase.itemList[i].itemPrice * 80
                    };
                    shopDict.Add(itemDatabase.itemList[i], valueShop);
                }
            }
        }
        #endregion
    }
}