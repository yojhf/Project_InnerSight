using UnityEngine;

public class TransformMovementWithGroundAndLook : MonoBehaviour
{
    public float moveSpeed = 5f;          // 이동 속도
    public float groundCheckDistance = 1.5f; // 캐릭터와 바닥 간의 최대 거리
    public LayerMask groundLayer;        // 바닥으로 사용할 레이어
    public float rotationSpeed = 3f;     // 시야 회전 속도

    private Vector3 currentVelocity;     // 현재 이동 속도 (SmoothDamp 용)
    private float pitch = 0f;            // 수평 회전 각도
    private float yaw = 0f;              // 좌우 회전 각도

    void Update()
    {
        Move();
        StickToGround();
        LookAround();
    }

    private void Move()
    {
        // 입력 받기 (WASD 입력)
        float moveX = Input.GetAxis("Horizontal"); // A/D 키 입력 (좌/우)
        float moveZ = Input.GetAxis("Vertical");   // W/S 키 입력 (앞/뒤)

        // 이동 방향을 카메라의 로컬 좌표계로 변환
        Vector3 forward = transform.forward;  // 앞 방향
        Vector3 right = transform.right;      // 우측 방향

        // W = 앞, S = 뒤, A = 좌, D = 우
        Vector3 moveDirection = (forward * moveZ + right * moveX).normalized;

        // 이동 처리
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);
    }

    private void StickToGround()
    {
        // Raycast를 아래로 쏴서 바닥 감지
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, groundCheckDistance, groundLayer))
        {
            // 캐릭터를 부드럽게 바닥 위로 위치시키기
            Vector3 targetPosition = new Vector3(transform.position.x, hit.point.y, transform.position.z);
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref currentVelocity, 0.1f);
        }
    }

    private void LookAround()
    {
        // 마우스 입력 받기 (좌우 회전)
        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed;

        // 수평 회전 (좌우)
        yaw += mouseX;
        // 수직 회전 (상하)
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, -60f, 60f); // 상하 회전 제한 (보통 -60~60)

        // 회전 처리 (회전 속도 조정)
        transform.rotation = Quaternion.Euler(pitch, yaw, 0);
    }
}
