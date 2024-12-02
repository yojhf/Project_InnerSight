using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace InnerSight_Seti
{
    public enum NPC_Behaviour
    {
        OUTSIDE,
        SHOWING,
        ENTERING,
        BROWSING,
        PURCHASING,
        EXITING,
        DISABLE
    }

    /// <summary>
    /// 손님 NPC의 행동논리를 정의하는 클래스
    /// </summary>
    public class NPC_Customer : NPC
    {
        // 필드
        #region Variables
        // NPC 존재 정의
        [SerializeField]
        private int NPC_ID;
        [SerializeField]
        private Database_NPC npcDatabase;
        private Item NPC_wannaItem;

        // NPC 기능
        private NavMeshAgent agent;
        private NPC_Manager npcManager;
        private NPC_Behaviour npcState;
        private Vector3 targetPoint;    // 현재 이동 타겟 지점

        // 쇼핑
        private Transform centerOfShop;
        private List<Item> shopItems;
        #endregion

        // 라이프 사이클
        #region Life Cycle
        private void Update()
        {
            AIStateChange();
        }

        private void Awake()
        {
            npcManager = FindFirstObjectByType<NPC_Manager>();
            agent = GetComponent<NavMeshAgent>();
            NPC_wannaItem = npcDatabase.NPC_List.
                            Find(key => key.NPC_ID == NPC_ID).
                            NPC_Item_Want.GetComponent<Item>();
        }

        private void OnEnable()
        {
            AIBehaviour(NPC_Behaviour.SHOWING);
        }
        #endregion

        // 오버라이드
        #region Override
        // 플레이어와의 상호작용
        public override void Interaction()
        {
            throw new System.NotImplementedException();
        }

        // AI 행동논리
        protected override void AIBehaviour(NPC_Behaviour npcBehaviour)
        {
            npcState = npcBehaviour;

            switch (npcState)
            {
                case NPC_Behaviour.SHOWING:
                    targetPoint = npcManager.points_Behaviour[0].position;
                    agent.SetDestination(targetPoint);
                    break;

                case NPC_Behaviour.ENTERING:
                    targetPoint = npcManager.points_Behaviour[1].position;
                    agent.SetDestination(targetPoint);
                    break;

                case NPC_Behaviour.EXITING:
                    targetPoint = npcManager.points_Behaviour[0].position;
                    agent.SetDestination(targetPoint);
                    break;

                case NPC_Behaviour.OUTSIDE:
                    targetPoint = npcManager.point_Disable.position;
                    agent.SetDestination(targetPoint);
                    break;

                case NPC_Behaviour.BROWSING:
                    // 둘러보기
                    // 상점 내부 순회
                    // ItemID 확인
                    // 일치하는 게 있으면 PURCHASING
                    break;

                case NPC_Behaviour.PURCHASING:
                    break;

                case NPC_Behaviour.DISABLE:
                    npcManager.Pool_Push(gameObject);
                    break;
            }
        }
        #endregion

        // 메서드
        #region Methods
        // 상태 전환
        void AIStateChange()
        {
            if (agent.remainingDistance < 0.5f)
            {
                switch (npcState)
                {
                    case NPC_Behaviour.OUTSIDE:
                        AIBehaviour(NPC_Behaviour.DISABLE);
                        break;

                    case NPC_Behaviour.SHOWING:
                        AIBehaviour(NPC_Behaviour.ENTERING);
                        break;

                    case NPC_Behaviour.ENTERING:
                        AIBehaviour(NPC_Behaviour.EXITING);
                        break;

                    case NPC_Behaviour.EXITING:
                        AIBehaviour(NPC_Behaviour.OUTSIDE);
                        break;
                }
            }
        }

        // 상점 둘러보기
        void BrowsingShop()
        {

        }

        // ID 확인
        bool CheckID()
        {
            return (NPC_wannaItem);
        }

        Vector3 FrontOfItem(Transform itemTransform)
        {
            Vector3 offset = (centerOfShop.position - itemTransform.position).normalized * 0.5f;
            return itemTransform.position + offset;
        }
        #endregion

        // 이벤트 메서드
        #region Event Methods
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Shop"))
            {
                centerOfShop = other.transform.GetChild(0);
                shopItems.AddRange(other.transform.GetComponentsInChildren<Item>());
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Shop"))
            {
                shopItems.Clear();
            }
        }
        #endregion
    }
}