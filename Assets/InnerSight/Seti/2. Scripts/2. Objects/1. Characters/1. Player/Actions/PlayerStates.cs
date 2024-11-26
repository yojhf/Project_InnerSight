using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace InnerSight_Seti
{
    // 플레이어의 상태를 관리하는 클래스
    // 플레이어의 각종 불리언 변수와 스탠스를 관리한다
    public class PlayerStates
    {
        // 필드
        #region Variables
        // 단순 변수
        private int initialColl = 1000;     // 초기 소지금
        private int currentColl;            // 현재 소지금

        // 불리언 변수
        public bool isGrounded = true;      // 지면 판정
        public bool isClimbing;             // 등반 판정
        public bool isKeepGoing;            // KeepGoing 기능 활성화 플래그
        public bool isShopEnter;            // 상점 입장 여부 판정
        public bool? isBoard;               // 라이딩기어 탑승 및 타입 판정 nullable 플래그, true = 보드, false = 부츠, null = 기본

        // 클래스 컴포넌트
        private readonly Player player;
        #endregion

        // 속성
        #region Properties
        public int CurrentColl => currentColl;
        #endregion

        // 생성자
        #region Constructor
        public PlayerStates(Player player)
        {
            this.player = player;
            currentColl = initialColl;
        }
        #endregion

        // 이벤트 핸들러
        #region Event Handlers
        public void OnRestartStarted(InputAction.CallbackContext _) => SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        #endregion

        // 메서드
        #region Methods
        public void EarnColl(int amount)
        {
            currentColl += amount;
            player.InfoUI.UpdateDeviceUI();
        }

        public bool SpendColl(int amount)
        {
            if (currentColl < amount)
            {
                Debug.Log($"Spend - currentColl: {currentColl}, amount: {amount}");
                return false;
            }
            else
            {
                currentColl -= amount;
                player.InfoUI.UpdateDeviceUI();
                return true;
            }
        }

        public void SetThisGear(bool? isBoard)
        {
            this.isBoard = isBoard;
        }
        #endregion
    }
}