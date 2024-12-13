using UnityEngine;

public class InGameUI_CatUIActive : MonoBehaviour
{
    [SerializeField] private GameObject catUI;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            catUI.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            catUI.SetActive(false);
        }
    }
}
