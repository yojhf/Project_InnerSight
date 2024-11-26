using Unity.XR.CoreUtils;
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
        [SerializeField] private PlayerSetting player;
        [SerializeField] private PlayerUse playerUse;
        #endregion

        // �Ӽ�
        #region Properties
        public PlayerSetting Player => player;
        public PlayerUse PlayerUse => playerUse;
        #endregion
    }
}