using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Specialized;
using System.Linq;

namespace InnerSight_Seti
{
    // 플레이어의 인벤토리를 관리하는 클래스
    public class Inventory : MonoBehaviour
    {
        // 필드
        #region Variables
        // 클래스 컴포넌트
        private InventoryManager inventoryManager;

        // 인벤토리 요소
        [SerializeField]
        private GameObject invenPanel;
        private TextMeshProUGUI[] invenSlotsCountTexts;
        public Button[] invenSlots;
        [HideInInspector]
        public RectTransform invenRect;   // 슬롯 초기화를 위해 SetActive가 아닌 scale로 스위칭
        public Dictionary<ItemKey, ItemValue> invenDict = new();

        // 초기화용 필드
        [SerializeField]
        private Sprite emptySlotSprite;
        private const float alphaValue = 0.9f;        // 인벤토리에 담긴 아이템의 알파값
        private const float initialAlphaValue = 0.4f;
        #endregion

        // 라이프 사이클
        #region Life Cycle
        private void Start()
        {
            // 컴포넌트 초기화
            inventoryManager = GetComponentInParent<InventoryManager>();
            invenRect = transform.GetChild(0).GetChild(0).GetComponent<RectTransform>();
            //invenPanel = transform.GetChild(0).gameObject;
            invenSlots = invenPanel.GetComponentsInChildren<Button>();
            invenSlotsCountTexts = GetComponentsInChildren<TextMeshProUGUI>();
        }

        private void Update()
        {
            /*for (int i = 0; i < invenSlots.Length; i++)
            {
                invenSlotsCountTexts[i].text = "";
                invenSlots[i].GetComponent<Image>().sprite = emptySlotSprite;
                ColorUtility.SetAlpha(invenSlots[i].image, initialAlphaValue);
            }

            foreach (var pair in invenDict)
            {
                UpdateSlot(pair, pair.Value.itemIndex);
            }*/
        }
        #endregion

        // 메서드
        #region Methods
        // 인벤토리에 새 아이템이 추가될 때 새 슬롯을 생성하는 메서드
        public void MakeSlot(ItemKey itemKey)
        {
            // 습득한 아이템의 itemValue를 인스턴스로 만들고
            ItemValue itemValue = new();

            // 아이템을 추가하고
            invenDict.Add(itemKey, itemValue);

            // 습득한 아이템의 슬롯 인덱스를 지정
            itemValue.itemIndex = invenDict.Count - 1;

            // 인벤토리 상태 업데이트
            UpdateSlot(inventoryManager.ItemData(itemKey), invenDict[itemKey].itemIndex);
        }

        // 슬롯을 삭제하는 메서드
        public void TakeSlot(ItemKey selectedItemKey)
        {
            // 타겟 슬롯을 먼저 비우고
            int targetIndex = invenDict[selectedItemKey].itemIndex;
            ClearSlot(inventoryManager.ItemData(selectedItemKey));
            invenDict.Remove(selectedItemKey);

            // 슬롯 자동 정렬 - Dictionary는 순서 보장 X, 그러므로
            // 우선 비우고
            foreach (KeyValuePair<ItemKey, ItemValue> pair in invenDict)
                if (pair.Value.itemIndex > targetIndex)
                    ClearSlot(inventoryManager.ItemData(pair.Key));
                // 다시 채운다
            foreach (KeyValuePair<ItemKey, ItemValue> pair in invenDict)
            {
                if (pair.Value.itemIndex > targetIndex)
                {
                    pair.Value.itemIndex--;
                    UpdateSlot(inventoryManager.ItemData(pair.Key), invenDict[pair.Key].itemIndex);
                }
            }
        }

        // 두 아이템의 슬롯을 교환하는 메서드
        public void SwapInvenSlots(int firstIndex, int secondIndex, ItemKey secondItem)
        {
            if (firstIndex == secondIndex)
                return;

            // 기존 아이템 참조 저장
            var firstItem = CollectionUtility.FirstOrDefault(invenDict.Keys, key => invenDict[key].itemIndex == firstIndex);

            // 첫 번째 슬롯과 두 번째 슬롯을 교환
            if (firstItem != null)
            {
                ItemValue itemValue = new()
                {
                    itemCount = invenDict[firstItem].itemCount,
                    itemIndex = secondIndex
                };
                UpdateSlot(inventoryManager.ItemData(firstItem), secondIndex);
                invenDict.Remove(firstItem);
                invenDict.Add(firstItem, itemValue);
                CountItem(inventoryManager.ItemData(firstItem), secondIndex);
            }

            if (secondItem != null)
            {
                ItemValue itemValue = new()
                {
                    itemCount = invenDict[secondItem].itemCount,
                    itemIndex = firstIndex
                };
                UpdateSlot(inventoryManager.ItemData(secondItem), firstIndex);
                invenDict.Remove(secondItem);
                invenDict.Add(secondItem, itemValue);
                CountItem(inventoryManager.ItemData(secondItem), firstIndex);
            }
        }

        // 인벤토리슬롯 업데이트 메서드 (아이템 추가)
        private void UpdateSlot(KeyValuePair<ItemKey, ItemValue> pair, int slotIndex)
        {
            // 슬롯 아이콘 업데이트
            Image image = invenSlots[slotIndex].GetComponent<Image>();
            image.overrideSprite = pair.Key.itemImage;

            // 업데이트 된 아이콘의 알파값 조정
            ColorUtility.SetAlpha(image, alphaValue);

            CountItem(pair, slotIndex);
        }

        // 인벤토리슬롯 업데이트 메서드 (아이템 제거)
        private void ClearSlot(KeyValuePair<ItemKey, ItemValue> pair)
        {
            if (!invenDict.ContainsKey(pair.Key)) return;

            invenSlotsCountTexts[pair.Value.itemIndex].text = "";
            invenSlots[pair.Value.itemIndex].GetComponent<Image>().overrideSprite = emptySlotSprite;
            ColorUtility.SetAlpha(invenSlots[pair.Value.itemIndex].GetComponent<Image>(), initialAlphaValue);
        }

        // 아이템 수량을 표기하는 메서드
        public void CountItem(KeyValuePair<ItemKey, ItemValue> pair, int slotIndex)
        {
            TextMeshProUGUI itemCount = invenSlots[slotIndex].GetComponentInChildren<TextMeshProUGUI>();

            // 아이템의 수량이 2개 이상이라면 개수를 표기하고
            if (invenDict[pair.Key].itemCount > 1)
                itemCount.text = invenDict[pair.Key].ItemCount;

            // 1개라면 표기 안 함
            else itemCount.text = "";
        }

        // NPC와의 거래 - 리스너 등록
        public void AssignInvenSlots()
        {
            foreach (var pair in invenDict)
            {
                int index = pair.Value.itemIndex;
                invenSlots[index].GetComponent<Button>().onClick.RemoveAllListeners();
                invenSlots[index].GetComponent<Button>().onClick.AddListener(() => inventoryManager.UseInven(pair));
            }
        }

        // NPC와의 거래 - 리스너 해제
        public void EraseInvenSlots()
        {
            foreach (var pair in invenDict)
            {
                int index = pair.Value.itemIndex;
                invenSlots[index].GetComponent<Button>().onClick.RemoveAllListeners();
            }
        }
        #endregion
    }
}