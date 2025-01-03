using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit.UI;
using InnerSight_Kys;

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

        // 컴포넌트
        private Button initialSlot;
        [SerializeField]
        private Button thisSlot;
        private EventSystem eventSystem;
        private TrackedDeviceGraphicRaycaster raycaster;

        // 클래스 컴포넌트
        private PlayerSetting player;
        [SerializeField]
        private Inventory inventory;
        private NPC_Merchant tradeNPC;
        #endregion

        // 속성
        #region Properties
        public int? SelectedSlotIndex { get; private set; }
        public float PhantomDepth { get; private set; }
        public bool IsSelected { get; private set; }
        public bool IsOpenInventory { get; set; }
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
            //UIManager.PlayerUse.SetInventory(this);
            player = UIManager.Player;

            eventSystem = FindAnyObjectByType<EventSystem>();
            raycaster = GetComponentInChildren<TrackedDeviceGraphicRaycaster>();
            //raycaster = GetComponentInChildren<GraphicRaycaster>();
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
        public void OnCursorClickStarted(InputAction.CallbackContext _)
        {

        }

        public void OnCursorClickCanceled(InputAction.CallbackContext _)
        {

        }
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
            //tradeNPC.TradeSetItem(pair.Key);
            DecreaseItem(pair);
        }

        public void ForTrade(bool isOpen) => ShowItem(IsOpenInventory = isOpen);

        // 아이템 운용으로 인벤토리가 비게 되면 호출되는 메서드
        public void EmptySignal()
        {
            //player.CursorUtility.CursorSwitch(false);
            ShowItem(IsOpenInventory = false);
        }

        // 인벤토리 개폐
        public void ShowItem(bool isOpen)
        {
            float scaleX = isOpen? 1.3f : 0;
            inventory.invenRect.localScale = new(scaleX, 1.3f, 1.3f);

            foreach (var pair in inventory.invenDict)
                inventory.CountItem(pair, pair.Value.itemIndex);

            if (!isOpen)
                Initialize();
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

                // 개수를 늘린다
                inventory.invenDict[itemKey].Count(count - 1);
                inventory.CountItem(ItemData(itemKey), inventory.invenDict[itemKey].itemIndex);
            }
        }

        // 아이템을 떨어뜨리는 메서드
        public void DropItem(KeyValuePair<ItemKey, ItemValue> selectedItem)
        {
            AudioManager.Instance.Play("Drop");

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
        private IEnumerator WhichSelect()
        {
            if (!player.rayInteractor.gameObject.activeSelf)
                yield break;

            if (IsOnTrade) yield break;

            // 아이템 선택 플래그를 true
            //IsSelected = true;

            StartCoroutine(FindInitialSlot(player.rayInteractor));
            while (initialSlot == null) yield return null;
            XR_Detect();

            if (initialSlot != null)
            {
                // 슬롯이 인벤토리에 있는 경우
                if (Array.Exists(inventory.invenSlots, slot => slot == initialSlot))
                {
                    SelectedSlotIndex = Array.IndexOf(inventory.invenSlots, initialSlot);
                    SelectInven((int)SelectedSlotIndex);
                }
                SelectedSlotIndex = null;
            }
        }


        // WhichSelect에서 인벤토리로 확인했을 때 호출되는 메서드
        private void SelectInven(int invenIndex)
        {
            // 선택한 아이템의 키를 읽을 변수
            ItemKey selectedItemKey = null;

            // 선택한 아이템과 일치하는 키를 찾고
            selectedItemKey = CollectionUtility.FirstOrNull(inventory.invenDict.Keys, key => inventory.invenDict[key].itemIndex == invenIndex);
            if (selectedItemKey != null)
                // 허상 아이템 생성
                GenItemPhantom(selectedItemKey);
        }   

        // 허상 아이템을 생성하는 메서드
        private void GenItemPhantom(ItemKey selectedItemKey)
        {
            // 해당 키가 존재할 때에만
            if (selectedItemKey == null) return;
            if (itemPhantom != null) return;

            // 허상 아이템을 생성하여 드랍할 위치를 선정하는 반복기 호출
            itemPhantom = Instantiate(selectedItemKey.itemPhantomPrefab, player.rayInteractor.transform.position, Quaternion.identity);
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

                StopCoroutine(phantomCor);  // 허상 아이템 동기화 코루틴을 강제 종료하고
                Destroy(itemPhantom);       // 허상 아이템을 제거한 뒤
                itemPhantom = null;
                phantomCor = null;
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
            while (IsSelected)
            {
                // cursorUtility의 이벤트 핸들러를 통해 Vector2 CursorPosition을 Vector3 변수에 입력
                Vector3 mousePosition = player.rayInteractor.transform.position;

                // 허상 아이템의 transform.position을 해당 좌표와 동기화
                itemPhantom.transform.position = mousePosition;

                // 본 반복기를 매 프레임 실행
                yield return null;
            }

            // 다른 슬롯에 두었다면 종료
            if (thisSlot != null)
            {
                if (initialSlot == thisSlot)
                {
                    SameSelect(thisSlot);
                }
                else
                {
                    int firstIndex = Array.IndexOf(inventory.invenSlots, initialSlot);
                    int secondIndex = Array.IndexOf(inventory.invenSlots, thisSlot);
                    ItemKey secondItem = CollectionUtility.FirstOrNull(inventory.invenDict.Keys,
                                                                       key => inventory.invenDict[key].itemIndex == secondIndex);
                    if (secondItem != null)
                        inventory.SwapInvenSlots(firstIndex, secondIndex, secondItem);
                }
                
                Destroy(itemPhantom);
                initialSlot = null;
                phantomCor = null;
                yield break;
            }

            // 플레이어가 적절한 위치에서 드래그를 해제하면 해당 아이템을 실체화
            DropItem(ItemData(selectedItemKey));

            // 변수 초기화
            initialSlot = null;
            phantomCor = null;

            // 작동 중인 코루틴 정지
            yield break;
        }

        public IEnumerator FindInitialSlot(XRRayInteractor rayInteractor)
        {
            int count = 0;

            // 다시 클릭할 때까지 계속 반복
            while (count < 50)
            {
                if (rayInteractor.TryGetCurrentUIRaycastResult(out var re))
                {
                    //Button UI 획득을 시도해보고 잡히면 선택
                    if (re.gameObject.transform.TryGetComponent<Button>(out var slot))
                    {
                        initialSlot = slot;
                        yield break;
                    }
                }
                count++;
                // 이 반복기를 매 프레임 반복하고
                yield return null;
            }
            // 역할이 끝나면 종료
            yield break;
        }

        private void Initialize()
        {
            if (phantomCor != null)
            {
                StopAllCoroutines();
                phantomCor = null;
            }

            if (itemPhantom)
            {
                Destroy(itemPhantom);
                itemPhantom = null;
            }
            
            thisSlot = null;
            initialSlot = null;
            IsSelected = false;
        }
        #endregion













        #region Right_Hand
        public void Right_XR_WhichSelect()
        {
            StartCoroutine(Right_WhichSelect());
        }

        void Right_XR_Detect()
        {

            if (player.right_rayInteractor == null || eventSystem == null)
            {
                Debug.LogWarning("XR_Right_RayInteractor 또는 EventSystem이 설정되지 않았습니다.");
                return;
            }

            StartCoroutine(Detect(player.right_rayInteractor));
        }

        // 어떤 슬롯을 선택했는지 확인하는 메서드
        private IEnumerator Right_WhichSelect()
        {
            if (!player.right_rayInteractor.gameObject.activeSelf)
                yield break;

            if (IsOnTrade) yield break;

            // 아이템 선택 플래그를 true
            //IsSelected = true;

            StartCoroutine(FindInitialSlot(player.right_rayInteractor));
            while (initialSlot == null) yield return null;
            Right_XR_Detect();

            if (initialSlot != null)
            {
                // 슬롯이 인벤토리에 있는 경우
                if (Array.Exists(inventory.invenSlots, slot => slot == initialSlot))
                {
                    SelectedSlotIndex = Array.IndexOf(inventory.invenSlots, initialSlot);
                    Right_SelectInven((int)SelectedSlotIndex);
                }
                SelectedSlotIndex = null;
            }
        }

        // WhichSelect에서 인벤토리로 확인했을 때 호출되는 메서드
        private void Right_SelectInven(int invenIndex)
        {
            // 선택한 아이템의 키를 읽을 변수
            ItemKey selectedItemKey = null;

            // 선택한 아이템과 일치하는 키를 찾고
            selectedItemKey = CollectionUtility.FirstOrNull(inventory.invenDict.Keys, key => inventory.invenDict[key].itemIndex == invenIndex);
            if (selectedItemKey != null)
                // 허상 아이템 생성
                Right_GenItemPhantom(selectedItemKey);
        }

        // 허상 아이템을 생성하는 메서드
        private void Right_GenItemPhantom(ItemKey selectedItemKey)
        {
            // 해당 키가 존재할 때에만
            if (selectedItemKey == null) return;
            if (itemPhantom != null) return;

            // 허상 아이템을 생성하여 드랍할 위치를 선정하는 반복기 호출
            itemPhantom = Instantiate(selectedItemKey.itemPhantomPrefab, player.right_rayInteractor.transform.position, Quaternion.identity);
            phantomCor = Right_PhantomUpdate(selectedItemKey);
            StartCoroutine(phantomCor);
        }

        // 허상 아이템을 잡아두는 반복기
        public IEnumerator Right_PhantomUpdate(ItemKey selectedItemKey)
        {
            while (IsSelected)
            {
                // cursorUtility의 이벤트 핸들러를 통해 Vector2 CursorPosition을 Vector3 변수에 입력
                Vector3 mousePosition = player.right_rayInteractor.transform.position;

                // 허상 아이템의 transform.position을 해당 좌표와 동기화
                itemPhantom.transform.position = mousePosition;

                // 본 반복기를 매 프레임 실행
                yield return null;
            }

            // 다른 슬롯에 두었다면 종료
            if (thisSlot != null)
            {
                if (initialSlot == thisSlot)
                {
                    SameSelect(thisSlot);
                }
                else
                {
                    int firstIndex = Array.IndexOf(inventory.invenSlots, initialSlot);
                    int secondIndex = Array.IndexOf(inventory.invenSlots, thisSlot);
                    ItemKey secondItem = CollectionUtility.FirstOrNull(inventory.invenDict.Keys,
                                                                       key => inventory.invenDict[key].itemIndex == secondIndex);
                    if (secondItem != null)
                        inventory.SwapInvenSlots(firstIndex, secondIndex, secondItem);
                }

                Destroy(itemPhantom);
                initialSlot = null;
                phantomCor = null;
                yield break;
            }

            // 플레이어가 적절한 위치에서 드래그를 해제하면 해당 아이템을 실체화
            DropItem(ItemData(selectedItemKey));

            // 변수 초기화
            initialSlot = null;
            phantomCor = null;

            // 작동 중인 코루틴 정지
            yield break;
        }
        #endregion
















        #region VR

        // VR

        public void ResetData()
        {
            IsSelected = false;
            //thisSlot = null;
        }

        public void XR_WhichSelect()
        {
            StartCoroutine(WhichSelect());
        }

        void XR_Detect()
        {

            if (player.rayInteractor == null || eventSystem == null)
            {
                Debug.LogWarning("XRRayInteractor 또는 EventSystem이 설정되지 않았습니다.");
                return;
            }

            StartCoroutine(Detect(player.rayInteractor));
        }

        // 현재 슬롯 갱신 반복기
        public IEnumerator Detect(XRRayInteractor rayInteractor)
        {
            // 그를 기반으로 GraphicRaycaster 시행
            List<RaycastResult> results = new();

            IsSelected = true;

            // 다시 클릭할 때까지 계속 반복
            while (IsSelected)
            {
                thisSlot = null;
                if (rayInteractor.TryGetCurrentUIRaycastResult(out var re))
                {
                    if (!results.Contains(re))
                        results.Add(re);
                    
                    foreach (var result in results)
                    {
                        //Button UI 획득을 시도해보고 잡히면 선택
                        if (ComponentUtility.TryGetComponentInChildren<Button>(result.gameObject.transform, out var slot))
                        {
                            thisSlot = slot;
                        }
                    }
                }

                // 이 반복기를 매 프레임 반복하고
                yield return null;
            }

            results.Clear();

            // 역할이 끝나면 종료
            yield break;
        }
        #endregion
    }
}

