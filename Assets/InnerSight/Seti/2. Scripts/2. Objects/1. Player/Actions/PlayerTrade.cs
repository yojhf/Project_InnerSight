using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InnerSight_Seti
{
    // NPC���� �ŷ� ����� �Ѱ��ϴ� ��� Ŭ����
    public class PlayerTrade : MonoBehaviour
    {
        // �ʵ�
        #region Variables
        // ��Ȯ�� �������� Ȯ���ϴ� ��ɿ� �ʵ�
        private readonly List<ItemForTrade> tradableItems = new();

        // Ŭ���� ������Ʈ
        private Player player;
        private ShopBag shopBag;
        #endregion

        // �Ӽ�
        #region Properties
        public Player Player => player;
        public ShopBag ShopBag => shopBag;
        #endregion

        // ������ ����Ŭ
        #region Life Cycle
        private void Start()
        {
            player = GetComponent<Player>();
        }
        #endregion

        // �̺�Ʈ �ڵ鷯
        #region Event Handlers
        // �׼��� ���ϸ� �������� ��ٱ��Ͽ� ��´�
        public void OnLeftCursorClickStarted(InputAction.CallbackContext _)
        {
            CallNPC();
            ChooseItem();
        }

        public void OnRightCursorClickStarted(InputAction.CallbackContext _)
        {

        }
        #endregion

        // �޼���
        #region Methods
        // ShopBag ���� �޼���
        #region Set ShopBag
        public void GetShopBag()
        {
            shopBag = player.PlayerUse.InventoryManager.GetComponentInChildren<ShopBag>();

            if (shopBag.PlayerTrade == null)
            {
                shopBag.SetPlayer(this);
                shopBag.transform.GetChild(0).gameObject.SetActive(true);
            }
        }

        public void TakeOffShopBag()
        {
            if (shopBag.PlayerTrade != null)
            {
                if (shopBag.shopDict.Count != 0)
                {
                    foreach (var item in shopBag.shopItems)
                    {
                        Destroy(item);
                    }
                    shopBag.shopItems.Clear();
                    shopBag.shopDict.Clear();
                }

                shopBag.transform.GetChild(0).gameObject.SetActive(false);
                shopBag.SetPlayer(null);
            }
        }
        #endregion

        // PlayerInteraction�� �����ϴ� Ʈ���� �޼������ �����
        #region CheckShop
        public void CheckInShop(Collider other)
        {
            if (other.transform.TryGetComponent<ItemForTrade>(out var tradableItem))
            {
                if (!tradableItems.Contains(tradableItem))
                    tradableItems.Add(tradableItem);

                tradableItem.MarkItem();
            }

            if (other.transform.CompareTag("Shop"))
            {
                SearchForTrade();
            }
        }
        public void CheckOutShop(Collider other)
        {
            if (other.transform.TryGetComponent<ItemForTrade>(out var tradableItem))
            {
                tradableItem.ForgetItem();

                if (tradableItems.Contains(tradableItem))
                    tradableItems.Remove(tradableItem);
            }
        }
        #endregion

        // NPC�� �����ϴ� �޼���
        private void CallNPC()
        {
            if (player.PlayerUse.InventoryManager.ThisSlot != null) return;

            if (player.CursorUtility.CursorSelect() == null) return;

            if (player.CursorUtility.CursorSelect().
                TryGetComponent<NPC_Merchant>(out var merchant))
            {
                merchant.Trade(player);
            }
        }

        // �������� �����ϴ� �޼���
        private void ChooseItem()
        {
            if (player.CursorUtility.CursorSelect() == null) return;

            // ���� ������ �������� ǥ��
            if (player.CursorUtility.CursorSelect().
                TryGetComponent<ItemForTrade>(out var tradableItem))
            {
                if (!tradableItem.IsIdentify) return;

                ItemKey shopItem = tradableItem.GetItemData();
                AddItem(shopItem);
            }
        }

        // NPC ������ �Ŵ뿡 ������ �������� ��ٱ��Ͽ� �����ϴ� �޼���
        private void AddItem(ItemKey itemKey)
        {
            // ���� ��ٱ��Ͽ� �ش� �������� ������
            if (shopBag.shopDict.ContainsKey(itemKey))
            {
                // ������ �ø���
                shopBag.shopDict[itemKey].Count(1);
                shopBag.CountItem(itemKey, shopBag.shopDict[itemKey].itemIndex);
            }

            // ���� ��ٱ��Ͽ� �ش� �������� ������
            else
            {
                // ��ٱ��Ͽ� ���� �������� itemValue�� �ν��Ͻ��� �����
                ItemValue itemValue = new();

                // �� ������ �����ؼ� ������ �Ҵ�
                shopBag.MakeSlot(itemKey, itemValue);
            }
        }
        #endregion

        // ��Ÿ ��ƿ��Ƽ
        #region Utilities
        // NPC ���� ���� ��Ȯ�� �������� Ȯ���ϴ� �񵿱� �ݺ���
        private void SearchForTrade()
        {
            Transform head = player.transform.Find("Head_Root");

            while (tradableItems.Count != 0)
            {
                if (player == null) continue;

                if (Physics.Raycast(head.position, head.forward, out RaycastHit hit, 7.5f))
                {
                    if (hit.transform.TryGetComponent<ItemForTrade>(out var tradableItem))
                    {
                        tradableItem.DefineItem();
                        tradableItems.Remove(tradableItem);
                    }
                }
            }
        }
        #endregion
    }
}