using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace InnerSight_Seti
{
    /// <summary>
    /// ���� ���� ������ �����ϴ� Ŭ����
    /// </summary>
    public class Cost_Random : Singleton<Cost_Random>
    {
        // �ʵ�
        #region Variables
        // �Ϲ� �ʵ�
        private const int identifier = 4000;    // �������� �д´�
        private List<int> elixirsIndex = new(); // �������� �ε���
        private List<int> elixirsPrice = new(); // �������� ���� ����
        private List<TextMeshProUGUI> elixirsText = new();

        // ������ �����ͺ��̽�
        [SerializeField]
        private ItemDatabase itemDatabase;
        [SerializeField]
        private GameObject priceUI;
        #endregion

        // ������ ����Ŭ
        #region Life Cycle
        private void Start()
        {
            SetElixirs();
            elixirsText.AddRange(priceUI.GetComponentsInChildren<TextMeshProUGUI>());
            RandomPrice();
        }

        private void OnDisable()
        {
            Initialize();
        }
        #endregion

        // �޼���
        #region public
        // ������DB�� ������ ������ �����ϰ� ����
        public void RandomPrice()
        {
            Initialize();
            for (int i = 0; i < elixirsPrice.Count; i++)
            {
                int deviation = Random.Range(-20, 21);
                itemDatabase.itemList[i + elixirsIndex[0]].itemPrice += deviation;
                elixirsText[i].text = itemDatabase.itemList[i + elixirsIndex[0]].itemPrice.ToString() + "G";
            }
        }
        #endregion
        #region private
        // ����Ʈ �������� �ʱ�ȭ
        private void Initialize()
        {
            for (int i = 0; i < elixirsPrice.Count; i++)
            {
                itemDatabase.itemList[i + elixirsIndex[0]].itemPrice = elixirsPrice[i];
            }
        }

        // ������DB�κ��� �������� �о�� ���� ��ųʸ��� ����
        private void SetElixirs()
        {
            for (int i = 0; i < itemDatabase.itemList.Count; i++)
            {
                if (itemDatabase.itemList[i].itemID - identifier > 0)
                {
                    elixirsIndex.Add(i);
                    elixirsPrice.Add(itemDatabase.itemList[i].itemPrice);
                }
            }
        }
        #endregion
    }
}