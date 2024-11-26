using UnityEngine;

namespace InnerSight_Seti
{
    // 플레이어의 중심 클래스
    // 플레이어의 각종 기능 클래스를 취합하고 총괄한다.
    public class Player : MonoBehaviour
    {
        // 필드
        #region Variables
        // 단순 변수
        private readonly float walkSpeed = 3f;
        private readonly float JumpForce = 350f;
        private readonly float mouseSensitivity = 0.1f;
        private readonly float syncSensitivity = 10f;

        // 컨트롤러 획득
        public Control control;

        [SerializeField]
        private Transform head;

        // 플레이어 기능 클래스
        private PlayerInteraction playerInteraction;
        private PlayerTrade playerTrade;
        private PlayerUse playerUse;
        public PlayerStates playerStates;
        public PlayerMove playerMove;
        public PlayerLook playerLook;

        // 유틸리티
        private CursorUtility cursorUtility;
        private DialogueUI dialogueUI;
        private InfoUI infoUI;
        #endregion

        // 속성
        #region Properties
        public PlayerUse PlayerUse => playerUse;
        public PlayerTrade PlayerTrade => playerTrade;
        public CursorUtility CursorUtility => cursorUtility;
        public DialogueUI DialogueUI => dialogueUI;
        public InfoUI InfoUI => infoUI;
        #endregion

        // 라이프 사이클
        #region Life Cycle
        private void Start()
        {
            // 마우스 커서 잠금
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void FixedUpdate()
        {
            // 이동 제어
            playerMove.Move();
        }

        private void Awake()
        {
            control = new();

            // MonoBehaviour 기능 클래스 가져오기
            playerInteraction = GetComponent<PlayerInteraction>();
            playerTrade = GetComponent<PlayerTrade>();
            playerUse = GetComponent<PlayerUse>();

            // 기능 클래스 인스턴스화
            playerStates = new(this);
            playerMove = new(this, walkSpeed, JumpForce);
            playerLook = new(this, head, mouseSensitivity, syncSensitivity);

            // 유틸리티 인스턴스화
            cursorUtility = new(this);
        }

        private void OnEnable()
        {
            // 이동 제어 이벤트 핸들러 구독
            control.Player.Move.performed += playerMove.OnMovePerformed;
            control.Player.Move.canceled += playerMove.OnMoveCanceled;
            control.Player.Jump.started += playerMove.OnJumpStarted;

            // 시야 제어 이벤트 핸들러 구독
            control.Player.Look.performed += playerLook.OnLookPerformed;
            control.Player.Look.canceled += playerLook.OnLookCanceled;

            // 상호작용 이벤트 핸들러 구독
            control.Player.Interaction.started += playerInteraction.OnInteractionStarted;

            // NPC 상점에서 아이템 선택 이벤트 핸들러 구독
            control.Player.CursorClick.started += playerTrade.OnLeftCursorClickStarted;

            // 마우스 커서 제어 이벤트 핸들러 구독
            control.Player.CursorSwitch.started += cursorUtility.OnCursorSwitchStarted;
            control.Player.CursorSwitch.canceled += cursorUtility.OnCursorSwitchCanceled;
            control.Player.CursorPosition.performed += cursorUtility.OnCursorPositionPerformed;

            // 컨트롤 활성화
            control.Player.Enable();
        }

        private void OnDisable()
        {
            // 컨트롤 비활성화
            control.Player.Disable();

            // 마우스 커서 제어 이벤트 핸들러 구독 해제
            control.Player.CursorSwitch.started -= cursorUtility.OnCursorSwitchStarted;
            control.Player.CursorSwitch.canceled -= cursorUtility.OnCursorSwitchCanceled;
            control.Player.CursorPosition.performed -= cursorUtility.OnCursorPositionPerformed;

            // NPC 상점에서 아이템 선택 이벤트 핸들러 구독
            control.Player.CursorClick.started -= playerTrade.OnLeftCursorClickStarted;

            // 상호작용 이벤트 핸들러 구독 해제
            control.Player.Interaction.started -= playerInteraction.OnInteractionStarted;

            // 시야 제어 이벤트 핸들러 구독 해제
            control.Player.Look.performed -= playerLook.OnLookPerformed;
            control.Player.Look.canceled -= playerLook.OnLookCanceled;

            // 이동 제어 이벤트 핸들러 구독 해제
            control.Player.Move.performed -= playerMove.OnMovePerformed;
            control.Player.Move.canceled -= playerMove.OnMoveCanceled;
            control.Player.Jump.started -= playerMove.OnJumpStarted;
        }
        #endregion

        // 메서드
        #region Methods
        // 플레이어 대사 출력 세팅
        public void SetDialogueUI(DialogueUI dialogueUI) => this.dialogueUI = dialogueUI;
        public void SetDeviceUI(InfoUI infoUI) => this.infoUI = infoUI;
        #endregion

        // 이벤트 메서드
        #region Event Methods
        // Collision 시리즈
        #region OnCollision
        private void OnCollisionChange(Collision collision, bool groundedState)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                playerMove.LastMoveDirection = playerMove.MoveInput;
                playerStates.isGrounded = groundedState;
            }
        }

        private void OnCollisionEnter(Collision collision) => OnCollisionChange(collision, true);
        private void OnCollisionStay(Collision collision) => OnCollisionChange(collision, true);
        private void OnCollisionExit(Collision collision) => OnCollisionChange(collision, false);
        #endregion
        #endregion
    }
}