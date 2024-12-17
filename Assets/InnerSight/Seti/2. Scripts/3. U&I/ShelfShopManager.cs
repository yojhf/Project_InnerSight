using System.Collections.Generic;

namespace InnerSight_Seti
{
    /// <summary>
    /// ���� ���� ���� UI
    /// </summary>
    public class ShelfShopManager : ShopManager
    {
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
            int howMuch = selectItem.Value.itemCost;

            // ����
            if (PlayerStats.Instance.SpendGold(howMuch))
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
                tradeCor = TradeComplete("You don't have enough money");
            }

            StartCoroutine(tradeCor);
        }
        #endregion

        // ��Ÿ ��ƿ��Ƽ
        #region Utilities
        // ��ư ����
        protected override void SelectSlot(KeyValuePair<ItemKey, ItemValueShop> pair)
        {
            selectItem = pair;
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
        }
        #endregion
    }
}