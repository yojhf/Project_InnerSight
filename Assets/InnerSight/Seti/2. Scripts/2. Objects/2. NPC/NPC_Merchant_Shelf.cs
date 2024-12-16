using JetBrains.Annotations;
using System.Collections.Generic;
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
        // �ʵ�
        #region Variables
        private int firstElixir;
        private bool isFirstElixir = false;
        private const int identifier = 4000;

        [SerializeField]
        private ItemDatabase itemDatabase;
        public ShelfShopManager shopManager;
        public Dictionary<ItemKey, ItemValueShop> shopDict = new();
        #endregion

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

        void Initialize()
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
                        itemCost = itemDatabase.itemList[i].itemPrice * 80
                    };
                    shopDict.Add(itemDatabase.itemList[i], valueShop);
                }
            }
        }
        #endregion
    }
}