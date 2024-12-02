using UnityEngine;
using UnityEngine.AI;

namespace InnerSight_Seti
{
    public enum NPC_Behaviour
    {
        OUTSIDE,
        ENTERING,
        BROWSING,
        PURCHASING,
        EXITING,
    }

    /// <summary>
    /// �մ� NPC�� �ൿ���� �����ϴ� Ŭ����
    /// </summary>
    public class NPC_Customer : NPC
    {
        // �ʵ�
        #region Variables
        // NPC ���
        private NavMeshAgent agent;
        private NPC_Manager npcManager;
        private NPC_Behaviour npcBehaviour;
        #endregion

        // ������ ����Ŭ
        #region Life Cycle
        private void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            AIBehaviour(NPC_Behaviour.OUTSIDE);
        }
        #endregion

        // �������̵�
        // �÷��̾���� ��ȣ�ۿ�
        public override void Interaction()
        {
            throw new System.NotImplementedException();
        }

        // AI �ൿ��
        protected override void AIBehaviour(NPC_Behaviour npcBehaviour)
        {
            switch (npcBehaviour)
            {
                case NPC_Behaviour.OUTSIDE:
                    agent.SetDestination(npcManager.Point_Entrance.position);
                    AIBehaviour(NPC_Behaviour.EXITING);
                    break;
                case NPC_Behaviour.ENTERING:
                    break;
                case NPC_Behaviour.BROWSING:
                    break;
                case NPC_Behaviour.PURCHASING:
                    break;
                case NPC_Behaviour.EXITING:
                    agent.SetDestination(npcManager.Point_Disable.position);
                    break;
            }
        }

        // �޼���
        #region Methods
        public void SetBehaviour(NPC_Manager npcManager)
        {
            this.npcManager = npcManager;
        }
        #endregion

        // �̺�Ʈ �޼���
        #region Event Methods
        #endregion
    }
}