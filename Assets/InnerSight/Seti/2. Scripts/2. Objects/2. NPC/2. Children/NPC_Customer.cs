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
    /// �մ� NPC�� �ൿ���� �����ϴ� Ŭ����
    /// </summary>
    public class NPC_Customer : NPC
    {
        // �ʵ�
        #region Variables
        // NPC ���� ����
        [SerializeField] private int NPC_ID;
        [SerializeField] private Database_NPC npcDatabase;
        private ItemKey NPC_currentWant;
        private List<ItemKey> NPC_wants = new();

        // NPC ���
        private Animator animator;
        private NavMeshAgent agent;
        private NPC_Manager npcManager;
        private NPC_Behaviour npcState;
        private Vector3 targetPoint;    // ���� �̵� Ÿ�� ����

        // ����
        private int currentOrder;       // ���� �켱����
        private int currentIndex;       // ������ ���� Index
        private int whichDir;           // ��ȸ ����: -1: �ݽð�, 0: �ٷ�, 1: �ð�
        private bool isThisItem = false;
        private const float checkDelay = 2f;
        private readonly List<ShelfStorage> shopItems = new();
        private Transform centerOfShop;
        private ShelfStorage thisItem;

        // n�� ��ȸ ó���� �Ҹ���
        private bool isFirst;
        #endregion

        // �Ӽ�
        #region Properties
        public Key_NPC NPCKey { get; private set; }
        #endregion

        // ������ ����Ŭ
        #region Life Cycle
        private void Update()
        {
            AIStateChange();
        }

        private void Awake()
        {
            // ���� ����
            NPCKey = npcDatabase.NPC_List.Find(key => key.NPC_ID == NPC_ID);

            // �ʱ�ȭ
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

        // �������̵�
        #region Override
        // �÷��̾���� ��ȣ�ۿ�
        public override void Interaction()
        {
            throw new System.NotImplementedException();
        }

        // AI �ൿ��
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

        // �޼���
        #region Methods
        // ���� ��ȯ
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

        // ���� �ѷ�����
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

        // ID Ȯ��
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

        // �̺�Ʈ �޼���
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

        // ��Ÿ ��ƿ��Ƽ
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

        // ���� ���ϴ� ������ ����
        private void SetNextWant()
        {
            NPC_currentWant = NPC_wants[currentOrder];
            NPC_wants.Remove(NPC_wants[currentOrder]);
            currentOrder = 0;
        }

        // ������ ���� �� ��ġ ���
        Vector3 FrontOfItem(Transform itemTransform)
        {
            Vector3 offset = (centerOfShop.position - itemTransform.position).normalized * 0.5f;
            return itemTransform.position + offset;
        }
        #endregion
    }
}