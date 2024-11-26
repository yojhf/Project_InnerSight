using UnityEngine;

namespace InnerSight_Seti
{
    /// <summary>
    /// 각 UI의 중심 클래스
    /// </summary>
    public class UIManager : MonoBehaviour
    {
        // 필드
        #region Variables
        // 클래스
        [SerializeField] private Player player;
        [SerializeField] private PlayerUse playerUse;
        #endregion

        // 속성
        #region Properties
        public Player Player => player;
        public PlayerUse PlayerUse => playerUse;
        #endregion
    }
}