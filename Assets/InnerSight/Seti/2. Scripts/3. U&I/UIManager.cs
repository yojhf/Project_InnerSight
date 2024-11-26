using UnityEngine;

namespace InnerSight_Seti
{
    /// <summary>
    /// �� UI�� �߽� Ŭ����
    /// </summary>
    public class UIManager : MonoBehaviour
    {
        // �ʵ�
        #region Variables
        // Ŭ����
        [SerializeField] private Player player;
        [SerializeField] private PlayerUse playerUse;
        #endregion

        // �Ӽ�
        #region Properties
        public Player Player => player;
        public PlayerUse PlayerUse => playerUse;
        #endregion
    }
}