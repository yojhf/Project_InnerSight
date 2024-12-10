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
        public Transform point_Disable;
        public Transform point_Enable;
        public Transform[] points_Behaviour; 

        // NPC ���� �ֱ�
        bool isPlaying = false;
        bool isGenerating = false;
        [SerializeField] int NPC_GenMaxTime = 5;
        List<GameObject> customers = new();
        #endregion

        // ������ ����Ŭ
        #region Life Cycle
        private void Update()
        {
            if (isGenerating || !isPlaying) return;
            StartCoroutine(GenNPC());
        }

        private void Awake()
        {
            // points�� ���� �ڽ� Ʈ�������� ���� �迭�� ��������
            var allPoints = transform.Find("Points").GetComponentsInChildren<Transform>();
            List<Transform> filteredPoints = new();
            foreach (var point in allPoints)
                if (point != transform.Find("Points").transform)
                    filteredPoints.Add(point);
            points_Behaviour = filteredPoints.ToArray();
        }

        private void OnEnable()
        {
            Initialize();
            isPlaying = true;
        }

        private void OnDisable()
        {
            isPlaying = false;
        }
        #endregion

        // �޼���
        #region Methods
        // NPC �ʱ�ȭ
        public void Initialize()
        {
            foreach (var npc in customers)
                Destroy(npc);
            customers.Clear();
        }

        // NPC ����
        IEnumerator GenNPC()
        {
            isGenerating = true;

            float popPeriod = Random.Range(NPC_GenMaxTime / 3f, NPC_GenMaxTime);
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
        public void Pool_Push(GameObject NPC)
        {
            /*if (NPC_Pool.Count < NPCPoolSize)
            {
                NPC.SetActive(false);
                NPC.transform.position = point_Disable.position;
                NPC_Pool.Enqueue(NPC);
            }

            else*/ NPC_Destroy(NPC);
        }

        // Ǯ���� NPC�� ���� ��ȯ�ϴ� �޼���
        void Pool_Pop()
        {
            /*if (NPC_Pool.Count > 0)
            {
                GameObject popNPC = NPC_Pool.Dequeue();
                popNPC.transform.position = point_Enable.position;
                popNPC.SetActive(true);
            }

            else*/ NPC_Instantiate();
        }

        // NPC ���� �޼���
        void NPC_Instantiate()
        {
            // Database_NPC���� ������ NPC �̱�
            int popID = Random.Range(0, database_NPC.NPC_List.Count);
            GameObject popNPC = database_NPC.NPC_List[popID].NPC_Prefab;    // ������

            GameObject thisNPC = Instantiate(popNPC,
                                             point_Enable.position,
                                             Quaternion.identity,
                                             transform.Find("NPC_Pool"));   // ���� ������Ʈ

            customers.Add(thisNPC);
        }

        // NPC �ı� �޼���
        void NPC_Destroy(GameObject NPC)
        {
            customers.Remove(NPC);
            Destroy(NPC);
        }
        #endregion
    }
}

#region Dummy
/*
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
        public Transform point_Disable;
        public Transform point_Enable;
        public Transform[] points_Behaviour; 

        // Ǯ��
        private readonly Queue<GameObject> NPC_Pool = new();
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

        private void Awake()
        {
            // points�� ���� �ڽ� Ʈ�������� ���� �迭�� ��������
            var allPoints = transform.Find("Points").GetComponentsInChildren<Transform>();
            List<Transform> filteredPoints = new();
            foreach (var point in allPoints)
                if (point != transform.Find("Points").transform)
                    filteredPoints.Add(point);
            points_Behaviour = filteredPoints.ToArray();
        }
        #endregion

        // �޼���
        #region Methods
        // NPC ����
        IEnumerator GenNPC()
        {
            isGenerating = true;

            float popPeriod = Random.Range(NPC_GenMaxTime / 3f, NPC_GenMaxTime);
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
        public void Pool_Push(GameObject NPC)
        {
            if (NPC_Pool.Count < NPCPoolSize)
            {
                NPC.SetActive(false);
                NPC.transform.position = point_Disable.position;
                NPC_Pool.Enqueue(NPC);
            }

            else NPC_Destroy(NPC);
        }

        // Ǯ���� NPC�� ���� ��ȯ�ϴ� �޼���
        void Pool_Pop()
        {
            if (NPC_Pool.Count > 0)
            {
                GameObject popNPC = NPC_Pool.Dequeue();
                popNPC.transform.position = point_Enable.position;
                popNPC.SetActive(true);
            }

            else NPC_Instantiate();
        }

        // NPC ���� �޼���
        void NPC_Instantiate()
        {
            // Database_NPC���� ������ NPC �̱�
            int popID = Random.Range(0, database_NPC.NPC_List.Count);
            GameObject popNPC = database_NPC.NPC_List[popID].NPC_Prefab;    // ������

            Instantiate(popNPC,
                        point_Enable.position,
                        Quaternion.identity,
                        transform.Find("NPC_Pool"));       // ���� ������Ʈ
        }

        // NPC �ı� �޼���
        void NPC_Destroy(GameObject NPC)
        {
            Destroy(NPC);
        }
        #endregion
    }
}
 */
#endregion