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
        [SerializeField] private GameObject NPC_Customer_Prefab;
        [SerializeField] private Transform frontOfPlayer;
        [SerializeField] private Transform enterPoint;
        [SerializeField] private Transform exitPoint;

        private NPC_Customer NPC_Customer;
        private GameObject customer;
        #endregion

        // 라이프 사이클
        #region Life Cycle
        private void Awake()
        {
            GenNPC();
        }
        #endregion

        // 메서드
        #region Methods
        void GenNPC()
        {
            customer = Instantiate(NPC_Customer_Prefab, enterPoint.position, Quaternion.identity);
            NPC_Customer = customer.GetComponent<NPC_Customer>();
            NPC_Customer.SetFrontOfPlayer(frontOfPlayer);
        }
        #endregion
    }
}