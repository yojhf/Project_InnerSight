using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using TMPro;

namespace InnerSight_Seti
{
    // 플레이어의 인벤토리와 퀵슬롯을 운영하는 클래스
    public class InventoryManager : MonoBehaviour
    {
        // 필드
        #region Variables
        // 컨트롤 제어 획득
        private Control control;

        // 허상 아이템 관련 필드
        private GameObject itemPhantom;
        private IEnumerator phantomCor;
        private Vector3 cursorPosition = Vector3.zero;

        // 단순 변수
        private int? selectedSlotIndex = null;  // 슬롯 인덱스, 인벤토리나 퀵슬롯의 슬롯을 선택했다면 값이 존재하고 그렇지 않으면 null

        // 불리언 변수
        private bool isSelected = false;        // 아이템 선택 상태 판정

        // 컴포넌트
        private Button initialSlot;
        [SerializeField]
        private Button thisSlot;
        private EventSystem eventSystem;
        private GraphicRaycaster raycaster;

        // 클래스 컴포넌트
        private PlayerSetting player;
        private Inventory inventory;
        private NPC_Merchant tradeNPC;
        #endregion

        // 속성
        #region Properties
        public float PhantomDepth { get; set; }
        public int? SelectedSlotIndex => selectedSlotIndex;
        public bool IsSelected => isSelected;
        public bool IsOpenInventory { get; private set; }
        public bool IsOnTrade { get; set; }
        public Button ThisSlot => thisSlot;
        public Inventory Inventory => inventory;
        #endregion

        // 라이프 사이클
        #region Life Cycle
        private void Awake()
        {
            control = new();

            // 초기화
            UIManager UIManager = GetComponent<UIManager>();
            UIManager.PlayerUse.SetInventory(this);
            player = UIManager.Player;

            eventSystem = FindAnyObjectByType<EventSystem>();
            raycaster = GetComponentInChildren<GraphicRaycaster>();
            inventory = GetComponentInChildren<Inventory>();

            PhantomDepth = 1;
        }

        private void OnEnable()
        {
            // 인벤토리 관련 이벤트 핸들러 구독
            control.Player.Inventory.started += OnInventoryStarted;
            control.Player.CursorClick.started += OnCursorClickStarted;
            control.Player.CursorClick.canceled += OnCursorClickCanceled;

            // 실행취소 이벤트 핸들러 구독
            control.UI.Cancel.started += OnCancelStarted;

            // 컨트롤 제어 활성화
            control.Player.CursorClick.Enable();
            control.Player.Inventory.Enable();
            control.UI.Cancel.Enable();
        }

        private void OnDisable()
        {
            // 컨트롤 제어 비활성화
            control.UI.Cancel.Disable();
            control.Player.Inventory.Disable();
            control.Player.CursorClick.Disable();

            // 실행취소 이벤트 핸들러 구독 해제
            control.UI.Cancel.started -= OnCancelStarted;

            // 인벤토리 관련 이벤트 핸들러 구독 해제
            control.Player.Inventory.started -= OnInventoryStarted;
            control.Player.CursorClick.started -= OnCursorClickStarted;
            control.Player.CursorClick.canceled -= OnCursorClickCanceled;
        }
        #endregion

        // 이벤트 핸들러
        #region Event Handlers
        public void OnCancelStarted(InputAction.CallbackContext _)
        {
            if (IsOpenInventory) ShowItem(IsOpenInventory = false);
        }
        public void OnInventoryStarted(InputAction.CallbackContext _) => ShowItem(IsOpenInventory = !IsOpenInventory);
        public void OnCursorClickStarted(InputAction.CallbackContext _) => WhichSelect();

        public void OnCursorClickCanceled(InputAction.CallbackContext _) => isSelected = false;
        #endregion

        // 메서드
        #region Methods
        // 아이템 정보를 캡슐화
        public KeyValuePair<ItemKey, ItemValue> ItemData(ItemKey itemKey)
        {
            return new KeyValuePair<ItemKey, ItemValue>(itemKey, inventory.invenDict[itemKey]);
        }

        public void SetNPC(NPC_Merchant tradeNPC)
        {
            this.tradeNPC = tradeNPC;
        }

        public void UseQuick(KeyValuePair<ItemKey, ItemValue> pair)
        {
            //player.PlayerUse.UseItem(pair);
        }

