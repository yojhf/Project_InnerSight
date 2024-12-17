namespace InnerSight_Seti
{
    /// <summary>
    /// ������ ����
    /// </summary>
    public class NPC_Merchant_Elixir : NPC_Merchant
    {
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
        // �ʱ�ȭ - ������DB�κ��� �������� �о�� ���� ��ųʸ��� ����
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
                    // ��ųʸ��� ����
                    ItemValueShop valueShop = new()
                    {
                        itemIndex = i - firstElixir,
                        itemCost = itemDatabase.itemList[i].itemPrice / 2,
                        itemCost_Knowhow = itemDatabase.itemList[i].itemPrice * 20,
                        itemKnowhow = false
                    };
                    shopDict.Add(itemDatabase.itemList[i], valueShop);
                }
            }
        }
        #endregion
    }
}