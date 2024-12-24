using UnityEngine;

namespace InnerSight_Seti
{
    /// <summary>
    /// ���� �Ǹ� NPC
    /// </summary>
    /// �Ǹŵ� �� ��Ȱ��ȭ
    /// �� 6��
    /// ���� ��� �ٸ��� 8000 / 20000
    /// ���������� �ش� ���� Ȱ��ȭ
    /// 
    /// UI
    /// ���� 6��
    /// � ������? ����?
    /// �����Ͻðڽ��ϱ�?
    /// ���� �Ϸ�
    public class NPC_Merchant_Shelf : NPC_Merchant
    {
        [SerializeField]
        private int costFactor = 40;

        // ������ ����Ŭ
        #region Life Cycle
        private void Start()
        {
            Initialize();
            shopManager.SetItemInfo(shopDict);
        }
        #endregion

        // �޼���
        #region Methods
        protected override void Initialize()
        {
            // �����ͺ��̽��� ��ȸ�ϵ�
            for (int i = 0; i < itemDatabase.itemList.Count; i++)
            {
                // itemID > 4000�� ������, �������� �а�
                if (itemDatabase.itemList[i].itemID - identifier > 0)
                {
                    if (!isFirstElixir)
                    {
                        isFirstElixir = true;
                        firstElixir = i;
                    }
                    if (i - firstElixir < 2) continue; 
                    int thirdElixir = firstElixir + 2;

                    // ��ųʸ��� ����
                    ItemValueShop valueShop = new()
                    {
                        itemIndex = i - thirdElixir,
                        itemCost = Cost_Random.Instance.elixirsPrice[i - thirdElixir + 2] * costFactor
                    };
                    shopDict.Add(itemDatabase.itemList[i], valueShop);
                }
            }
        }
        #endregion
    }
}