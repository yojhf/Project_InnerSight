using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

namespace InnerSight_Seti
{
    /// <summary>
    /// ���� NPC ���� UI
    /// </summary>
    public class ShopUI : MonoBehaviour
    {
        // �ʵ�
        #region Variables
        private GameObject shopUI;
        private TextMeshProUGUI goldText;

        private Button[] shopSlots;
        private TextMeshProUGUI[] shopCosts;
        private Dictionary<ItemKey, ItemValueShop> shopDict = new();
        #endregion

        // ������ ����Ŭ
        #region Life Cycle
        private void Start()
        {
            Codex_Recipe_Manager.Instance.SetCodexToShop(this);
        }

        private void Awake()
        {
            shopUI = transform.GetChild(0).GetChild(0).gameObject;
            goldText = shopUI.transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>();
            shopSlots = shopUI.transform.GetChild(1).GetComponentsInChildren<Button>();
            shopCosts = shopUI.transform.GetChild(1).GetComponentsInChildren<TextMeshProUGUI>();
        }
        #endregion

        // �޼���
        #region Methods
        // ���� UI ����ġ
        public void SwitchUI(bool isOpen)
        {
            shopUI.SetActive(isOpen);
            goldText.text = PlayerStats.Instance.CurrentGold.ToString() + " G";
        }

        // ������ ���� �ʱ�ȭ
        public void SetItemInfo(Dictionary<ItemKey, ItemValueShop> shopDict)
        {
            this.shopDict = shopDict;
            AssignSlots();
            AssignCosts();
        }

        // ���� ������ ����
        public void GetKnowhow(ItemKey itemKey)
        {
            shopDict[itemKey].itemKnowhow = true;
            AssignCosts();
        }
        #endregion

        // ��Ÿ ��ƿ��Ƽ
        #region Utilities
        // ��ư ����
        private void SelectSlot(KeyValuePair<ItemKey, ItemValueShop> pair)
        {
            if (pair.Value.itemKnowhow)
            {
                Debug.Log(pair.Key.itemName);

                // ���Ͽ찡 �˷��� �������� ���� �ŷ�
                // �� ��?
                // �ϰ� ����
            }
            else
            {
                Debug.Log(pair.Key.itemName);

                // ���Ͽ찡 �˷����� ���� �������� ���� �ŷ�
                // �� ���� ������ Trade Confirm â ��� �� - ��δϱ�
                // �ŷ��� ����Ǹ� ���� ����
                //AssignCosts();
            }
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
        #endregion
    }
}