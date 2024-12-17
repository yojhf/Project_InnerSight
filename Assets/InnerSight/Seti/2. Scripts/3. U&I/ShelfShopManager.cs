using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace InnerSight_Seti
{
    /// <summary>
    /// ���� ���� ���� UI
    /// </summary>
    public class ShelfShopManager : MonoBehaviour
    {
        // �ʵ�
        #region Variables
        private GameObject shopUI;
        private GameObject confirmUI;   //
        private GameObject completeUI;   //
        private TextMeshProUGUI goldText;
        private TextMeshProUGUI completeText;  //
        private IEnumerator tradeCor;

        private Button[] shopSlots;
        private TextMeshProUGUI[] shopCosts;
        private KeyValuePair<ItemKey, ItemValueShop> selectItem;
        private Dictionary<ItemKey, ItemValueShop> shopDict = new();

        private PlayerSetting player;
        private Transform cameraOffset;
        #endregion

        // �Ӽ�
        #region Properties
        public bool OnTrade { get; protected set; }
        #endregion

        // ������ ����Ŭ
        #region Life Cycle
        private void Start()
        {
            //cameraOffset = player.transform.GetChild(0);
        }

        private void Update()
        {
            if (shopUI.activeSelf)
            {
                transform.GetChild(0).LookAt(Camera.main.transform);
            }
            
        }

        private void Awake()
        {
            // UI
            shopUI = transform.GetChild(0).GetChild(0).gameObject;

            // Gold
            goldText = shopUI.transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>();

            // ��ư
            shopSlots = shopUI.transform.GetChild(1).GetComponentsInChildren<Button>();
            shopCosts = shopUI.transform.GetChild(1).GetComponentsInChildren<TextMeshProUGUI>();

            // ����
            confirmUI = shopUI.transform.GetChild(2).gameObject;

            // �Ϸ�
            completeUI = transform.GetChild(0).GetChild(1).gameObject;
            completeText = completeUI.GetComponentInChildren<TextMeshProUGUI>();
        }
        #endregion

        // �޼���
        #region Methods
        // ���� UI ����ġ
        public void SwitchUI(bool isOpen)
        {
            OnTrade = isOpen;
            shopUI.SetActive(isOpen);
            goldText.text = PlayerStats.Instance.CurrentGold.ToString() + "G";
        }

        public void DefineTrade()
        {
            confirmUI.SetActive(true);
        }

        public void Confirm_Yes()
        {
            Confirm();
            confirmUI.SetActive(false);
        }
        public void Confirm_No()
        {
            confirmUI.SetActive(false);
        }

        // ���� �� ���� UI
        public void Confirm()
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

        // ��ư ����
        private void SelectSlot(KeyValuePair<ItemKey, ItemValueShop> pair)
        {
            selectItem = pair;
            DefineTrade();
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
                int cost = pair.Value.itemCost;
                int index = pair.Value.itemIndex;
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

        public void SetPlayer(PlayerSetting player)
        {
            this.player = player;
        }
        #endregion
    }
}