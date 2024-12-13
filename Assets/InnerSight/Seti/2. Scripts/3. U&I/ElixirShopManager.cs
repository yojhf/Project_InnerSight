using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace InnerSight_Seti
{
    /// <summary>
    /// 상인 NPC 전용 UI
    /// </summary>
    public class ElixirShopManager : MonoBehaviour
    {
        // 필드
        #region Variables
        private int itemCount;
        private GameObject shopUI;
        private GameObject countUI; //
        private GameObject confirmUI;   //
        private GameObject completeUI;   //
        private TextMeshProUGUI goldText;
        private TextMeshProUGUI countText;  //
        private TextMeshProUGUI completeText;  //
        private IEnumerator tradeCor;

        private Button[] shopSlots;
        private TextMeshProUGUI[] shopCosts;
        private Dictionary<ItemKey, ItemValueShop> shopDict = new();
        private KeyValuePair<ItemKey, ItemValueShop> selectItem;

        // 클래스
        private PlayerSetting player;
        #endregion

        // 속성
        #region Properties
        public bool OnTrade { get; protected set; }
        #endregion

        // 라이프 사이클
        #region Life Cycle
        private void Start()
        {
            Codex_Recipe_Manager.Instance.SetCodexToShop(this);
        }

        private void Awake()
        {
            // UI 묶음
            shopUI = transform.GetChild(0).GetChild(0).gameObject;
            
            // 골드
            goldText = shopUI.transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>();
            
            // 선택 배열
            shopSlots = shopUI.transform.GetChild(1).GetComponentsInChildren<Button>();
            shopCosts = shopUI.transform.GetChild(1).GetComponentsInChildren<TextMeshProUGUI>();

            // 카운트
            countUI = shopUI.transform.GetChild(2).gameObject;
            countText = countUI.transform.GetChild(0).GetChild(0).GetComponentInChildren<TextMeshProUGUI>();

            // 컨펌
            confirmUI = shopUI.transform.GetChild(3).gameObject;

            // 완료
            completeUI = transform.GetChild(0).GetChild(1).gameObject;
            completeText = completeUI.GetComponentInChildren<TextMeshProUGUI>();
        }
        #endregion

        // 메서드
        #region Methods
        // 도감 레시피 세팅
        public void GetKnowhow(ItemKey itemKey)
        {
            shopDict[itemKey].itemKnowhow = true;
            AssignCosts();
        }

        // 상점 UI 스위치
        public void SwitchUI(bool isOpen)
        {
            OnTrade = isOpen;
            shopUI.SetActive(isOpen);
            goldText.text = PlayerStats.Instance.CurrentGold.ToString() + "G";
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

        public void Confirm_Yes()
        {
            Confirm();
        }
        public void Confirm_No()
        {
            confirmUI.SetActive(false);
        }

        // 정산 및 컨펌 UI
        public void Confirm()
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
                    player.PlayerUse.inventoryManager.AddItem(selectItem.Key, itemCount);
                }
                else
                {
                    player.PlayerUse.inventoryManager.AddItem(selectItem.Key, 1);
                    GetKnowhow(selectItem.Key);
                }
                tradeCor = TradeComplete("Purchase complete");
                SwitchUI(false);
            }
            else
            {
                tradeCor = TradeComplete("You don't have enough money");
                confirmUI.SetActive(false);
            }

            StartCoroutine(tradeCor);
            itemCount = 0;
        }
        #endregion

        // 기타 유틸리티
        #region Utilities
        // 컨펌 텍스트
        IEnumerator TradeComplete(string text)
        {
            completeUI.SetActive(true);
            completeText.text = text;
            yield return new WaitForSeconds(2);

            completeUI.SetActive(false);
            tradeCor = null;
            yield break;
        }

        // 거래 성사 확인 버튼
        /*public void ConfirmComplete()
        {
            if (tradeCor != null)
            {
                StopCoroutine(tradeCor);
                tradeCor = null;
            }
            completeUI.SetActive(false);
        }*/

        // 버튼 선택
        private void SelectSlot(KeyValuePair<ItemKey, ItemValueShop> pair)
        {
            selectItem = pair;
            if (pair.Value.itemKnowhow)
                SwitchCount(true);
            else Confirm();
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

        // 아이템 정보 초기화
        public void SetItemInfo(Dictionary<ItemKey, ItemValueShop> shopDict)
        {
            this.shopDict = shopDict;
            AssignSlots();
            AssignCosts();
        }
        #endregion
    }
}