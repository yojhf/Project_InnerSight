using UnityEngine;

namespace Noah
{
    public class PauseManager : MonoBehaviour
    {
        [SerializeField] private GameObject pauseUI;

        private void Update()
        {
            Pause();
        }

        void Pause()
        {
            if (InputActManager.Instance.IsPauseButtonDown() && !pauseUI.activeSelf)
            {
                pauseUI.SetActive(true);

                ResetManager.Instance.Pause();
            }
            else if (InputActManager.Instance.IsPauseButtonDown() && pauseUI.activeSelf)
            {
                pauseUI.SetActive(false);

                ResetManager.Instance.ResetPause();
            }
        }

        public void Resume()
        {
            pauseUI.SetActive(false);
            ResetManager.Instance.ResetPause();
        }

        public void MainMenu()
        {
            pauseUI.SetActive(false);
            ResetManager.Instance.ResetPause();

            Debug.Log("메인메뉴");
        }


    }

}
