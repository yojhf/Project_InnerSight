using UnityEngine;
using UnityEngine.SceneManagement; // �� ��ȯ�� �ʿ�

public class TutorialExit : MonoBehaviour
{
    [SerializeField]
    private string targetSceneName = "PlayScene"; // ��ȯ�� �� �̸�

    [SerializeField]
    private Transform targetObject; // Ÿ�� ������Ʈ�� ��ġ

    [SerializeField]
    private float triggerDistance = 3.0f; // �÷��̾�� ������Ʈ �� �Ÿ� ���� ����

    private void Update()
    {
        if (IsPlayerNearTarget())
        {
            ChangeScene();
        }
    }

    private bool IsPlayerNearTarget()
    {
        if (targetObject == null)
        {
            Debug.LogWarning("Target Object�� �������� �ʾҽ��ϴ�!");
            return false;
        }

        // ���� ������Ʈ�� Ÿ�� ������Ʈ �� �Ÿ� ���
        float distance = Vector3.Distance(transform.position, targetObject.position);
        return distance <= triggerDistance;
    }

    private void ChangeScene()
    {
        Debug.Log("PlayScene���� �̵��մϴ�!");
        SceneManager.LoadScene(targetSceneName); // �� ����
    }
}
