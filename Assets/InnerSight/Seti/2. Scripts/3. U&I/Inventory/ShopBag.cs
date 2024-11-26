using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace InnerSight_Seti
{
    // 상점에서 플레이어가 선택한 아이템을 관리하는 클래스
    public class ShopBag : MonoBehaviour
    {
        // 필드
        #region Variables
        // 클래스 컴포넌트
        private PlayerTrade playerTrade;

        // 장바구니 요소
        private const float alphaValue = 0.9f;      // 장바구니에 담긴 아이템의 알파값
        private RectTransform shopBagTransform;     // 장바구니 아이콘
        [SerializeField]
        private GameObject shopItem;                // 슬롯 프리팹
        public List<GameObject> shopItems = new();  // 장바구니 리스트
        public Dictionary<ItemKey, ItemValue> shopDict = new();
        #endregion

        // 속성
        #region Properties
        public PlayerTrade PlayerTrade => playerTrade;
        #endregion

        // 메서드
        #region Methods
        public void SetPlayer(PlayerTrade playerTrade)
        {
            this.playerTrade = playerTrade;
            shopBagTransform = this.transform.GetChild(0) as RectTransform;
        }

        // 장바구니에 새 아이템이 추가될 때 새 슬롯을 생성하는 메서드
        public void MakeSlot(ItemKey itemKey, ItemValue itemValue)
        {
            // 아이템을 추가하고
            shopDict.Add(itemKey, itemValue);

            // 습득한 아이템의 슬롯 인덱스를 지정
            itemValue.itemIndex = shopDict.Count - 1;

            // 새 슬롯 UI 오브젝트를 생성
            shopItems.Add(Instantiate(shopItem, shopBagTransform));

            // 슬롯 상태 업데이트
            UpdateSlot(itemKey, itemValue.itemIndex);

            // 슬롯 재배치
            MathUtility.ReArrObjects(80, shopItems);

            // OnClick 리스너 자동 등록
            AssignSlots();
        }

        // 슬롯을 삭제하는 메서드
        private void TakeSlot(ItemKey selectedItemKey)
        {
            foreach (KeyValuePair<ItemKey, ItemValue> pair in shopDict)
            {
                if (pair.Key == selectedItemKey)
                {
                    // 슬롯 제거
                    Destroy(shopItems[pair.Value.itemIndex]);
                    shopItems.RemoveAt(pair.Value.itemIndex);
                }

                else if (pair.Value.itemIndex > shopDict[selectedItemKey].itemIndex)
                    pair.Value.itemIndex--;
            }

            // 해당 Element 제거
            shopDict.Remove(selectedItemKey);
        }

        // 슬롯을 업데이트하는 메서드
        private void UpdateSlot(ItemKey itemKey, int slotIndex)
        {
            // 아이콘 업데이트
            Image image = shopItems[slotIndex].GetComponent<Image>();
            image.overrideSprite = itemKey.itemImage;

            // 업데이트 된 아이콘의 알파값 조정
            ColorUtility.SetAlpha(image, alphaValue);

            CountItem(itemKey, slotIndex);
        }

        // 각 버튼의 번호에 맞는 인덱스를 자동으로 할당하는 메서드
        private void AssignSlots()
        {
            foreach (var pair in shopDict)
            {
                int index = pair.Value.itemIndex;  // 이 변수를 반드시 따로 선언해줘야 람다 함수 안에서 올바르게 작동

                shopItems[index].GetComponent<Button>().onClick.RemoveAllListeners();
                shopItems[index].GetComponent<Button>().onClick.AddListener(() => TakeSlot(pair.Key));
            }
        }

        // 아이템 수량을 표기하는 메서드
        public void CountItem(ItemKey itemKey, int index)
        {
            // 아이템의 수량이 2개 이상이라면 개수를 표기하고
            if (shopDict[itemKey].itemCount > 1)
                shopItems[index].GetComponentInChildren<TextMeshProUGUI>().text = shopDict[itemKey].ItemCount;

            // 1개라면 표기 안 함
            else
                shopItems[index].GetComponentInChildren<TextMeshProUGUI>().text = "";
        }
        #endregion
    }
}