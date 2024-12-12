using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

namespace InnerSight_Seti
{
    /// <summary>
    /// 상인 NPC 전용 UI
    /// </summary>
    public class ShopUI : MonoBehaviour
    {
        // 필드
        #region Variables
        private GameObject shopUI;
        private TextMeshProUGUI goldText;

        private Button[] shopSlots;
        private TextMeshProUGUI[] shopCosts;
        private Dictionary<ItemKey, ItemValueShop> shopDict = new();
        #endregion

        // 라이프 사이클
        #region Life Cycle
        private void Start()
        {
            Codex_Recipe_Manager.Instance.SetCodexToShop(this);
        }

        private void Awake()
        {
            shopUI = transform.GetChild(0).GetChild(0).gameObject;
            goldText = shopUI.transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>();
            shopSlots = shopUI.transform.GetChild(1).GetComponentsInChildren<Button>();
            shopCosts = shopUI.transform.GetChild(1).GetComponentsInChildren<TextMeshProUGUI>();
        }
        #endregion

        // 메서드
        #region Methods
        // 상점 UI 스위치
        public void SwitchUI(bool isOpen)
        {
            shopUI.SetActive(isOpen);
            goldText.text = PlayerStats.Instance.CurrentGold.ToString() + " G";
        }

        // 아이템 정보 초기화
        public void SetItemInfo(Dictionary<ItemKey, ItemValueShop> shopDict)
        {
            this.shopDict = shopDict;
            AssignSlots();
            AssignCosts();
        }

        // 도감 레시피 세팅
        public void GetKnowhow(ItemKey itemKey)
        {
            shopDict[itemKey].itemKnowhow = true;
            AssignCosts();
        }
        #endregion

        // 기타 유틸리티
        #region Utilities
        // 버튼 선택
        private void SelectSlot(KeyValuePair<ItemKey, ItemValueShop> pair)
        {
            if (pair.Value.itemKnowhow)
            {
                Debug.Log(pair.Key.itemName);

                // 노하우가 알려진 아이템은 복수 거래
                // 몇 개?
                // 일괄 구매
            }
            else
            {
                Debug.Log(pair.Key.itemName);

                // 노하우가 알려지지 않은 아이템은 단일 거래
                // 한 번만 눌러도 Trade Confirm 창 띄울 것 - 비싸니까
                // 거래가 성사되면 가격 갱신
                //AssignCosts();
            }
        }

        // 각 버튼의 인덱스에 맞는 리스너를 자동으로 할당하는 메서드
        private void AssignSlots()
        {
            foreach (var pair in shopDict)
            {
                int index = pair.Value.itemIndex;  // 이 변수를 반드시 따로 선언해줘야 람다 함수 안에서 올바르게 작동

                shopSlots[index].onClick.RemoveAllListeners();
                shopSlots[index].onClick.AddListener(() => SelectSlot(pair));
            }
        }

        // 각 텍스트에 맞는 코스트를 자동으로 갱신하는 메서드
        private void AssignCosts()
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