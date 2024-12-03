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
    /// �̱���: ��ǰ �ε��� ����
    public class NPC_Customer : NPC
    {
        // �ʵ�
        #region Variables
        // NPC ���� ����
        [SerializeField]
        private int NPC_ID;
        [SerializeField]
        private Database_NPC npcDatabase;
        private Item NPC_wannaItem;

        // NPC ���
        private NavMeshAgent agent;
        private NPC_Manager npcManager;
        private NPC_Behaviour npcState;
        private Vector3 targetPoint;    // ���� �̵� Ÿ�� ����

        // ����
        private Transform centerOfShop;
        private List<ShelfStorage> shopItems = new();
        private ShelfStorage thisItem;
        private int currentIndex;
        private bool isThisItem = false;
        
        [SerializeField]
        private float checkDelay = 3f;
        #endregion

        // ������ ����Ŭ
        #region Life Cycle
        private void Start()
        {
            NPC_wannaItem = npcDatabase.NPC_List.
                            Find(key => key.NPC_ID == NPC_ID).
                            NPC_Item_Want.GetComponent<Item>();
        }

        private void Update()
        {
            AIStateChange();
        }

        private void Awake()
        {
            npcManager = FindFirstObjectByType<NPC_Manager>();
            agent = GetComponent<NavMeshAgent>();
        }

        private void OnEnable()
        {
            AIBehaviour(NPC_Behaviour.SHOWING);
            currentIndex = 0;
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
                    break;

                case NPC_Behaviour.OUTSIDE:
                    targetPoint = npcManager.point_Disable.position;
                    agent.SetDestination(targetPoint);
                    break;

                case NPC_Behaviour.BROWSING:
                    BrowsingShop();
                    break;

                case NPC_Behaviour.CHECKING:
                    StartCoroutine(CheckID());
                    break;

                case NPC_Behaviour.PURCHASING:
                    thisItem.RemoveObject();
                    AIBehaviour(NPC_Behaviour.EXITING);
                    if (!thisItem.IsCanBuy)
                    {
                        AIBehaviour(NPC_Behaviour.BROWSING);
                    }
                    else
                    {
                        Debug.Log(PlayerStats.Instance);
                        PlayerStats.Instance.EarnGold(NPC_wannaItem.GetItemData().itemPrice);
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
            // thisItem ����
            thisItem = shopItems[currentIndex % shopItems.Count];
            agent.SetDestination(FrontOfItem(thisItem.transform));
            currentIndex++;
        }

        // ID Ȯ��
        IEnumerator CheckID()
        {
            float timeStamp = Time.time;
            while (timeStamp + checkDelay > Time.time)
            {
                yield return null;
            }
            isThisItem = (NPC_wannaItem.ItemId == thisItem.keyId);

            if (isThisItem) AIBehaviour(NPC_Behaviour.PURCHASING);
            else AIBehaviour(NPC_Behaviour.BROWSING);

            yield break;
        }

        Vector3 FrontOfItem(Transform itemTransform)
        {
            Vector3 offset = (centerOfShop.position - itemTransform.position).normalized * 0.5f;
            return itemTransform.position + offset;
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
    }
}