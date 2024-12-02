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
    /// �մ� NPC�� �ൿ���� �����ϴ� Ŭ����
    /// </summary>
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
        private List<Item> shopItems;
        #endregion

        // ������ ����Ŭ
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
                    targetPoint = npcManager.points_Behaviour[0].position;
                    agent.SetDestination(targetPoint);
                    break;

                case NPC_Behaviour.OUTSIDE:
                    targetPoint = npcManager.point_Disable.position;
                    agent.SetDestination(targetPoint);
                    break;

                case NPC_Behaviour.BROWSING:
                    // �ѷ�����
                    // ���� ���� ��ȸ
                    // ItemID Ȯ��
                    // ��ġ�ϴ� �� ������ PURCHASING
                    break;

                case NPC_Behaviour.PURCHASING:
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

        // ���� �ѷ�����
        void BrowsingShop()
        {

        }

        // ID Ȯ��
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

        // �̺�Ʈ �޼���
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