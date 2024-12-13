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
        private List<Item> shopList = new();
        private TextMeshProUGUI[] shopCosts;
        #endregion

        // 속성
        #region Properties
        public bool OnTrade { get; protected set; }
        #endregion

        // 라이프 사이클
        #region Life Cycle
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
            OnTrade = isOpen;
            shopUI.SetActive(isOpen);
            goldText.text = PlayerStats.Instance.CurrentGold.ToString() + "G";
        }

        // 정산 및 컨펌 UI
        public void Confirm()
        {
            tradeCor = TradeComplete("소지금이 부족합니다.");
            StartCoroutine(tradeCor);
            SwitchUI(false);
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
            
        }

        // 각 버튼의 인덱스에 맞는 리스너를 자동으로 할당하는 메서드
        private void AssignSlots()
        {
            /*foreach (var pair in shopList)
            {
                int index = pair.Value.itemIndex;  // 이 변수를 반드시 따로 선언해줘야 람다 함수 안에서 올바르게 작동

                shopSlots[index].onClick.RemoveAllListeners();
                shopSlots[index].onClick.AddListener(() => SelectSlot(pair));
            }*/
        }
        #endregion
    }
}