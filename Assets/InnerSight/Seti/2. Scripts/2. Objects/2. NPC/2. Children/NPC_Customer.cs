using UnityEngine;
using UnityEngine.AI;

namespace InnerSight_Seti
{
    /// <summary>
    /// �մ� NPC�� �ൿ���� �����ϴ� Ŭ����
    /// </summary>
    public class NPC_Customer : NPC
    {
        // �ʵ�
        #region Variables
        // NPC ���
        private NavMeshAgent agent;

        private Transform frontOfPlayer;
        #endregion

        // ������ ����Ŭ
        #region Life Cycle
        private void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            agent.SetDestination(frontOfPlayer.position);
        }
        #endregion

        // �������̵�
        // �÷��̾���� ��ȣ�ۿ�
        public override void Interaction()
        {
            throw new System.NotImplementedException();
        }

        // AI �ൿ��
        protected override void AIBehaviour()
        {
            
        }

        // �޼���
        #region Methods
        public void SetFrontOfPlayer(Transform frontOfPlayer)
        {
            this.frontOfPlayer = frontOfPlayer;
        }
        #endregion

        // �̺�Ʈ �޼���
        #region Event Methods
        #endregion
    }
}