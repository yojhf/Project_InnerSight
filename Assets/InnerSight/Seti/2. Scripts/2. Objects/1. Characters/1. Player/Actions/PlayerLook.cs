using UnityEngine;
using UnityEngine.InputSystem;

namespace InnerSight_Seti
{
    // 플레이어의 시야 제어를 관리하는 클래스
    public class PlayerLook
    {
        // 필드
        #region Variables
        // 조정값
        private readonly float mouseSensitivity;     // 마우스 감도
        private readonly float syncSensitivity;      // Lerp, Slerp 보간 감도

        // 단순 변수
        private float headXRotation;        // head X축 회전값
        private float bodyYRotation;        // body Y축 회전값

        // 복합 변수
        private Vector2 lookInput;          // 마우스 입력

        // 컴포넌트
        private Transform head;             // 플레이어의 머리 부분 Transform
        private readonly Rigidbody rb;      // 플레이어 Rigidbody

        private readonly Player player;     // 플레이어 클래스 참조
        #endregion

        // 속성
        #region Properties
        public float XLookInput => lookInput.x;
        #endregion

        // 이벤트 핸들러
        #region Event Handlers
        public void OnLookPerformed(InputAction.CallbackContext context)
        {
            lookInput = context.ReadValue<Vector2>();
            Rotation();
        }

        public void OnLookCanceled(InputAction.CallbackContext _)
        {
            lookInput = Vector2.zero;
        }
        #endregion

        // 생성자
        #region Constructor
        public PlayerLook(Player player, Transform head, float mouseSensitivity, float syncSensitivity)
        {
            this.player = player;
            this.rb = player.transform.GetComponent<Rigidbody>();
            this.head = head;
            this.mouseSensitivity = mouseSensitivity;
            this.syncSensitivity = syncSensitivity;

            player.playerStates.isKeepGoing = false;                // 초기화 시 KeepGoing 상태 false로 설정
        }
        #endregion

        // 메서드
        #region Methods
        private void Rotation()
        {
            headXRotation -= lookInput.y * mouseSensitivity;            // X축의 Delta 값
            headXRotation = Mathf.Clamp(headXRotation, -50f, 50f);      // X축의 한계 회전각
            head.localRotation = Quaternion.Euler(headXRotation, 0f, 0f);

            bodyYRotation = lookInput.x * mouseSensitivity;
            rb.MoveRotation(rb.rotation * Quaternion.Euler(0f, bodyYRotation, 0f));
        }
        #endregion
    }
}