        // NPC와의 거래 - 아이템 판매
        public void UseInven(KeyValuePair<ItemKey, ItemValue> pair)
        {
            tradeNPC.TradeSetItem(pair.Key);
            DecreaseItem(pair);
        }

        public void ForTrade(bool isOpen) => ShowItem(IsOpenInventory = isOpen);

        // 아이템 운용으로 인벤토리가 비게 되면 호출되는 메서드
        public void EmptySignal()
        {
            player.CursorUtility.CursorSwitch(false);
            ShowItem(IsOpenInventory = false);
        }

        // 인벤토리 개폐
        public void ShowItem(bool isOpen)
        {
            int scaleX = isOpen? 1 : 0;
            inventory.invenRect.localScale = new(scaleX, 1, 1);

            // NPC와 거래 중일 때만 실행하는 로직
            if (IsOnTrade)
            {
                if (isOpen)
                {
                    inventory.AssignInvenSlots();
                }
                else
                {
                    inventory.EraseInvenSlots();
                }
            }

            foreach (var pair in inventory.invenDict)
                inventory.CountItem(pair, pair.Value.itemIndex);

            player.CursorUtility.CursorSwitch(isOpen);
        }

        // PlayerInteraction 클래스로부터 획득한 아이템을 인벤토리에 저장하는 메서드
        public void AddItem(ItemKey itemKey, int count)
        {
            // 현재 인벤토리에 해당 아이템이 있으면
            if (inventory.invenDict.ContainsKey(itemKey))
            {
                // 개수를 늘린다
                inventory.invenDict[itemKey].Count(count);
                inventory.CountItem(ItemData(itemKey), inventory.invenDict[itemKey].itemIndex);
            }

            // 현재 인벤토리에 해당 아이템이 없으면
            else
            {
                // 새 슬롯을 생성해서 아이템 할당
                inventory.MakeSlot(itemKey);
            }
        }

        // 아이템을 떨어뜨리는 메서드
        public void DropItem(KeyValuePair<ItemKey, ItemValue> selectedItem)
        {
            // 먼저 허상 아이템을 제거한 뒤
            Destroy(itemPhantom);

            // 허상 아이템의 위치에 진짜 아이템을 생성하고
            float yRot = UnityEngine.Random.Range(0, 360);
            Instantiate(selectedItem.Key.itemPrefab, itemPhantom.transform.position, Quaternion.Euler(0, yRot, 0));

            // 허상 아이템 저장 변수를 비운 다음
            itemPhantom = null;

            // 수량 갱신
            DecreaseItem(selectedItem);
        }

        // 아이템 수량 관리 메서드
        public void DecreaseItem(KeyValuePair<ItemKey, ItemValue> selectedItem)
        {
            // 아이템의 수량을 감소시킨다
            if (inventory.invenDict.ContainsKey(selectedItem.Key))
                inventory.invenDict[selectedItem.Key].Count(-1);

            // 아이템이 아직 사용 가능하다면 수량만 감소시키고
            if (inventory.invenDict[selectedItem.Key].IsUsable())
                inventory.CountItem(selectedItem, selectedItem.Value.itemIndex);

            // 더 이상 아이템이 없다면 슬롯 제거
            else inventory.TakeSlot(selectedItem.Key);
        }

        #region SelectSlot
        // 어떤 슬롯을 선택했는지 확인하는 메서드
        private void WhichSelect()
        {
            if (IsOnTrade) return;

            // 아이템 선택 플래그를 true
            isSelected = true;

            StartCoroutine(DetectSlot());

            if (thisSlot != null)
            {
                // 슬롯이 인벤토리에 있는 경우
                if (Array.Exists(inventory.invenSlots, slot => slot == thisSlot))
                {
                    initialSlot = thisSlot;
                    selectedSlotIndex = Array.IndexOf(inventory.invenSlots, thisSlot);
                    SelectInven((int)selectedSlotIndex);
                }
                selectedSlotIndex = null;
            }

            else
            {
                StopCoroutine(DetectSlot());
                return;
            }
        }

        // WhichSelect에서 인벤토리로 확인했을 때 호출되는 메서드
        private void SelectInven(int invenIndex)
        {
            // 선택한 아이템의 키를 읽을 변수
            ItemKey selectedItemKey = null;

            // 선택한 아이템과 일치하는 키를 찾고
            selectedItemKey = CollectionUtility.FirstOrDefault(inventory.invenDict, pair => pair.Value.itemIndex == invenIndex).Key;

            // 허상 아이템 생성
            GenItemPhantom(selectedItemKey);
        }   

