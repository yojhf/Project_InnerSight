using System.Collections.Generic;
using UnityEngine;

namespace InnerSight_Seti
{
    /// <summary>
    /// ���� NPC�� �θ� Ŭ����
    /// </summary>
    /// 1. ���ο��� �ٰ����� UI ����
    /// 2. UI �ȳ��� ���� Ű �Է��� �ϸ� �ŷ� ���� - �ŷ��� UI
    /// 3. ������ �Ǹ� ���� ��ǰ ���, ������ ���� ���� ��ǰ ���
    /// 4. ������ �����ϰ� ���� ��ư�� ������ �Һ� �� ������ ���� �̷� ����
    /// 5. �ŷ� ��!
    public abstract class NPC_Merchant : NPC
    {
        // �ʵ�
        #region Variables
        // �ŷ� ����
        protected bool OnTrade = false;
        protected bool CanTrade = false;
        protected const int standardDis = 15;
        [SerializeField] protected PlayerSetting player;

        // ������ �˻� ����
        protected int firstElixir;
        protected bool isFirstElixir = false;
        protected const int identifier = 4000;

        [SerializeField]
        protected ItemDatabase itemDatabase;
        public ShopManager shopManager;
        public Dictionary<ItemKey, ItemValueShop> shopDict = new();

        // �ӽ� - �÷��̾���� �Ÿ� ����
        public float distance;
        #endregion

        // ������ ����Ŭ
        #region Life Cycle
        protected virtual void Update()
        {
            distance = DistanceToPlayer();
            if (distance < standardDis)
            {
                CanTrade = true;
                player.Merchant = this;
            }
            else
            {
                CanTrade = false;
                player.Merchant = null;
            }
            shopManager.DistanceCheck(CanTrade);
        }
        #endregion

        // �������̵�
        #region Override
        public override void Interaction()
        {
            if (!CanTrade) return;
            shopManager.SwitchUI(OnTrade = !OnTrade);
            shopManager.SetPlayer(player);
        }

        protected override void AIBehaviour(NPC_Behaviour npcBehaviour)
        {
            
        }

        protected abstract void Initialize();
        #endregion

        // ��Ÿ ��ƿ��Ƽ
        #region Utilities
        float DistanceToPlayer() => Vector3.Distance(transform.position, player.transform.position);
        #endregion
    }
}