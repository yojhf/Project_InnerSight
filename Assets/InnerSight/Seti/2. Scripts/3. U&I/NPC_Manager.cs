using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InnerSight_Seti
{
    /// <summary>
    /// NPC 자동 생성을 담당하는 클래스
    /// </summary>
    public class NPC_Manager : MonoBehaviour
    {
        // 필드
        #region Variables
        [SerializeField] private Database_NPC database_NPC;
        public Transform point_Disable;
        public Transform point_Enable;
        public Transform[] points_Behaviour; 

        // NPC 생성 주기
        bool isPlaying = false;
        bool isGenerating = false;
        [SerializeField] int NPC_GenMaxTime = 5;
        List<GameObject> customers = new();
        #endregion

        // 라이프 사이클
        #region Life Cycle
        private void Update()
        {
            if (isGenerating || !isPlaying) return;
            StartCoroutine(GenNPC());
        }

        private void Awake()
        {
            // points만 빼고 자식 트랜스폼을 전부 배열로 가져오기
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

        // 메서드
        #region Methods
        // NPC 초기화
        public void Initialize()
        {
            foreach (var npc in customers)
                Destroy(npc);
            customers.Clear();
        }

        // NPC 생성
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

        // 퇴장한 NPC를 풀로 되돌리는 메서드
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

        // 풀에서 NPC를 꺼내 소환하는 메서드
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

        // NPC 생성 메서드
        void NPC_Instantiate()
        {
            // Database_NPC에서 임의의 NPC 뽑기
            int popID = Random.Range(0, database_NPC.NPC_List.Count);
            GameObject popNPC = database_NPC.NPC_List[popID].NPC_Prefab;    // 프리팹

            GameObject thisNPC = Instantiate(popNPC,
                                             point_Enable.position,
                                             Quaternion.identity,
                                             transform.Find("NPC_Pool"));   // 실제 오브젝트

            customers.Add(thisNPC);
        }

        // NPC 파괴 메서드
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
    /// NPC 자동 생성을 담당하는 클래스
    /// </summary>
    public class NPC_Manager : MonoBehaviour
    {
        // 필드
        #region Variables
        [SerializeField] private Database_NPC database_NPC;
        public Transform point_Disable;
        public Transform point_Enable;
        public Transform[] points_Behaviour; 

        // 풀링
        private readonly Queue<GameObject> NPC_Pool = new();
        [SerializeField] int NPCPoolSize = 100;

        // NPC 생성 주기
        bool isGenerating = false;
        [SerializeField] int NPC_GenMaxTime = 5;
        #endregion

        // 라이프 사이클
        #region Life Cycle
        private void Update()
        {
            if (isGenerating) return;
            StartCoroutine(GenNPC());
        }

        private void Awake()
        {
            // points만 빼고 자식 트랜스폼을 전부 배열로 가져오기
            var allPoints = transform.Find("Points").GetComponentsInChildren<Transform>();
            List<Transform> filteredPoints = new();
            foreach (var point in allPoints)
                if (point != transform.Find("Points").transform)
                    filteredPoints.Add(point);
            points_Behaviour = filteredPoints.ToArray();
        }
        #endregion

        // 메서드
        #region Methods
        // NPC 생성
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

        // 퇴장한 NPC를 풀로 되돌리는 메서드
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

        // 풀에서 NPC를 꺼내 소환하는 메서드
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

        // NPC 생성 메서드
        void NPC_Instantiate()
        {
            // Database_NPC에서 임의의 NPC 뽑기
            int popID = Random.Range(0, database_NPC.NPC_List.Count);
            GameObject popNPC = database_NPC.NPC_List[popID].NPC_Prefab;    // 프리팹

            Instantiate(popNPC,
                        point_Enable.position,
                        Quaternion.identity,
                        transform.Find("NPC_Pool"));       // 실제 오브젝트
        }

        // NPC 파괴 메서드
        void NPC_Destroy(GameObject NPC)
        {
            Destroy(NPC);
        }
        #endregion
    }
}
 */
#endregion