        // 허상 아이템을 생성하는 메서드
        private void GenItemPhantom(ItemKey selectedItemKey)
        {
            // 해당 키가 존재할 때에만
            if (selectedItemKey == null) return;

            // 허상 아이템을 생성하여 드랍할 위치를 선정하는 반복기 호출
            itemPhantom = Instantiate(selectedItemKey.itemPhantomPrefab, cursorPosition, Quaternion.identity);

            phantomCor = PhantomUpdate(selectedItemKey);
            StartCoroutine(phantomCor);
        }

        // 같은 슬롯에 아이템을 두었다면 사용
        private void SameSelect(Button thisSlot)
        {
            // 이제 인벤토리에서 제자리 버튼이라면 아이템을 사용하도록 바꾼다
            if (initialSlot == thisSlot)
            {
                // 해당 슬롯의 아이템을 찾고
                var itemKey = CollectionUtility.FirstOrNull(inventory.invenDict.Keys,
                    key => inventory.invenDict[key].itemIndex == Array.IndexOf(inventory.invenSlots, thisSlot));

                StopCoroutine(phantomCor);              // 허상 아이템 동기화 코루틴을 강제 종료하고
                Destroy(itemPhantom);  // 허상 아이템을 제거한 뒤
                phantomCor = null;
                itemPhantom = null;
                thisSlot = null;

                /*if (itemKey != null)
                    player.PlayerUse.UseItem(ItemData(itemKey));   // 사용*/
            }

            else return;
        }
        #endregion
        #endregion

        // 기타 유틸리티
        #region Utilities
        // 허상 아이템을 잡아두는 반복기
        public IEnumerator PhantomUpdate(ItemKey selectedItemKey)
        {
            while (isSelected)
            {
                // cursorUtility의 이벤트 핸들러를 통해 Vector2 CursorPosition을 Vector3 변수에 입력
                Vector3 mousePosition = player.CursorUtility.CursorPosition;

                // 적절한 깊이값 설정
                mousePosition.z = PhantomDepth;

                // 마우스 포인터의 위치를 월드 좌표로 변환
                Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

                // 허상 아이템의 transform.position을 해당 좌표와 동기화
                itemPhantom.transform.position = worldPosition;

                // 본 반복기를 매 프레임 실행
                yield return null;
            }

            // 다른 슬롯에 두었다면 종료
            if (thisSlot != null)
            {
                if (initialSlot == thisSlot)
                {

                    //SameSelect(thisSlot);
                }

                else
                {
                    int firstIndex = Array.IndexOf(inventory.invenSlots, initialSlot);
                    int secondIndex = Array.IndexOf(inventory.invenSlots, thisSlot);
                    inventory.SwapInvenSlots(firstIndex, secondIndex);
                }

                Destroy(itemPhantom);
                phantomCor = null;
                yield break;
            }

            // 플레이어가 적절한 위치에서 드래그를 해제하면 해당 아이템을 실체화
            DropItem(ItemData(selectedItemKey));

            // 이 반복기를 기억하는 변수를 비우고
            phantomCor = null;

            // 작동 중인 코루틴 정지
            yield break;
        }

        // 슬롯을 감지하는 반복기
        public IEnumerator DetectSlot()
        {
            // 다시 클릭할 때까지 계속 반복
            while (IsSelected)
            {
                // EventSystem으로부터 포인터 정보를 받고
                PointerEventData pointerData = new(eventSystem)
                {
                    position = Mouse.current.position.ReadValue()
                };

                // 그를 기반으로 GraphicRaycaster 시행
                List<RaycastResult> results = new();
                raycaster.Raycast(pointerData, results);

                // 현재의 슬롯을 감지
                thisSlot = null;
                foreach (RaycastResult result in results)
                {
                    // 감지한 UGUI가 텍스트박스라면 무시
                    if (result.gameObject.GetComponent<TextMeshProUGUI>())
                        continue;

                    // Button UI 획득을 시도해보고 잡히면 선택
                    if (result.gameObject.TryGetComponent<Button>(out var slot))
                        thisSlot = slot;
                }

                // 이 반복기를 매 프레임 반복하고
                yield return null;
            }

            // 역할이 끝나면 종료
            yield break;
        }
        #endregion
    }
}