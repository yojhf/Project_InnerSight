using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class TransformMovementWithGroundAndLook : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float runSpeed = 8f;
    public float jumpHeight = 2f;
    public float gravity = -9.81f;
    public LayerMask groundLayer;
    public float groundCheckDistance = 0.2f;
    public Transform cameraTransform;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private Vector2 moveInput;
    private Vector2 lookInput;
    private bool isRunning;

    private void Start()
    {
        controller = GetComponent<CharacterController>();

        // Step Offset 자동 조정
        controller.stepOffset = Mathf.Min(controller.height + controller.radius * 2, controller.stepOffset);

        if (cameraTransform == null)
        {
            Debug.LogError("Camera Transform is not assigned.");
        }
    }

    private void Update()
    {
        if (!controller.enabled) return; // 비활성화된 경우 로직 중단

        Move();
        LookAround();
        ApplyGravity();
    }

    private void Move()
    {
        if (!controller.enabled) return;

        float currentSpeed = isRunning ? runSpeed : walkSpeed;
        Vector3 moveDirection = (transform.forward * moveInput.y + transform.right * moveInput.x).normalized;
        controller.Move(moveDirection * currentSpeed * Time.deltaTime);
    }

    private void LookAround()
    {
        if (cameraTransform == null) return;

        float yaw = lookInput.x * 3f;
        float pitch = -lookInput.y * 3f;

        Vector3 cameraRotation = cameraTransform.localEulerAngles;
        cameraRotation.x += pitch;
        cameraRotation.x = Mathf.Clamp(cameraRotation.x, -60f, 60f);
        cameraTransform.localEulerAngles = cameraRotation;

        transform.Rotate(0, yaw, 0);
    }

    private void ApplyGravity()
    {
        isGrounded = Physics.CheckSphere(transform.position, groundCheckDistance, groundLayer);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    public void OnLook(InputValue value)
    {
        lookInput = value.Get<Vector2>();
    }

    public void OnJump(InputValue value)
    {
        if (isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    public void OnRun(InputValue value)
    {
        isRunning = value.isPressed;
    }
}
