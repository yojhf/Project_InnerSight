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
        private List<Item> shopList = new();
        private TextMeshProUGUI[] shopCosts;
        #endregion

        // �Ӽ�
        #region Properties
        public bool OnTrade { get; protected set; }
        #endregion

        // ������ ����Ŭ
        #region Life Cycle
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
            OnTrade = isOpen;
            shopUI.SetActive(isOpen);
            goldText.text = PlayerStats.Instance.CurrentGold.ToString() + "G";
        }

        // ���� �� ���� UI
        public void Confirm()
        {
            tradeCor = TradeComplete("�������� �����մϴ�.");
            StartCoroutine(tradeCor);
            SwitchUI(false);
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
            
        }

        // �� ��ư�� �ε����� �´� �����ʸ� �ڵ����� �Ҵ��ϴ� �޼���
        private void AssignSlots()
        {
            /*foreach (var pair in shopList)
            {
                int index = pair.Value.itemIndex;  // �� ������ �ݵ�� ���� ��������� ���� �Լ� �ȿ��� �ùٸ��� �۵�

                shopSlots[index].onClick.RemoveAllListeners();
                shopSlots[index].onClick.AddListener(() => SelectSlot(pair));
            }*/
        }
        #endregion
    }
}