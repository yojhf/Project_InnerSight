using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace InnerSight_Seti
{
    /// <summary>
    /// ���� NPC ���� UI
    /// </summary>
    public class ElixirShopManager : ShopManager
    {
        // �ʵ�
        #region Variables
        private int itemCount;
        private GameObject countUI;
        private TextMeshProUGUI countText;
        #endregion

        // ������ ����Ŭ
        #region Life Cycle
        private void Start()
        {
            Codex_Recipe_Manager.Instance.SetCodexToShop(this);
        }

        protected override void Awake()
        {
            base.Awake();

            // ī��Ʈ
            countUI = shopUI.transform.GetChild(2).gameObject;
            countText = countUI.transform.GetChild(0).GetChild(0).GetComponentInChildren<TextMeshProUGUI>();

            // ����
            confirmUI = shopUI.transform.GetChild(3).gameObject;
        }
        #endregion

        // �޼���
        #region Methods
        // ���� ������ ����
        public void GetKnowhow(ItemKey itemKey)
        {
            Codex_Recipe_Manager.Instance.IdentifyRecipe(itemKey);
            shopDict[itemKey].itemKnowhow = true;
            AssignCosts();
        }

        // ������ ī���� ON
        private void SwitchCount(bool isOpen)
        {
            itemCount = 1;
            countUI.SetActive(isOpen);
            countText.text = itemCount.ToString();
        }

        public void CountUp()
        {
            itemCount++;
            if (itemCount > 99) itemCount -= 99;
            countText.text = itemCount.ToString();
        }

        public void CountDown()
        {
            itemCount--;
            if (itemCount < 1) itemCount += 99;
            countText.text = itemCount.ToString();
        }

        public void DefineCount()
        {
            countUI.SetActive(false);
            confirmUI.SetActive(true);
        }

        // ���� �� ���� UI
        public override void Confirm()
        {
            // ���� ����
            int howMuch;
            if (selectItem.Value.itemKnowhow)
            {
                howMuch = selectItem.Value.itemCost * itemCount;
            }
            else
            {
                howMuch = selectItem.Value.itemCost_Knowhow;
            }

            // ����
            if (PlayerStats.Instance.SpendGold(howMuch))
            {
                if (selectItem.Value.itemKnowhow)
                {
                    player.PlayerUse.InventoryManager.AddItem(selectItem.Key, itemCount);
                }
                else
                {
                    player.PlayerUse.InventoryManager.AddItem(selectItem.Key, 1);
                    GetKnowhow(selectItem.Key);
                }
                tradeCor = TradeComplete("Purchase complete");
                SwitchUI(false);
            }
            else
            {
                tradeCor = TradeComplete("You don't have enough money");
            }

            StartCoroutine(tradeCor);
            itemCount = 0;
        }

        protected override void UIReset()
        {
            SwitchCount(false);
            base.UIReset();
        }
        #endregion

        // ��Ÿ ��ƿ��Ƽ
        #region Utilities
        // ��ư ����
        protected override void SelectSlot(KeyValuePair<ItemKey, ItemValueShop> pair)
        {
            selectItem = pair;
            if (pair.Value.itemKnowhow)
                SwitchCount(true);
            else Confirm();
        }

        // �� �ؽ�Ʈ�� �´� �ڽ�Ʈ�� �ڵ����� �����ϴ� �޼���
        protected override void AssignCosts()
        {
            foreach (var pair in shopDict)
            {
                int cost;
                int index = pair.Value.itemIndex;
                if (pair.Value.itemKnowhow)
                {
                    cost = pair.Value.itemCost;
                }
                else
                {
                    cost = pair.Value.itemCost_Knowhow;
                }
                shopCosts[index].text = cost.ToString() + " G";
            }
        }
        #endregion
    }
}