namespace InnerSight_Seti
{
    /// <summary>
    /// 엘릭서 상인
    /// </summary>
    public class NPC_Merchant_Elixir : NPC_Merchant
    {
        // 라이프 사이클
        #region Life Cycle
        private void Start()
        {
            Initialize();
            shopManager.SetItemInfo(shopDict);
        }
        #endregion

        // 메서드
        #region Methods
        // 초기화 - 아이템DB로부터 엘릭서를 읽어와 도감 딕셔너리에 저장
        protected override void Initialize()
        {
            // 데이터베이스를 순회하되
            for (int i = 0; i < itemDatabase.itemList.Count; i++)
            {
                // itemID > 4000인 아이템, 엘릭서만 읽고
                if (itemDatabase.itemList[i].itemID - identifier > 0)
                {
                    if (!isFirstElixir)
                    {
                        isFirstElixir = true;
                        firstElixir = i;
                    }
                    // 딕셔너리에 저장
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