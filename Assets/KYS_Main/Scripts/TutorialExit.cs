using Noah;
using UnityEngine;
using UnityEngine.SceneManagement; // �� ��ȯ�� �ʿ�

public class TutorialExit : MonoBehaviour
{
    [SerializeField]
    private string targetSceneName = "PlayScene"; // ��ȯ�� �� �̸�

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            ChangeScene();
    }
    private void ChangeScene()
    {
        SceneFade.instance.FadeOut(targetSceneName);
    }
}
