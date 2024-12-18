using Noah;
using UnityEngine;
using UnityEngine.SceneManagement; // 씬 전환에 필요

public class TutorialExit : MonoBehaviour
{
    [SerializeField]
    private string targetSceneName = "PlayScene"; // 전환할 씬 이름

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
