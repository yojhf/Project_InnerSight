using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace InnerSight_Seti
{
    /// <summary>
    /// ���� NPC ���� UI
    /// </summary>
    public class ElixirShopManager : MonoBehaviour
    {
        // �ʵ�
        #region Variables
        private int itemCount;
        private GameObject shopUI;
        private GameObject countUI; //
        private GameObject confirmUI;   //
        private GameObject completeUI;   //
        private TextMeshProUGUI goldText;
        private TextMeshProUGUI countText;  //
        private TextMeshProUGUI completeText;  //
        private IEnumerator tradeCor;

        private Button[] shopSlots;
        private TextMeshProUGUI[] shopCosts;
        private Dictionary<ItemKey, ItemValueShop> shopDict = new();
        private KeyValuePair<ItemKey, ItemValueShop> selectItem;

        // Ŭ����
        private PlayerSetting player;
        #endregion

        // �Ӽ�
        #region Properties
        public bool OnTrade { get; protected set; }
        #endregion

        // ������ ����Ŭ
        #region Life Cycle
        private void Start()
        {
            Codex_Recipe_Manager.Instance.SetCodexToShop(this);
        }

        private void Awake()
        {
            // UI ����
            shopUI = transform.GetChild(0).GetChild(0).gameObject;
            
            // ���
            goldText = shopUI.transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>();
            
            // ���� �迭
            shopSlots = shopUI.transform.GetChild(1).GetComponentsInChildren<Button>();
            shopCosts = shopUI.transform.GetChild(1).GetComponentsInChildren<TextMeshProUGUI>();

            // ī��Ʈ
            countUI = shopUI.transform.GetChild(2).gameObject;
            countText = countUI.transform.GetChild(0).GetChild(0).GetComponentInChildren<TextMeshProUGUI>();

            // ����
            confirmUI = shopUI.transform.GetChild(3).gameObject;

            // �Ϸ�
            completeUI = transform.GetChild(0).GetChild(1).gameObject;
            completeText = completeUI.GetComponentInChildren<TextMeshProUGUI>();
        }
        #endregion

        // �޼���
        #region Methods
        // ���� ������ ����
        public void GetKnowhow(ItemKey itemKey)
        {
            shopDict[itemKey].itemKnowhow = true;
            AssignCosts();
        }

        // ���� UI ����ġ
        public void SwitchUI(bool isOpen)
        {
            OnTrade = isOpen;
            shopUI.SetActive(isOpen);
            goldText.text = PlayerStats.Instance.CurrentGold.ToString() + "G";
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

        public void Confirm_Yes()
        {
            Confirm();
        }
        public void Confirm_No()
        {
            confirmUI.SetActive(false);
        }

        // ���� �� ���� UI
        public void Confirm()
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
                    player.PlayerUse.inventoryManager.AddItem(selectItem.Key, itemCount);
                }
                else
                {
                    player.PlayerUse.inventoryManager.AddItem(selectItem.Key, 1);
                    GetKnowhow(selectItem.Key);
                }
                tradeCor = TradeComplete("Purchase complete");
                SwitchUI(false);
            }
            else
            {
                tradeCor = TradeComplete("You don't have enough money");
                confirmUI.SetActive(false);
            }

            StartCoroutine(tradeCor);
            itemCount = 0;
        }
        #endregion

        // ��Ÿ ��ƿ��Ƽ
        #region Utilities
        // ���� �ؽ�Ʈ
        IEnumerator TradeComplete(string text)
        {
            completeUI.SetActive(true);
            completeText.text = text;
            yield return new WaitForSeconds(2);

            completeUI.SetActive(false);
            tradeCor = null;
            yield break;
        }

        // �ŷ� ���� Ȯ�� ��ư
        /*public void ConfirmComplete()
        {
            if (tradeCor != null)
            {
                StopCoroutine(tradeCor);
                tradeCor = null;
            }
            completeUI.SetActive(false);
        }*/

        // ��ư ����
        private void SelectSlot(KeyValuePair<ItemKey, ItemValueShop> pair)
        {
            selectItem = pair;
            if (pair.Value.itemKnowhow)
                SwitchCount(true);
            else Confirm();
        }

        // �� ��ư�� �ε����� �´� �����ʸ� �ڵ����� �Ҵ��ϴ� �޼���
        private void AssignSlots()
        {
            foreach (var pair in shopDict)
            {
                int index = pair.Value.itemIndex;  // �� ������ �ݵ�� ���� ��������� ���� �Լ� �ȿ��� �ùٸ��� �۵�

                shopSlots[index].onClick.RemoveAllListeners();
                shopSlots[index].onClick.AddListener(() => SelectSlot(pair));
            }
        }

        // �� �ؽ�Ʈ�� �´� �ڽ�Ʈ�� �ڵ����� �����ϴ� �޼���
        private void AssignCosts()
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

        // ������ ���� �ʱ�ȭ
        public void SetItemInfo(Dictionary<ItemKey, ItemValueShop> shopDict)
        {
            this.shopDict = shopDict;
            AssignSlots();
            AssignCosts();
        }
        #endregion
    }
}