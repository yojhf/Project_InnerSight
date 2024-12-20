using System.Collections.Generic;
using UnityEngine;
using InnerSight_Kys;

namespace InnerSight_Seti
{
    /// <summary>
    /// ���� ���� ���� UI
    /// </summary>
    public class ShelfShopManager : ShopManager
    {
        [SerializeField]
        private GameObject AutoLoot;
        [SerializeField]
        private int cost_AutoLoot = 5000;
        private int thisIndex;

        // ������ ����Ŭ
        #region Life Cycle
        protected override void Awake()
        {
            base.Awake();

            // ����
            confirmUI = shopUI.transform.GetChild(2).gameObject;
        }
        #endregion

        // �޼���
        #region Methods
        public void DefineTrade()
        {
            confirmUI.SetActive(true);
        }

        // ���� �� ���� UI
        public override void Confirm()
        {
            // ���� ����
            int howMuch;
            if (thisIndex < shopDict.Count)
                howMuch = selectItem.Value.itemCost;
            else howMuch = cost_AutoLoot;

            // ����
            if (PlayerStats.Instance.SpendGold(howMuch))
            {
                AudioManager.Instance.Play("ElementSucceed");
                if (thisIndex < shopDict.Count)
                {
                    // ���� Ȱ��ȭ
                    Item thisShelf = selectItem.Key.itemPrefab.GetComponent<Item>();
                    Noah.ShelfManager.Instance.ActiveShelf(thisShelf);

                    shopSlots[selectItem.Value.itemIndex].interactable = false;
                    tradeCor = TradeComplete("Purchase complete");
                    SwitchUI(false);
                }
                else
                {
                    shopSlots[shopDict.Count].interactable = false;
                    tradeCor = TradeComplete("Purchase complete");
                    PlayerStats.Instance.OnAuto();
                    AutoLoot.SetActive(true);
                    SwitchUI(false);
                }
            }
            else
            {
                tradeCor = TradeComplete("You don't have enough money");
            }

            StartCoroutine(tradeCor);
            selectItem = default;
        }
        #endregion

        // ��Ÿ ��ƿ��Ƽ
        #region Utilities
        // ��ư ����
        protected override void SelectSlot(KeyValuePair<ItemKey, ItemValueShop> pair)
        {
            selectItem = pair;
            thisIndex = pair.Value.itemIndex;
            DefineTrade();
        }

        public void SelectSlot()
        {
            thisIndex = shopDict.Count;
            DefineTrade();
        }

        // �� �ؽ�Ʈ�� �´� �ڽ�Ʈ�� �ڵ����� �����ϴ� �޼���
        protected override void AssignCosts()
        {
            foreach (var pair in shopDict)
            {
                int cost = pair.Value.itemCost;
                int index = pair.Value.itemIndex;
                shopCosts[index].text = cost.ToString() + "G";
            }

            shopCosts[shopDict.Count].text = cost_AutoLoot.ToString() + "G";
        }
        #endregion
    }
}