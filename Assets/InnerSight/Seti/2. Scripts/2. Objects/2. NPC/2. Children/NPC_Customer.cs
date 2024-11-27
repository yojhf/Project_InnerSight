using UnityEngine;
using UnityEngine.AI;

namespace InnerSight_Seti
{
    /// <summary>
    /// 손님 NPC의 행동논리를 정의하는 클래스
    /// </summary>
    public class NPC_Customer : NPC
    {
        // 필드
        #region Variables
        // NPC 기능
        private NavMeshAgent agent;

        private Transform frontOfPlayer;
        #endregion

        // 라이프 사이클
        #region Life Cycle
        private void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            agent.SetDestination(frontOfPlayer.position);
        }
        #endregion

        // 오버라이드
        // 플레이어와의 상호작용
        public override void Interaction()
        {
            throw new System.NotImplementedException();
        }

        // AI 행동논리
        protected override void AIBehaviour()
        {
            
        }

        // 메서드
        #region Methods
        public void SetFrontOfPlayer(Transform frontOfPlayer)
        {
            this.frontOfPlayer = frontOfPlayer;
        }
        #endregion

        // 이벤트 메서드
        #region Event Methods
        #endregion
    }
}