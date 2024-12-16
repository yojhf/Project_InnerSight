using System.Collections.Generic;
using UnityEngine;

namespace InnerSight_Seti
{
    /// <summary>
    /// ������ ����
    /// </summary>
    public class NPC_Merchant_Elixir : NPC_Merchant
    {
        private int firstElixir;
        private bool isFirstElixir = false;
        private const int identifier = 4000;
        [SerializeField]
        private ItemDatabase itemDatabase;
        public ElixirShopManager shopManager;
        public Dictionary<ItemKey, ItemValueShop> shopDict = new();

        private bool OnTrade = false;

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
        public override void Interaction()
        {
            base.Interaction();
            if (!CanTrade) return;
            shopManager.SwitchUI(OnTrade = !OnTrade);
            shopManager.SetPlayer(player);
        }

        protected override void AIBehaviour(NPC_Behaviour npcBehaviour)
        {
            base.AIBehaviour(npcBehaviour);
        }

        // �ʱ�ȭ - ������DB�κ��� �������� �о�� ���� ��ųʸ��� ����
        private void Initialize()
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