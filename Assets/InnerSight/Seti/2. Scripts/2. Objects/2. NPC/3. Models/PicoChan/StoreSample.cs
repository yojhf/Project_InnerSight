using UnityEngine;

namespace InnerSight_Seti
{
    public class StoreSample : MonoBehaviour
    {
        // 필드
        #region Variables
        // 상점 주인
        public NPC_Merchant_PicoChan picoChan;
        #endregion

        // 이벤트 메서드
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