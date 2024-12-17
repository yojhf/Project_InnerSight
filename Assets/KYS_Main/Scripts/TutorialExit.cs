using UnityEngine;
using UnityEngine.SceneManagement; // 씬 전환에 필요

public class TutorialExit : MonoBehaviour
{
    [SerializeField]
    private string targetSceneName = "PlayScene"; // 전환할 씬 이름

    [SerializeField]
    private Transform targetObject; // 타겟 오브젝트의 위치

    [SerializeField]
    private float triggerDistance = 3.0f; // 플레이어와 오브젝트 간 거리 감지 범위

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
            Debug.LogWarning("Target Object가 설정되지 않았습니다!");
            return false;
        }

        // 현재 오브젝트와 타겟 오브젝트 간 거리 계산
        float distance = Vector3.Distance(transform.position, targetObject.position);
        return distance <= triggerDistance;
    }

    private void ChangeScene()
    {
        Debug.Log("PlayScene으로 이동합니다!");
        SceneManager.LoadScene(targetSceneName); // 씬 변경
    }
}
