using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace InnerSight_Seti
{
    /// <summary>
    /// 상점 UI 관리 체계의 부모 클래스
    /// </summary>
    public abstract class ShopManager : MonoBehaviour
    {
        // 필드
        #region Variables
        protected GameObject shopUI;
        protected GameObject confirmUI;
        protected GameObject completeUI;
        protected TextMeshProUGUI goldText;
        protected TextMeshProUGUI completeText;
        protected IEnumerator tradeCor;

        protected Button[] shopSlots;
        protected TextMeshProUGUI[] shopCosts;
        protected KeyValuePair<ItemKey, ItemValueShop> selectItem;
        protected Dictionary<ItemKey, ItemValueShop> shopDict = new();

        // 클래스
        protected PlayerSetting player;

        protected bool CanTrade = false;
        #endregion

        // 속성
        #region
        public bool OnTrade { get; set; }
        #endregion

        // 라이프 사이클
        #region Life Cycle
        protected virtual void Awake()
        {
            // UI
            shopUI = transform.GetChild(0).GetChild(0).gameObject;

            // Gold
            goldText = shopUI.transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>();

            // 버튼
            shopSlots = shopUI.transform.GetChild(1).GetComponentsInChildren<Button>();
            shopCosts = shopUI.transform.GetChild(1).GetComponentsInChildren<TextMeshProUGUI>();

            // 완료
            completeUI = transform.GetChild(0).GetChild(1).gameObject;
            completeText = completeUI.GetComponentInChildren<TextMeshProUGUI>();
        }

        protected virtual void Update()
        {
            if (shopUI.activeSelf)
            {
                transform.GetChild(0).LookAt(Camera.main.transform);

                if (!CanTrade)
                {
                    UIReset();
                }
            }
        }
        #endregion

        // 메서드
        #region Methods
        // 상점 UI 스위치
        public void SwitchUI(bool isOpen)
        {
            UIReset();
            OnTrade = isOpen;
            shopUI.SetActive(isOpen);
            goldText.text = PlayerStats.Instance.CurrentGold.ToString() + "G";
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

        protected virtual void UIReset()
        {
            confirmUI.SetActive(false);
            shopUI.SetActive(false);
            OnTrade = false;
        }
        #endregion

        // 추상 메서드
        #region Abstract
        public abstract void Confirm();
        protected abstract void SelectSlot(KeyValuePair<ItemKey, ItemValueShop> pair);
        protected abstract void AssignCosts();
        #endregion

        // 기타 유틸리티
        #region Utilities
        // 컨펌 텍스트
        protected IEnumerator TradeComplete(string text)
        {
            completeUI.SetActive(true);
            completeText.text = text;
            yield return new WaitForSeconds(2);

            completeUI.SetActive(false);
            tradeCor = null;
            yield break;
        }

        // 각 버튼의 인덱스에 맞는 리스너를 자동으로 할당하는 메서드
        protected void AssignSlots()
        {
            foreach (var pair in shopDict)
            {
                int index = pair.Value.itemIndex;  // 이 변수를 반드시 따로 선언해줘야 람다 함수 안에서 올바르게 작동

                shopSlots[index].onClick.RemoveAllListeners();
                shopSlots[index].onClick.AddListener(() => SelectSlot(pair));
            }
        }

        public void DistanceCheck(bool CanTrade)
        {
            this.CanTrade = CanTrade;
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