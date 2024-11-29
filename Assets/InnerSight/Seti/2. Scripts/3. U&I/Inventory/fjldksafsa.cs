using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit.UI;

public class fjldksafsa : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}


/*using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.InputSystem.XR;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit.UI;

namespace InnerSight_Seti
{
    // �÷��̾��� �κ��丮�� �������� ��ϴ� Ŭ����
    public class InventoryManager : MonoBehaviour
    {
        // �ʵ�
        #region Variables
        // ��Ʈ�� ���� ȹ��
        private Control control;

        // ��� ������ ���� �ʵ�
        private GameObject itemPhantom;
        private IEnumerator phantomCor;
        private Vector3 cursorPosition = Vector3.zero;

        // �ܼ� ����
        private int? selectedSlotIndex = null;  // ���� �ε���, �κ��丮�� �������� ������ �����ߴٸ� ���� �����ϰ� �׷��� ������ null

        // �Ҹ��� ����
        private bool isSelected = false;        // ������ ���� ���� ����

        // ������Ʈ
        private Button initialSlot;
        [SerializeField]
        private Button thisSlot;
        private EventSystem eventSystem;
        private TrackedDeviceGraphicRaycaster raycaster;

        // Ŭ���� ������Ʈ
        private PlayerSetting player;
        [SerializeField]
        private Inventory inventory;
        private NPC_Merchant tradeNPC;
        #endregion

        // �Ӽ�
        #region Properties
        public float PhantomDepth { get; set; }
        public int? SelectedSlotIndex => selectedSlotIndex;
        public bool IsSelected
        {
            get { return isSelected; }
            set { isSelected = value; }
        }
        public bool IsOpenInventory { get; set; }
        public bool IsOnTrade { get; set; }
        public Button ThisSlot => thisSlot;
        public Inventory Inventory => inventory;
        #endregion

        // ������ ����Ŭ
        #region Life Cycle
        private void Awake()
        {
            control = new();

            // �ʱ�ȭ
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
            // �κ��丮 ���� �̺�Ʈ �ڵ鷯 ����
            control.Player.Inventory.started += OnInventoryStarted;
            control.Player.CursorClick.started += OnCursorClickStarted;
            control.Player.CursorClick.canceled += OnCursorClickCanceled;

            // ������� �̺�Ʈ �ڵ鷯 ����
            control.UI.Cancel.started += OnCancelStarted;

            // ��Ʈ�� ���� Ȱ��ȭ
            control.Player.CursorClick.Enable();
            control.Player.Inventory.Enable();
            control.UI.Cancel.Enable();
        }

        private void OnDisable()
        {
            // ��Ʈ�� ���� ��Ȱ��ȭ
            control.UI.Cancel.Disable();
            control.Player.Inventory.Disable();
            control.Player.CursorClick.Disable();

            // ������� �̺�Ʈ �ڵ鷯 ���� ����
            control.UI.Cancel.started -= OnCancelStarted;

            // �κ��丮 ���� �̺�Ʈ �ڵ鷯 ���� ����
            control.Player.Inventory.started -= OnInventoryStarted;
            control.Player.CursorClick.started -= OnCursorClickStarted;
            control.Player.CursorClick.canceled -= OnCursorClickCanceled;
        }
        #endregion

        // �̺�Ʈ �ڵ鷯
        #region Event Handlers
        public void OnCancelStarted(InputAction.CallbackContext _)
        {
            if (IsOpenInventory) ShowItem(IsOpenInventory = false);
        }
        public void OnInventoryStarted(InputAction.CallbackContext _) => ShowItem(IsOpenInventory = !IsOpenInventory);
        public void OnCursorClickStarted(InputAction.CallbackContext _) => WhichSelect();

        public void OnCursorClickCanceled(InputAction.CallbackContext _) => isSelected = false;
        #endregion

        // �޼���
        #region Methods
        // ������ ������ ĸ��ȭ
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

        // NPC���� �ŷ� - ������ �Ǹ�
        public void UseInven(KeyValuePair<ItemKey, ItemValue> pair)
        {
            tradeNPC.TradeSetItem(pair.Key);
            DecreaseItem(pair);
        }

        public void ForTrade(bool isOpen) => ShowItem(IsOpenInventory = isOpen);

        // ������ ������� �κ��丮�� ��� �Ǹ� ȣ��Ǵ� �޼���
        public void EmptySignal()
        {
            //player.CursorUtility.CursorSwitch(false);
            ShowItem(IsOpenInventory = false);
        }

        // �κ��丮 ����
        public void ShowItem(bool isOpen)
        {
            float scaleX = isOpen ? 1.3f : 0;
            inventory.invenRect.localScale = new(scaleX, 1.3f, 1.3f);

            // NPC�� �ŷ� ���� ���� �����ϴ� ����
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

            //player.CursorUtility.CursorSwitch(isOpen);
        }

        // PlayerInteraction Ŭ�����κ��� ȹ���� �������� �κ��丮�� �����ϴ� �޼���
        public void AddItem(ItemKey itemKey, int count)
        {
            // ���� �κ��丮�� �ش� �������� ������
            if (inventory.invenDict.ContainsKey(itemKey))
            {
                // ������ �ø���
                inventory.invenDict[itemKey].Count(count);
                inventory.CountItem(ItemData(itemKey), inventory.invenDict[itemKey].itemIndex);
            }

            // ���� �κ��丮�� �ش� �������� ������
            else
            {
                // �� ������ �����ؼ� ������ �Ҵ�
                inventory.MakeSlot(itemKey);
            }
        }

        // �������� ����߸��� �޼���
        public void DropItem(KeyValuePair<ItemKey, ItemValue> selectedItem)
        {
            // ���� ��� �������� ������ ��
            Destroy(itemPhantom);

            // ��� �������� ��ġ�� ��¥ �������� �����ϰ�
            float yRot = UnityEngine.Random.Range(0, 360);
            Instantiate(selectedItem.Key.itemPrefab, itemPhantom.transform.position, Quaternion.Euler(0, yRot, 0));

            // ��� ������ ���� ������ ��� ����
            itemPhantom = null;

            // ���� ����
            DecreaseItem(selectedItem);
        }

        // ������ ���� ���� �޼���
        public void DecreaseItem(KeyValuePair<ItemKey, ItemValue> selectedItem)
        {
            // �������� ������ ���ҽ�Ų��
            if (inventory.invenDict.ContainsKey(selectedItem.Key))
                inventory.invenDict[selectedItem.Key].Count(-1);

            // �������� ���� ��� �����ϴٸ� ������ ���ҽ�Ű��
            if (inventory.invenDict[selectedItem.Key].IsUsable())
                inventory.CountItem(selectedItem, selectedItem.Value.itemIndex);

            // �� �̻� �������� ���ٸ� ���� ����
            else inventory.TakeSlot(selectedItem.Key);
        }

        #region SelectSlot
        // � ������ �����ߴ��� Ȯ���ϴ� �޼���
        private IEnumerator WhichSelect()
        {
            if (!player.rayInteractor.gameObject.activeSelf)
                yield break;

            if (IsOnTrade) yield break;

            // ������ ���� �÷��׸� true
            IsSelected = true;

            XR_Detect();
            StartCoroutine(FindInitialSlot(player.rayInteractor));
            while (initialSlot == null) yield return null;

            //StartCoroutine(DetectSlot(player.rayInteractor));
            //StartCoroutine(DetectSlotXR(player.rayInteractor));

            if (initialSlot != null)
            {
                // ������ �κ��丮�� �ִ� ���
                if (Array.Exists(inventory.invenSlots, slot => slot == initialSlot))
                {
                    selectedSlotIndex = Array.IndexOf(inventory.invenSlots, initialSlot);
                    SelectInven((int)selectedSlotIndex);
                    initialSlot = null;
                }

                selectedSlotIndex = null;
            }
            else
            {

                //StartCoroutine(DetectSlotXR(player.rayInteractor));
                StopCoroutine(Detect(player.rayInteractor));
                yield break;
            }
        }


        // WhichSelect���� �κ��丮�� Ȯ������ �� ȣ��Ǵ� �޼���
        private void SelectInven(int invenIndex)
        {
            // ������ �������� Ű�� ���� ����
            ItemKey selectedItemKey = null;

            // ������ �����۰� ��ġ�ϴ� Ű�� ã��
            selectedItemKey = CollectionUtility.FirstOrNull(inventory.invenDict.Keys, key => inventory.invenDict[key].itemIndex == invenIndex);
            if (selectedItemKey != null)
                // ��� ������ ����
                GenItemPhantom(selectedItemKey);
        }

        // ��� �������� �����ϴ� �޼���
        private void GenItemPhantom(ItemKey selectedItemKey)
        {
            // �ش� Ű�� ������ ������
            if (selectedItemKey == null) return;

            // ��� �������� �����Ͽ� ����� ��ġ�� �����ϴ� �ݺ��� ȣ��
            itemPhantom = Instantiate(selectedItemKey.itemPhantomPrefab, player.rayInteractor.transform.position, Quaternion.identity);
            phantomCor = PhantomUpdate(selectedItemKey);
            StartCoroutine(phantomCor);
        }

        // ���� ���Կ� �������� �ξ��ٸ� ���
        private void SameSelect(Button thisSlot)
        {
            // ���� �κ��丮���� ���ڸ� ��ư�̶�� �������� ����ϵ��� �ٲ۴�
            if (initialSlot == thisSlot)
            {
                // �ش� ������ �������� ã��
                var itemKey = CollectionUtility.FirstOrNull(inventory.invenDict.Keys,
                    key => inventory.invenDict[key].itemIndex == Array.IndexOf(inventory.invenSlots, thisSlot));

                StopCoroutine(phantomCor);  // ��� ������ ����ȭ �ڷ�ƾ�� ���� �����ϰ�
                Destroy(itemPhantom);       // ��� �������� ������ ��
                itemPhantom = null;
                phantomCor = null;
                thisSlot = null;

                *//*if (itemKey != null)
                    player.PlayerUse.UseItem(ItemData(itemKey));   // ���*//*
            }

            else return;
        }
        #endregion
        #endregion

        // ��Ÿ ��ƿ��Ƽ
        #region Utilities
        // ��� �������� ��Ƶδ� �ݺ���
        public IEnumerator PhantomUpdate(ItemKey selectedItemKey)
        {
            while (IsSelected)
            {
                // cursorUtility�� �̺�Ʈ �ڵ鷯�� ���� Vector2 CursorPosition�� Vector3 ������ �Է�
                Vector3 mousePosition = player.rayInteractor.transform.position;

                // ��� �������� transform.position�� �ش� ��ǥ�� ����ȭ
                itemPhantom.transform.position = mousePosition;

                // �� �ݺ��⸦ �� ������ ����
                yield return null;
            }

            // �ٸ� ���Կ� �ξ��ٸ� ����
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
                phantomCor = null;
                yield break;
            }

            // �÷��̾ ������ ��ġ���� �巡�׸� �����ϸ� �ش� �������� ��üȭ
            DropItem(ItemData(selectedItemKey));

            // �� �ݺ��⸦ ����ϴ� ������ ����
            phantomCor = null;

            // �۵� ���� �ڷ�ƾ ����
            yield break;
        }

        public IEnumerator FindInitialSlot(XRRayInteractor rayInteractor)
        {
            int count = 0;

            // �ٽ� Ŭ���� ������ ��� �ݺ�
            while (!initialSlot || count > 10)
            {
                if (rayInteractor.TryGetCurrentUIRaycastResult(out var re))
                {
                    //Button UI ȹ���� �õ��غ��� ������ ����
                    if (ComponentUtility.TryGetComponentInChildren<Button>(re.gameObject.transform, out var slot))
                    {
                        initialSlot = slot;
                    }
                }
                count++;
                // �� �ݺ��⸦ �� ������ �ݺ��ϰ�
                yield return null;
            }
            // ������ ������ ����
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
            //if (IsSelected)
            //    return;


            StartCoroutine(WhichSelect());
        }

        void XR_Detect()
        {

            if (player.rayInteractor == null || eventSystem == null)
            {
                Debug.LogWarning("XRRayInteractor �Ǵ� EventSystem�� �������� �ʾҽ��ϴ�.");
                return;
            }

            StartCoroutine(Detect(player.rayInteractor));
        }

        public IEnumerator Detect(XRRayInteractor rayInteractor)
        {

            // �׸� ������� GraphicRaycaster ����
            List<RaycastResult> results = new();

            // �ٽ� Ŭ���� ������ ��� �ݺ�
            while (IsSelected)
            {
                if (rayInteractor.TryGetCurrentUIRaycastResult(out var re))
                {
                    results.Add(re);
                    thisSlot = null;

                    foreach (var result in results)
                    {
                        //Button UI ȹ���� �õ��غ��� ������ ����
                        if (ComponentUtility.TryGetComponentInChildren<Button>(result.gameObject.transform, out var slot))
                        {
                            thisSlot = slot;
                        }
                    }
                }

                // �� �ݺ��⸦ �� ������ �ݺ��ϰ�
                yield return null;
            }

            thisSlot = null;
            results.Clear();

            // ������ ������ ����
            yield break;
        }
        #endregion

        #region Dummy_Seti
        *//*// ������ �����ϴ� �ݺ��� (XR ����)
        public IEnumerator DetectSlot(XRRayInteractor rayInteractor)
        {
            // �ٽ� Ŭ���� ������ ��� �ݺ�
            while (IsSelected)
            {
                // ���� ���ͷ��Ͱ� UI�� ��Ʈ�� ���ߴ��� Ȯ��
                if (rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
                {
                    // ��Ʈ ������ ���� ��ǥ�� ��ũ�� ��ǥ�� ��ȯ
                    Vector3 screenPosition = Camera.main.WorldToScreenPoint(hit.point);

                    // EventSystem���κ��� ������ ������ ����
                    PointerEventData pointerData = new(EventSystem.current)
                    {
                        position = (Vector2)screenPosition
                    };

                    // �׸� ������� GraphicRaycaster ����
                    List<RaycastResult> results = new();
                    raycaster.Raycast(pointerData, results);

                    Debug.Log(results);

                    // ������ ������ ����
                    thisSlot = null;
                    foreach (RaycastResult result in results)
                    {
                        *//*// ������ UGUI�� �ؽ�Ʈ�ڽ���� ����
                        if (result.gameObject.GetComponent<TextMeshProUGUI>())
                            continue;

                        // Button UI ȹ���� �õ��غ��� ������ ����
                        if (result.gameObject.TryGetComponent<Button>(out var slot))
                            thisSlot = slot;*//*

                        if (ComponentUtility.TryGetComponentInChildren<Button>(result.gameObject.transform, out var slot))
                            thisSlot = slot;
                    }
                }

                // �� �ݺ��⸦ �� ������ �ݺ��ϰ�
                yield return null;
            }

            // ������ ������ ����
            yield break;
        }

        // ������ �����ϴ� �ݺ��� (PC ����)
        public IEnumerator DetectSlot()
        {
            // �ٽ� Ŭ���� ������ ��� �ݺ�
            while (IsSelected)
            {
                // EventSystem���κ��� ������ ������ �ް�
                PointerEventData pointerData = new(eventSystem)
                {
                    position = Mouse.current.position.ReadValue()

                };

                // �׸� ������� GraphicRaycaster ����
                List<RaycastResult> results = new();

                raycaster.Raycast(pointerData, results);

                // ������ ������ ����
                thisSlot = null;
                foreach (RaycastResult result in results)
                {
                    // ������ UGUI�� �ؽ�Ʈ�ڽ���� ����
                    if (result.gameObject.GetComponent<TextMeshProUGUI>())
                        continue;

                    // Button UI ȹ���� �õ��غ��� ������ ����
                    if (result.gameObject.TryGetComponent<Button>(out var slot))
                        thisSlot = slot;

                }


                // �� �ݺ��⸦ �� ������ �ݺ��ϰ�
                yield return null;
            }

            // ������ ������ ����
            yield break;
        }*//*
        #endregion
    }
}*/