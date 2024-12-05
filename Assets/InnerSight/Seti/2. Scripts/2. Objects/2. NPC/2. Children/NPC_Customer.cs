using Noah;
using System.Collections;
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
        CHECKING,
        PURCHASING,
        EXITING,
        DISABLE
    }

    /// <summary>
    /// 손님 NPC의 행동논리를 정의하는 클래스
    /// </summary>
    /// 미구현: 상품 인덱스 셔플
    public class NPC_Customer : NPC
    {
        // 필드
        #region Variables
        // NPC 존재 정의
        [SerializeField]
        private int NPC_ID;
        [SerializeField]
        private Database_NPC npcDatabase;
        private ItemKey NPC_wannaItem;
        private Animator animator;

        // NPC 기능
        private NavMeshAgent agent;
        private NPC_Manager npcManager;
        private NPC_Behaviour npcState;
        private Vector3 targetPoint;    // 현재 이동 타겟 지점

        // 쇼핑
        private Transform centerOfShop;
        private List<ShelfStorage> shopItems = new();
        private ShelfStorage thisItem;
        [SerializeField]
        private int currentIndex;
        [SerializeField]
        private int whichDir;           // 순회 방향: -1: 반시계, 0: 바로, 1: 시계
        private bool isThisItem = false;
        [SerializeField]
        private float checkDelay = 2f;

        // n차 순회 처리용 불리언
        private bool isFirst;
        #endregion

        // 라이프 사이클
        #region Life Cycle
        private void Start()
        {
            Item wannaItem = npcDatabase.NPC_List.
                             Find(key => key.NPC_ID == NPC_ID).
                             NPC_Item_Want.GetComponent<Item>();
            int wannaItemID = wannaItem.ItemId;
            NPC_wannaItem = wannaItem.
                            itemDatabase.
                            itemList.Find(key => key.itemID == wannaItemID);
        }

        private void Update()
        {
            AIStateChange();
        }

        private void Awake()
        {
            npcManager = FindFirstObjectByType<NPC_Manager>();
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
        }

        private void OnEnable()
        {
            AIBehaviour(NPC_Behaviour.SHOWING);
            whichDir = Random.Range(-1, 2);
            currentIndex = 0;
            isFirst = true;
            
            Move(true);
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
                    targetPoint = npcManager.points_Behaviour[2].position;
                    agent.SetDestination(targetPoint);
                    Move(true);
                    break;

                case NPC_Behaviour.OUTSIDE:
                    targetPoint = npcManager.point_Disable.position;
                    agent.SetDestination(targetPoint);
                    break;

                case NPC_Behaviour.BROWSING:
                    BrowsingShop();
                    Move(true);
                    break;

                case NPC_Behaviour.CHECKING:
                    Move(false);
                    StartCoroutine(CheckID());
                    break;

                case NPC_Behaviour.PURCHASING:
                    thisItem.RemoveObject();
                    if (!thisItem.IsCanBuy)
                    {
                        AIBehaviour(NPC_Behaviour.BROWSING);
                    }
                    else
                    {
                        PlayerStats.Instance.EarnGold(NPC_wannaItem.itemPrice);
                        AIBehaviour(NPC_Behaviour.EXITING);
                    }
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
                    case NPC_Behaviour.SHOWING:
                        AIBehaviour(NPC_Behaviour.ENTERING);
                        break;

                    case NPC_Behaviour.ENTERING:
                        AIBehaviour(NPC_Behaviour.BROWSING);
                        break;

                    case NPC_Behaviour.EXITING:
                        AIBehaviour(NPC_Behaviour.OUTSIDE);
                        break;

                    case NPC_Behaviour.OUTSIDE:
                        AIBehaviour(NPC_Behaviour.DISABLE);
                        break;

                    case NPC_Behaviour.BROWSING:
                        AIBehaviour(NPC_Behaviour.CHECKING);
                        break;
                }
            }
        }

        // 상점 둘러보기
        void BrowsingShop()
        {
            switch (whichDir)
            {
                case 0:
                    if (isFirst)
                    {
                        ShelfStorage isItem = CollectionUtility.FirstOrDefault(
                                          shopItems, item => NPC_wannaItem.itemID == item.keyId);
                        currentIndex = shopItems.IndexOf(isItem);
                        isFirst = false;
                        whichDir = (currentIndex <= shopItems.Count / 2f) ? 1 : -1;
                    }
                    break;
                case 1:
                    if (isFirst)
                    {
                        isFirst = false;
                        break;
                    }
                    currentIndex++;
                    break;
                case -1:
                    if (currentIndex <= 0)
                        currentIndex += shopItems.Count;
                    currentIndex--;
                    break;
            }

            thisItem = shopItems[currentIndex % shopItems.Count];
            agent.SetDestination(FrontOfItem(thisItem.transform));
        }

        // ID 확인
        IEnumerator CheckID()
        {
            float timeStamp = Time.time;
            while (timeStamp + checkDelay > Time.time)
            {
                yield return null;
            }
            isThisItem = (NPC_wannaItem.itemID == thisItem.keyId);

            if (isThisItem) AIBehaviour(NPC_Behaviour.PURCHASING);
            else AIBehaviour(NPC_Behaviour.BROWSING);

            yield break;
        }
        #endregion

        // 이벤트 메서드
        #region Event Methods
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Shop"))
            {
                centerOfShop = other.transform.GetChild(0);
                shopItems.AddRange(other.transform.GetComponentsInChildren<ShelfStorage>());
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

        // 기타 유틸리티
        #region Utilities
        private void Move(bool isMove)
        {
            if (animator.GetBool("IsMove") == isMove) return;
            animator.SetBool("IsMove", isMove);
        }

        // 아이템 선반 앞 위치 계산
        Vector3 FrontOfItem(Transform itemTransform)
        {
            Vector3 offset = (centerOfShop.position - itemTransform.position).normalized * 0.5f;
            return itemTransform.position + offset;
        }
        #endregion
    }
}