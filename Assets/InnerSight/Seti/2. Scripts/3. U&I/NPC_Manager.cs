using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InnerSight_Seti
{
    /// <summary>
    /// NPC �ڵ� ������ ����ϴ� Ŭ����
    /// </summary>
    public class NPC_Manager : MonoBehaviour
    {
        // �ʵ�
        #region Variables
        [SerializeField] private Database_NPC database_NPC;
        public Transform Point_Entrance;
        public Transform Point_Enable;
        public Transform Point_Disable;

        // Ǯ��
        private Queue<GameObject> Pool_NPC = new();
        [SerializeField] int NPCPoolSize = 100;

        // NPC ���� �ֱ�
        bool isGenerating = false;
        [SerializeField] int NPC_GenMaxTime = 5;
        #endregion

        // ������ ����Ŭ
        #region Life Cycle
        private void Update()
        {
            if (isGenerating) return;
            StartCoroutine(GenNPC());
        }
        #endregion

        // �޼���
        #region Methods
        // NPC ����
        IEnumerator GenNPC()
        {
            isGenerating = true;

            float popPeriod = Random.Range(NPC_GenMaxTime / 2f, NPC_GenMaxTime);
            float timeStamp = Time.time;

            while (popPeriod + timeStamp > Time.time)
            {
                yield return null;
            }

            Pool_Pop();
            isGenerating = false;

            yield break;
        }

        // ������ NPC�� Ǯ�� �ǵ����� �޼���
        void Pool_Push(GameObject NPC)
        {
            if (Pool_NPC.Count < NPCPoolSize)
            {
                Pool_NPC.Enqueue(NPC);
            }

            else NPC_Destroy(NPC);
        }

        // Ǯ���� NPC�� ���� ��ȯ�ϴ� �޼���
        void Pool_Pop()
        {
            if (Pool_NPC.Count > 0)
            {
                Pool_NPC.Dequeue();
            }

            else NPC_Instantiate();
        }

        // NPC ���� �޼���
        void NPC_Instantiate()
        {
            // Database_NPC���� ������ NPC �̱�
            int popID = Random.Range(0, database_NPC.NPC_List.Count);
            GameObject popNPC = database_NPC.NPC_List[popID].NPC_Prefab;    // ������

            GameObject customer = Instantiate(popNPC, Point_Enable.position, Quaternion.identity);  // ���� ������Ʈ
            customer.GetComponent<NPC_Customer>().SetBehaviour(this);
        }

        // NPC �ı� �޼���
        void NPC_Destroy(GameObject NPC)
        {
            Destroy(NPC);
        }
        #endregion
    }
}