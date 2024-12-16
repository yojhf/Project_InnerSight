using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

namespace InnerSight_Seti
{
    // 플레이어의 아이템 사용을 관장하는 클래스
    public class PlayerUse : MonoBehaviour
    {
        // 필드
        #region Variables
        // 인벤토리 클래스
        [SerializeField]
        private InventoryManager inventoryManager;

        // 퀵슬롯
        private Button[] quickSlots;
        #endregion

        // 속성
        #region Properties
        public InventoryManager InventoryManager => inventoryManager;
        #endregion

        // 이벤트 핸들러
        #region Event Handlers
        public void OnQuickSlot1DownStarted(InputAction.CallbackContext _) => quickSlots[0].onClick.Invoke();
        public void OnQuickSlot2DownStarted(InputAction.CallbackContext _) => quickSlots[1].onClick.Invoke();
        public void OnQuickSlot3DownStarted(InputAction.CallbackContext _) => quickSlots[2].onClick.Invoke();
        #endregion

        private void Awake()
        {
            //inventoryManager = GetComponent<InventoryManager>();
        }

        // 메서드
        #region Methods
        // 인벤토리 세팅
        public void SetInventory(InventoryManager inventoryManager)
        {
            this.inventoryManager = inventoryManager;
        }

        /*// 아이템을 사용하는 메서드
        public void UseItem(KeyValuePair<ItemKey, ItemValue> pair)
        {
            Player player = GetComponent<Player>();

            // 인벤토리에 아이템이 없다면 메서드 종료
            if (!inventoryManager.Inventory.invenDict.ContainsKey(pair.Key)) return;

            // 우선 입력된 아이템 정보를 찾고
            ItemKey thisItem = CollectionUtility.FirstOrDefault(inventoryManager.Inventory.invenDict, kvp => pair.Key == kvp.Key).Key;

            // 해당 아이템이 사용 가능한지 여부를 확인한 뒤
            if (thisItem.itemPrefab.TryGetComponent<IUsable>(out var usableItem))
            {
                if (usableItem.CanUse(player))
                {
                    // 아이템을 사용
                    usableItem.UseItem();

                    // 수량 갱신
                    inventoryManager.DecreaseItem(pair);
                }
            }

            // 사용할 수 없는 아이템이라면 메서드 종료
            else return;
        }*/
        #endregion
    }
}