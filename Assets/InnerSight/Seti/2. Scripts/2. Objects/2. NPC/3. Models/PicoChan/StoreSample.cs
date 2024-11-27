using UnityEngine;

namespace InnerSight_Seti
{
    public class StoreSample : MonoBehaviour
    {
        // �ʵ�
        #region Variables
        // ���� ����
        public NPC_Merchant_PicoChan picoChan;
        #endregion

        // �̺�Ʈ �޼���
        #region Event Methods
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Player")) picoChan.PlayerEnter(other);
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Player")) picoChan.PlayerStay(other);
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Player")) picoChan.PlayerExit(other);
        }
        #endregion
    }
}