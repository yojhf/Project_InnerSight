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
        [SerializeField] private GameObject NPC_Customer_Prefab;
        [SerializeField] private Transform frontOfPlayer;
        [SerializeField] private Transform enterPoint;
        [SerializeField] private Transform exitPoint;

        private NPC_Customer NPC_Customer;
        private GameObject customer;
        #endregion

        // ������ ����Ŭ
        #region Life Cycle
        private void Awake()
        {
            GenNPC();
        }
        #endregion

        // �޼���
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