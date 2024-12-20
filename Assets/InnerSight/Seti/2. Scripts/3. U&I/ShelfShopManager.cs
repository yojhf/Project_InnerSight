using System.Collections.Generic;
using UnityEngine;
using InnerSight_Kys;

namespace InnerSight_Seti
{
    /// <summary>
    /// 선반 상인 전용 UI
    /// </summary>
    public class ShelfShopManager : ShopManager
    {
        [SerializeField]
        private GameObject AutoLoot;
        [SerializeField]
        private int cost_AutoLoot = 5000;
        private int thisIndex;

        // 라이프 사이클
        #region Life Cycle
        protected override void Awake()
        {
            base.Awake();

            // 컨펌
            confirmUI = shopUI.transform.GetChild(2).gameObject;
        }
        #endregion

        // 메서드
        #region Methods
        public void DefineTrade()
        {
            confirmUI.SetActive(true);
        }

        // 정산 및 컨펌 UI
        public override void Confirm()
        {
            // 가격 결정
            int howMuch;
            if (thisIndex < shopDict.Count)
                howMuch = selectItem.Value.itemCost;
            else howMuch = cost_AutoLoot;

            // 정산
            if (PlayerStats.Instance.SpendGold(howMuch))
            {
                AudioManager.Instance.Play("ElementSucceed");
                if (thisIndex < shopDict.Count)
                {
                    // 선반 활성화
                    Item thisShelf = selectItem.Key.itemPrefab.GetComponent<Item>();
                    Noah.ShelfManager.Instance.ActiveShelf(thisShelf);

                    shopSlots[selectItem.Value.itemIndex].interactable = false;
                    tradeCor = TradeComplete("Purchase complete");
                    SwitchUI(false);
                }
                else
                {
                    shopSlots[shopDict.Count].interactable = false;
                    tradeCor = TradeComplete("Purchase complete");
                    PlayerStats.Instance.OnAuto();
                    AutoLoot.SetActive(true);
                    SwitchUI(false);
                }
            }
            else
            {
                tradeCor = TradeComplete("You don't have enough money");
            }

            StartCoroutine(tradeCor);
            selectItem = default;
        }
        #endregion

        // 기타 유틸리티
        #region Utilities
        // 버튼 선택
        protected override void SelectSlot(KeyValuePair<ItemKey, ItemValueShop> pair)
        {
            selectItem = pair;
            thisIndex = pair.Value.itemIndex;
            DefineTrade();
        }

        public void SelectSlot()
        {
            thisIndex = shopDict.Count;
            DefineTrade();
        }

        // 각 텍스트에 맞는 코스트를 자동으로 갱신하는 메서드
        protected override void AssignCosts()
        {
            foreach (var pair in shopDict)
            {
                int cost = pair.Value.itemCost;
                int index = pair.Value.itemIndex;
                shopCosts[index].text = cost.ToString() + "G";
            }

            shopCosts[shopDict.Count].text = cost_AutoLoot.ToString() + "G";
        }
        #endregion
    }
}