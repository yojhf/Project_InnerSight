using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace InnerSight_Seti
{
    /// <summary>
    /// 선반 상인 전용 UI
    /// </summary>
    public class ShelfShopManager : MonoBehaviour
    {
        // 필드
        #region Variables
        private GameObject shopUI;
        private GameObject confirmUI;   //
        private GameObject completeUI;   //
        private TextMeshProUGUI goldText;
        private TextMeshProUGUI completeText;  //
        private IEnumerator tradeCor;

        private Button[] shopSlots;
        private TextMeshProUGUI[] shopCosts;
        private KeyValuePair<ItemKey, ItemValueShop> selectItem;
        private Dictionary<ItemKey, ItemValueShop> shopDict = new();

        private PlayerSetting player;
        private Transform cameraOffset;
        #endregion

        // 속성
        #region Properties
        public bool OnTrade { get; protected set; }
        #endregion

        // 라이프 사이클
        #region Life Cycle
        private void Start()
        {
            //cameraOffset = player.transform.GetChild(0);
        }

        private void Update()
        {
            if (shopUI.activeSelf)
            {
                transform.GetChild(0).LookAt(Camera.main.transform);
            }
            
        }

        private void Awake()
        {
            // UI
            shopUI = transform.GetChild(0).GetChild(0).gameObject;

            // Gold
            goldText = shopUI.transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>();

            // 버튼
            shopSlots = shopUI.transform.GetChild(1).GetComponentsInChildren<Button>();
            shopCosts = shopUI.transform.GetChild(1).GetComponentsInChildren<TextMeshProUGUI>();

            // 컨펌
            confirmUI = shopUI.transform.GetChild(2).gameObject;

            // 완료
            completeUI = transform.GetChild(0).GetChild(1).gameObject;
            completeText = completeUI.GetComponentInChildren<TextMeshProUGUI>();
        }
        #endregion

        // 메서드
        #region Methods
        // 상점 UI 스위치
        public void SwitchUI(bool isOpen)
        {
            OnTrade = isOpen;
            shopUI.SetActive(isOpen);
            goldText.text = PlayerStats.Instance.CurrentGold.ToString() + "G";
        }

        public void DefineTrade()
        {
            confirmUI.SetActive(true);
        }

        public void Confirm_Yes()
        {
            Confirm();
            confirmUI.SetActive(false);
        }
        public void Confirm_No()
        {
            confirmUI.SetActive(false);
        }

        // 정산 및 컨펌 UI
        public void Confirm()
        {
            // 가격 결정
            int howMuch = selectItem.Value.itemCost;

            // 정산
            if (PlayerStats.Instance.SpendGold(howMuch))
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
                tradeCor = TradeComplete("You don't have enough money");
            }

            StartCoroutine(tradeCor);
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

        // 버튼 선택
        private void SelectSlot(KeyValuePair<ItemKey, ItemValueShop> pair)
        {
            selectItem = pair;
            DefineTrade();
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
                int cost = pair.Value.itemCost;
                int index = pair.Value.itemIndex;
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

        public void SetPlayer(PlayerSetting player)
        {
            this.player = player;
        }
        #endregion
    }
}