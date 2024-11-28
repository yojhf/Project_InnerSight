using UnityEngine;

public class TransformMovementWithGroundAndLook : MonoBehaviour
{
    public float moveSpeed = 5f;          // �̵� �ӵ�
    public float groundCheckDistance = 1.5f; // ĳ���Ϳ� �ٴ� ���� �ִ� �Ÿ�
    public LayerMask groundLayer;        // �ٴ����� ����� ���̾�
    public float rotationSpeed = 3f;     // �þ� ȸ�� �ӵ�

    private Vector3 currentVelocity;     // ���� �̵� �ӵ� (SmoothDamp ��)
    private float pitch = 0f;            // ���� ȸ�� ����
    private float yaw = 0f;              // �¿� ȸ�� ����

    void Update()
    {
        Move();
        StickToGround();
        LookAround();
    }

    private void Move()
    {
        // �Է� �ޱ� (WASD �Է�)
        float moveX = Input.GetAxis("Horizontal"); // A/D Ű �Է� (��/��)
        float moveZ = Input.GetAxis("Vertical");   // W/S Ű �Է� (��/��)

        // �̵� ������ ī�޶��� ���� ��ǥ��� ��ȯ
        Vector3 forward = transform.forward;  // �� ����
        Vector3 right = transform.right;      // ���� ����

        // W = ��, S = ��, A = ��, D = ��
        Vector3 moveDirection = (forward * moveZ + right * moveX).normalized;

        // �̵� ó��
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);
    }

    private void StickToGround()
    {
        // Raycast�� �Ʒ��� ���� �ٴ� ����
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, groundCheckDistance, groundLayer))
        {
            // ĳ���͸� �ε巴�� �ٴ� ���� ��ġ��Ű��
            Vector3 targetPosition = new Vector3(transform.position.x, hit.point.y, transform.position.z);
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref currentVelocity, 0.1f);
        }
    }

    private void LookAround()
    {
        // ���콺 �Է� �ޱ� (�¿� ȸ��)
        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed;

        // ���� ȸ�� (�¿�)
        yaw += mouseX;
        // ���� ȸ�� (����)
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, -60f, 60f); // ���� ȸ�� ���� (���� -60~60)

        // ȸ�� ó�� (ȸ�� �ӵ� ����)
        transform.rotation = Quaternion.Euler(pitch, yaw, 0);
    }
}
