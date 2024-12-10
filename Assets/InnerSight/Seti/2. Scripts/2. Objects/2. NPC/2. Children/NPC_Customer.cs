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
    public class NPC_Customer : NPC
    {
        // 필드
        #region Variables
        // NPC 존재 정의
        [SerializeField] private int NPC_ID;
        [SerializeField] private Database_NPC npcDatabase;
        private ItemKey NPC_currentWant;
        private List<ItemKey> NPC_wants = new();

        // NPC 기능
        private Animator animator;
        private NavMeshAgent agent;
        private NPC_Manager npcManager;
        private NPC_Behaviour npcState;
        private Vector3 targetPoint;    // 현재 이동 타겟 지점

        // 쇼핑
        private int currentOrder;       // 쇼핑 우선순위
        private int currentIndex;       // 아이템 선반 Index
        private int whichDir;           // 순회 방향: -1: 반시계, 0: 바로, 1: 시계
        private bool isThisItem = false;
        private const float checkDelay = 2f;
        private readonly List<ShelfStorage> shopItems = new();
        private Transform centerOfShop;
        private ShelfStorage thisItem;

        // n차 순회 처리용 불리언
        private bool isFirst;
        #endregion

        // 속성
        #region Properties
        public Key_NPC NPCKey { get; private set; }
        #endregion

        // 라이프 사이클
        #region Life Cycle
        private void Update()
        {
            AIStateChange();
        }

        private void Awake()
        {
            // 존재 정의
            NPCKey = npcDatabase.NPC_List.Find(key => key.NPC_ID == NPC_ID);

            // 초기화
            npcManager = FindFirstObjectByType<NPC_Manager>();
            animator = GetComponent<Animator>();
            agent = GetComponent<NavMeshAgent>();
        }

        private void OnEnable()
        {
            Initialize();
        }

        private void OnDisable()
        {
            NPC_wants.Clear();
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
                        SetNextWant();
                        AIBehaviour(NPC_Behaviour.BROWSING);
                    }
                    else
                    {
                        PlayerStats.Instance.EarnGold(NPC_currentWant.itemPrice);
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
                                          shopItems, item => NPC_currentWant.itemID == item.keyId);
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
            isThisItem = (NPC_currentWant.itemID == thisItem.keyId);

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
        private void Initialize()
        {
            NPC_wants.Add(SetItem(NPCKey.NPC_Item_Want_First));
            NPC_wants.Add(SetItem(NPCKey.NPC_Item_Want_Second));
            NPC_wants.Add(SetItem(NPCKey.NPC_Item_Want_Third));

            AIBehaviour(NPC_Behaviour.SHOWING);
            whichDir = Random.Range(-1, 2);
            currentOrder = Random.Range(0, 2);
            currentIndex = 0;
            isFirst = true;
            SetNextWant();
            Move(true);
        }

        private void Move(bool isMove)
        {
            if (animator.GetBool("IsMove") == isMove) return;
            animator.SetBool("IsMove", isMove);
        }

        private ItemKey SetItem(GameObject item)
        {
            return item.GetComponent<Item>().itemDatabase.itemList.Find(key => key.itemID == item.GetComponent<Item>().ItemId);
        }

        // 현재 원하는 아이템 세팅
        private void SetNextWant()
        {
            NPC_currentWant = NPC_wants[currentOrder];
            NPC_wants.Remove(NPC_wants[currentOrder]);
            currentOrder = 0;
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