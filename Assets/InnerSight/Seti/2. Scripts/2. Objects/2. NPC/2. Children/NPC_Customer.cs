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
    /// 손님 NPC의 행동논리를 정의하는 클래스
    /// </summary>
    public class NPC_Customer : NPC
    {
        // 필드
        #region Variables
        // NPC 기능
        private NavMeshAgent agent;
        private NPC_Manager npcManager;
        private NPC_Behaviour npcBehaviour;
        #endregion

        // 라이프 사이클
        #region Life Cycle
        private void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            AIBehaviour(NPC_Behaviour.OUTSIDE);
        }
        #endregion

        // 오버라이드
        // 플레이어와의 상호작용
        public override void Interaction()
        {
            throw new System.NotImplementedException();
        }

        // AI 행동논리
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

        // 메서드
        #region Methods
        public void SetBehaviour(NPC_Manager npcManager)
        {
            this.npcManager = npcManager;
        }
        #endregion

        // 이벤트 메서드
        #region Event Methods
        #endregion
    }
}