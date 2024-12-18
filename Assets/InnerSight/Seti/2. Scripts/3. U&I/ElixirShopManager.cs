using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace InnerSight_Seti
{
    /// <summary>
    /// 상인 NPC 전용 UI
    /// </summary>
    public class ElixirShopManager : ShopManager
    {
        // 필드
        #region Variables
        private int itemCount;
        private GameObject countUI;
        private TextMeshProUGUI countText;
        #endregion

        // 라이프 사이클
        #region Life Cycle
        private void Start()
        {
            Codex_Recipe_Manager.Instance.SetCodexToShop(this);
        }

        protected override void Awake()
        {
            base.Awake();

            // 카운트
            countUI = shopUI.transform.GetChild(2).gameObject;
            countText = countUI.transform.GetChild(0).GetChild(0).GetComponentInChildren<TextMeshProUGUI>();

            // 컨펌
            confirmUI = shopUI.transform.GetChild(3).gameObject;
        }
        #endregion

        // 메서드
        #region Methods
        // 도감 레시피 세팅
        public void GetKnowhow(ItemKey itemKey)
        {
            Codex_Recipe_Manager.Instance.IdentifyRecipe(itemKey);
            shopDict[itemKey].itemKnowhow = true;
            AssignCosts();
        }

        // 아이템 카운터 ON
        private void SwitchCount(bool isOpen)
        {
            itemCount = 1;
            countUI.SetActive(isOpen);
            countText.text = itemCount.ToString();
        }

        public void CountUp()
        {
            itemCount++;
            if (itemCount > 99) itemCount -= 99;
            countText.text = itemCount.ToString();
        }

        public void CountDown()
        {
            itemCount--;
            if (itemCount < 1) itemCount += 99;
            countText.text = itemCount.ToString();
        }

        public void DefineCount()
        {
            countUI.SetActive(false);
            confirmUI.SetActive(true);
        }

        // 정산 및 컨펌 UI
        public override void Confirm()
        {
            // 가격 결정
            int howMuch;
            if (selectItem.Value.itemKnowhow)
            {
                howMuch = selectItem.Value.itemCost * itemCount;
            }
            else
            {
                howMuch = selectItem.Value.itemCost_Knowhow;
            }

            // 정산
            if (PlayerStats.Instance.SpendGold(howMuch))
            {
                if (selectItem.Value.itemKnowhow)
                {
                    player.PlayerUse.InventoryManager.AddItem(selectItem.Key, itemCount);
                }
                else
                {
                    player.PlayerUse.InventoryManager.AddItem(selectItem.Key, 1);
                    GetKnowhow(selectItem.Key);
                }
                tradeCor = TradeComplete("Purchase complete");
                SwitchUI(false);
            }
            else
            {
                tradeCor = TradeComplete("You don't have enough money");
            }

            StartCoroutine(tradeCor);
            itemCount = 0;
        }

        protected override void UIReset()
        {
            SwitchCount(false);
            base.UIReset();
        }
        #endregion

        // 기타 유틸리티
        #region Utilities
        // 버튼 선택
        protected override void SelectSlot(KeyValuePair<ItemKey, ItemValueShop> pair)
        {
            selectItem = pair;
            if (pair.Value.itemKnowhow)
                SwitchCount(true);
            else Confirm();
        }

        // 각 텍스트에 맞는 코스트를 자동으로 갱신하는 메서드
        protected override void AssignCosts()
        {
            foreach (var pair in shopDict)
            {
                int cost;
                int index = pair.Value.itemIndex;
                if (pair.Value.itemKnowhow)
                {
                    cost = pair.Value.itemCost;
                }
                else
                {
                    cost = pair.Value.itemCost_Knowhow;
                }
                shopCosts[index].text = cost.ToString() + " G";
            }
        }
        #endregion
    }
}