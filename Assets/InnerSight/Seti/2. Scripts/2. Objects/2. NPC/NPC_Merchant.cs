using System.Collections.Generic;
using Unity.VRTemplate;
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
    public class NPC_Merchant : NPC
    {
        // �ʵ�
        #region Variables
        protected bool CanTrade = false;
        protected const int standardDis = 20;
        [SerializeField] protected PlayerSetting player;

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
        }
        #endregion

        // �������̵�
        #region Override
        public override void Interaction() { }

        protected override void AIBehaviour(NPC_Behaviour npcBehaviour) { }
        #endregion

        // ��Ÿ ��ƿ��Ƽ
        #region Utilities
        float DistanceToPlayer() => Vector3.Distance(transform.position, player.transform.position);
        #endregion
    }
}