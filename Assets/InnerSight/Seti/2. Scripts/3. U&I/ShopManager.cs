using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace InnerSight_Seti
{
    /// <summary>
    /// ���� UI ���� ü���� �θ� Ŭ����
    /// </summary>
    public abstract class ShopManager : MonoBehaviour
    {
        // �ʵ�
        #region Variables
        protected GameObject shopUI;
        protected GameObject confirmUI;
        protected GameObject completeUI;
        protected TextMeshProUGUI goldText;
        protected TextMeshProUGUI completeText;
        protected IEnumerator tradeCor;

        protected Button[] shopSlots;
        protected TextMeshProUGUI[] shopCosts;
        protected KeyValuePair<ItemKey, ItemValueShop> selectItem;
        protected Dictionary<ItemKey, ItemValueShop> shopDict = new();

        // Ŭ����
        protected PlayerSetting player;

        protected bool CanTrade = false;
        #endregion

        // �Ӽ�
        #region
        public bool OnTrade { get; set; }
        #endregion

        // ������ ����Ŭ
        #region Life Cycle
        protected virtual void Awake()
        {
            // UI
            shopUI = transform.GetChild(0).GetChild(0).gameObject;

            // Gold
            goldText = shopUI.transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>();

            // ��ư
            shopSlots = shopUI.transform.GetChild(1).GetComponentsInChildren<Button>();
            shopCosts = shopUI.transform.GetChild(1).GetComponentsInChildren<TextMeshProUGUI>();

            // �Ϸ�
            completeUI = transform.GetChild(0).GetChild(1).gameObject;
            completeText = completeUI.GetComponentInChildren<TextMeshProUGUI>();
        }

        protected virtual void Update()
        {
            if (shopUI.activeSelf)
            {
                transform.GetChild(0).LookAt(Camera.main.transform);

                if (!CanTrade)
                {
                    UIReset();
                }
            }
        }
        #endregion

        // �޼���
        #region Methods
        // ���� UI ����ġ
        public void SwitchUI(bool isOpen)
        {
            UIReset();
            OnTrade = isOpen;
            shopUI.SetActive(isOpen);
            goldText.text = PlayerStats.Instance.CurrentGold.ToString() + "G";
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

        protected virtual void UIReset()
        {
            confirmUI.SetActive(false);
            shopUI.SetActive(false);
            OnTrade = false;
        }
        #endregion

        // �߻� �޼���
        #region Abstract
        public abstract void Confirm();
        protected abstract void SelectSlot(KeyValuePair<ItemKey, ItemValueShop> pair);
        protected abstract void AssignCosts();
        #endregion

        // ��Ÿ ��ƿ��Ƽ
        #region Utilities
        // ���� �ؽ�Ʈ
        protected IEnumerator TradeComplete(string text)
        {
            completeUI.SetActive(true);
            completeText.text = text;
            yield return new WaitForSeconds(2);

            completeUI.SetActive(false);
            tradeCor = null;
            yield break;
        }

        // �� ��ư�� �ε����� �´� �����ʸ� �ڵ����� �Ҵ��ϴ� �޼���
        protected void AssignSlots()
        {
            foreach (var pair in shopDict)
            {
                int index = pair.Value.itemIndex;  // �� ������ �ݵ�� ���� ��������� ���� �Լ� �ȿ��� �ùٸ��� �۵�

                shopSlots[index].onClick.RemoveAllListeners();
                shopSlots[index].onClick.AddListener(() => SelectSlot(pair));
            }
        }

        public void DistanceCheck(bool CanTrade)
        {
            this.CanTrade = CanTrade;
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