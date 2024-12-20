using InnerSight_Kys;
using System.Collections.Generic;

namespace InnerSight_Seti
{
    /// <summary>
    /// 선반 상인 전용 UI
    /// </summary>
    public class ShelfShopManager : ShopManager
    {
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
            int howMuch = selectItem.Value.itemCost;

            // 정산
            if (PlayerStats.Instance.SpendGold(howMuch))
            {
                AudioManager.Instance.Play("ElementSucceed"); 

                // 선반 활성화
                Item thisShelf = selectItem.Key.itemPrefab.GetComponent<Item>();
                Noah.ShelfManager.Instance.ActiveShelf(thisShelf);

                shopSlots[selectItem.Value.itemIndex].interactable = false;
                tradeCor = TradeComplete("Purchase complete");
                SwitchUI(false);
            }
            else
            {
                tradeCor = TradeComplete("You don't have enough money");
            }

            StartCoroutine(tradeCor);
        }
        #endregion

        // 기타 유틸리티
        #region Utilities
        // 버튼 선택
        protected override void SelectSlot(KeyValuePair<ItemKey, ItemValueShop> pair)
        {
            selectItem = pair;
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
        }
        #endregion
    }
}