using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace InnerSight_Seti
{
    /// <summary>
    /// ���� NPC�� �θ� Ŭ����
    /// </summary>
    /// 1. ���ο��� �ٰ����� UI ����
    /// 2. UI �ȳ��� ���� Ű �Է��� �ϸ� �ŷ� ���� - �ŷ��� UI
    /// 3. ������ �Ǹ� ���� ��ǰ ���, ������ ���� ���� ��ǰ ���
    /// 4. ������ �����ϰ� ���� ��ư�� ������ �Һ� �� ������ ���� �̷� ����
    /// 5. ���� ù �ŷ��� ��ΰ�(���ప+���Ͽ�), ����
    /// 6. �ŷ� ��!
    public class NPC_Merchant : NPC
    {
        // �ʵ�
        #region Variables
        private bool OnTrade = false;
        private bool CanTrade = false;
        private const int identifier = 4000;
        private const int standardDis = 20;
        [SerializeField] private ItemDatabase itemDatabase;
        [SerializeField] private PlayerSetting player;
        [SerializeField] private ShopUI shopUI;

        private int firstElixir;
        private bool isFirstElixir = false;
        public Dictionary<ItemKey,ItemValueShop> shopDict = new();



        public float distance;
        #endregion

        // ������ ����Ŭ
        #region Life Cycle
        private void Start()
        {
            Initialize();
            shopUI.SetItemInfo(shopDict);
        }

        private void Update()
        {
            distance = DistanceToPlayer();
            if (distance < standardDis)
            {
                CanTrade = true;
            }
            else CanTrade = false;

            if (Input.GetKeyDown(KeyCode.K))
                Interaction();
        }
        #endregion

        // �������̵�
        #region Override
        public override void Interaction()
        {
            if (!CanTrade) return;
            shopUI.SwitchUI(OnTrade = !OnTrade);
        }

        protected override void AIBehaviour(NPC_Behaviour npcBehaviour)
        {
            throw new System.NotImplementedException();
        }
        #endregion

        // �޼���
        #region Methods
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

        // �̺�Ʈ �޼���
        #region Event Methods
        #endregion

        // ��Ÿ ��ƿ��Ƽ
        #region Utilities
        float DistanceToPlayer() => Vector3.Distance(transform.position, player.transform.position);
        #endregion
    }
}