#region Dummy_Seti
/*// 슬롯을 감지하는 반복기 (XR 버전)
public IEnumerator DetectSlot(XRRayInteractor rayInteractor)
{
    // 다시 클릭할 때까지 계속 반복
    while (IsSelected)
    {
        // 레이 인터랙터가 UI에 히트를 가했는지 확인
        if (rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
        {
            // 히트 지점의 월드 좌표를 스크린 좌표로 변환
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(hit.point);

            // EventSystem으로부터 포인터 정보를 받음
            PointerEventData pointerData = new(EventSystem.current)
            {
                position = (Vector2)screenPosition
            };

            // 그를 기반으로 GraphicRaycaster 시행
            List<RaycastResult> results = new();
            raycaster.Raycast(pointerData, results);

            Debug.Log(results);

            // 현재의 슬롯을 감지
            thisSlot = null;
            foreach (RaycastResult result in results)
            {
                *//*// 감지한 UGUI가 텍스트박스라면 무시
                if (result.gameObject.GetComponent<TextMeshProUGUI>())
                    continue;

                // Button UI 획득을 시도해보고 잡히면 선택
                if (result.gameObject.TryGetComponent<Button>(out var slot))
                    thisSlot = slot;*//*

                if (ComponentUtility.TryGetComponentInChildren<Button>(result.gameObject.transform, out var slot))
                    thisSlot = slot;
            }
        }

        // 이 반복기를 매 프레임 반복하고
        yield return null;
    }

    // 역할이 끝나면 종료
    yield break;
}

// 슬롯을 감지하는 반복기 (PC 버전)
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
}*/
#endregion