using InnerSight_Kys;
using Noah;
using UnityEngine;

public class TutorialExit : MonoBehaviour
{
    [SerializeField]
    private string targetSceneName = "PlayScene"; // ��ȯ�� �� �̸�

    private void Awake()
    {
        AudioManager.Instance.PlayBgm("MapBgm");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            ChangeScene();
    }
    private void ChangeScene()
    {

        AudioManager.Instance.Play("GameStart");
        SceneFade.instance.FadeOut(targetSceneName);
    }
}
