using UnityEngine;
using UnityEngine.InputSystem;

namespace InnerSight_Seti
{
    // 플레이어의 이동 제어를 관리하는 클래스
    public class PlayerMove
    {
        // 필드
        #region Variables
        // 단순 변수
        private readonly float walkSpeed;
        private readonly float jumpForce;

        // 복합 변수
        private Vector2 moveInput;          // 이동 입력

        // 컴포넌트
        private readonly Rigidbody rb;

        // 클래스 컴포넌트
        private readonly Player player;
        #endregion

        // 속성
        #region Properties
        public Vector2 MoveInput => moveInput;
        public Vector2 LastMoveDirection { get; set; }
        #endregion

        // 생성자
        #region Constructor
        public PlayerMove(Player player, float walkSpeed, float jumpForce)
        {
            this.player = player;
            this.rb = player.transform.GetComponent<Rigidbody>();
            this.walkSpeed = walkSpeed;
            this.jumpForce = jumpForce;
        }
        #endregion

        // 이벤트 핸들러
        #region Event Handlers
        public void OnMovePerformed(InputAction.CallbackContext context)
        {
            moveInput = context.ReadValue<Vector2>();
        }

        public void OnMoveCanceled(InputAction.CallbackContext _)
        {
            moveInput = Vector2.zero;
        }

        public void OnJumpStarted(InputAction.CallbackContext _)
        {
            if (player.playerStates.isGrounded)
            {
                Jump();
            }
        }
        #endregion

        // 메서드
        #region Methods
        // 플레이어의 기본 이동 메서드
        public void Move()
        {
            if (rb == null) return;
            
            Vector3 moveDirection = new(LastMoveDirection.x, 0, LastMoveDirection.y);
            Vector3 forward = player.transform.forward * moveDirection.z;
            Vector3 right = player.transform.right * moveDirection.x;
            Vector3 move = walkSpeed * Time.fixedDeltaTime * (forward + right).normalized;

            rb.MovePosition(player.transform.position + move);
        }

        // 플레이어의 기본 점프 메서드
        public void Jump()
        {
            if (rb == null) return;

            rb.AddForce(rb.transform.up * jumpForce, ForceMode.Impulse);
        }
        #endregion
    }
}